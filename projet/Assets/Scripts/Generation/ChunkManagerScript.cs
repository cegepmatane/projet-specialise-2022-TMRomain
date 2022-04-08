using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;

public class ChunkManagerScript : MonoBehaviour
{
    int currentChunkGenerated = 0;
    public class Chunk
    {
        public int XPosition;
        public int YPosition;
        public TileGeneration tileScript;
        public bool isTileEnabled =false;
        public Chunk(int xPos, int yPos, TileGeneration script)
        {
            XPosition = xPos;
            YPosition = yPos;
            tileScript = script;
        }
        
    }
    [SerializeField]
    GameObject[] biomes;
    GameObject tileGeneratorPrefab;
    [SerializeField]
    int xTileSize;
    [SerializeField]
    int yTileSize;

    // [SerializeField]
    int mapSize =3;
    [SerializeField]
    int numberOfChunkToLoad =2;

    [SerializeField]
    Transform playerTransform;
    float2 playerChunk = new float2(1000,1000);
    private List<Chunk> allChunk = new List<Chunk>();
    
    bool baseChunkGenerated = false;
    PerlinNoiseGenerator noiseGenerator;
    float[,] perlinNoseGeneration;
    public void Awake(){
        noiseGenerator = GetComponent<PerlinNoiseGenerator>();
        noiseGenerator.pixWidth = 200;
        noiseGenerator.pixHeight = 200;
        perlinNoseGeneration = noiseGenerator.CalcNoise(0);
    }
    public void Start(){
        // GenerateBaseChunk();
    }
    private void FixedUpdate() {
        playerToChunkConverter();
    }
    // private void GenerateBaseChunk(){
    //     for (int x = 0-mapSize; x < mapSize; x++)
    //     {
    //         int xAdd = x * (xTileSize*10);
    //         for (int y = 0-mapSize; y < mapSize; y++)
    //         {
    //             int yAdd = y * (yTileSize*10);
    //               GameObject terrain = Instantiate(tileGeneratorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    //               TileGeneration tile = terrain.GetComponent<TileGeneration>();
    //               float3 offset = new float3(xAdd,0,yAdd);
    //               //Logic des tiles
    //               tile.mapOffset = offset;
    //               tile.xSize = xTileSize;
    //               tile.ySize = yTileSize;
    //             allChunk.Add(new Chunk(x,y,tile));
    //             tile.isTileEnabled =true;
    //             tile.StartGeneration(currentChunkGenerated);
    //             currentChunkGenerated++;
    //         }
    //     }
    //     baseChunkGenerated = true;    
    // }

    private void playerToChunkConverter(){
        int currentXChunk = (int)playerTransform.position.x / (xTileSize*10);
        int currentYChunk = (int)playerTransform.position.z / (yTileSize*10);
        if(playerChunk.x != currentXChunk || playerChunk.y != currentYChunk){
            playerChunk.x = currentXChunk;
            playerChunk.y= currentYChunk;
            UpdateCurrentChunk();
        }
    }

    private void UpdateCurrentChunk(){
        //  foreach (Chunk chunk in allChunk)
        // {
        //    if(playerChunk.x == chunk.XPosition && playerChunk.y == chunk.YPosition ){
        //         chunk.tileScript.isTileEnabled = true;
        //         chunk.tileScript.tileRenderLogic();
        //    }
        // }
        UpdateAdjacentChunk();
    }
    private void UpdateAdjacentChunk(){
        List<Chunk> adjacentChunkList = new List<Chunk>();
        List<Chunk> farChunkList = new List<Chunk>();
        farChunkList = new List<Chunk>(allChunk);
        for (int x = (int)playerChunk.x-numberOfChunkToLoad; x < (int)playerChunk.x+numberOfChunkToLoad; x++)
        {
           for (int y = (int)playerChunk.y-numberOfChunkToLoad; y < (int)playerChunk.y+numberOfChunkToLoad; y++)
            {
                bool asFindChunk = false;
                foreach (Chunk chunk in allChunk)
                {
                    if(x == chunk.XPosition && y == chunk.YPosition ){
                        asFindChunk=true;
                        adjacentChunkList.Add(chunk);
                        farChunkList.Remove(chunk);
                    }
                }
                if(asFindChunk == false){
                    Chunk generatedChunk = generateChunk(x,y);
                    adjacentChunkList.Add(generatedChunk);
                }
            } 
        }
        foreach (Chunk adjacentChunk in adjacentChunkList)
        {
            if(adjacentChunk.isTileEnabled == false){
                adjacentChunk.isTileEnabled =  true;
                adjacentChunk.tileScript.EnableTile();
                // adjacentChunk.tileScript.isTileEnabled = true;
                // adjacentChunk.tileScript.tileRenderLogic();
            }
        }
         foreach (Chunk farChunk in farChunkList)
        {
            if(farChunk.isTileEnabled == true){
                farChunk.isTileEnabled = false;
                farChunk.tileScript.DisableTile();
                // adjacentChunk.tileScript.isTileEnabled = true;
                // adjacentChunk.tileScript.tileRenderLogic();
            }
        }

    }

    private Chunk generateChunk(int x, int y)
    {
        int xAdd = x * (yTileSize*10);
        int yAdd = y * (yTileSize*10);
        GameObject terrain = Instantiate(chunkGenerator(x,y), new Vector3(0, 0, 0), Quaternion.identity);
        TileGeneration tile = terrain.GetComponent<TileGeneration>();
        float3 offset = new float3(xAdd,0,yAdd);
        //Logic des tiles
        tile.mapOffset = offset;
        tile.xSize = xTileSize;
        tile.ySize = yTileSize;
        Chunk generatedChunk = new Chunk(x,y,tile);
        generatedChunk.isTileEnabled = true;
        allChunk.Add(generatedChunk);
        // allChunk.Add(new Chunk(x,y,tile));
        tile.chunkID =currentChunkGenerated;
        currentChunkGenerated++;
        tile.StartGeneration(currentChunkGenerated);
        return generatedChunk;
    }


    bool estDansMarge(float valeur,float valeurSouhaiter,float marge){
        // Debug.Log(valeur);
        // Debug.Log(valeurSouhaiter+marge );
        // Debug.Log(valeurSouhaiter-marge);
        if( valeur <= valeurSouhaiter+marge ){
            if(valeurSouhaiter-marge <= valeur ){
                return true;
            }
        }
        return false;
    }
    GameObject chunkGenerator(int  x, int y){
        float positionNoise = perlinNoseGeneration[Mathf.Abs(x),Mathf.Abs(y)];
        // Debug.Log(Mathf.Abs(x));
        // Debug.Log(Mathf.Abs(y));
        if(positionNoise< 0.3){
            return biomes[0];
        }else if(positionNoise< 0.34){
            return biomes[1];
        }else{
            return biomes[2];
        }
    }
}
