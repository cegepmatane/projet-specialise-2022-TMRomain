using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;

public class ChunkManagerScript : MonoBehaviour
{
    public class Chunk
    {
        public int XPosition;
        public int YPosition;
        public TileGeneration tileScript;
        public Chunk(int xPos, int yPos, TileGeneration script)
        {
            XPosition = xPos;
            YPosition = yPos;
            tileScript = script;
        }
        
    }
    [SerializeField]
    GameObject tileGeneratorPrefab;
    [SerializeField]
    int xTileSize;
    [SerializeField]
    int yTileSize;

    [SerializeField]
    int mapSize =3;
    int numberOfChunkToLoad =2;

    [SerializeField]
    Transform playerTransform;
    float2 playerChunk = new float2(1000,1000);
    private List<Chunk> allChunk = new List<Chunk>();
    
    bool baseChunkGenerated = false;

    public void Start(){
        GenerateBaseChunk();
    }
    private void FixedUpdate() {
        if(baseChunkGenerated){
            playerToChunkConverter();
        }
    }
    private void GenerateBaseChunk(){
        for (int x = 0-mapSize; x < mapSize; x++)
        {
            int xAdd = x * (xTileSize*10);
            for (int y = 0-mapSize; y < mapSize; y++)
            {
                int yAdd = y * (yTileSize*10);
                  GameObject terrain = Instantiate(tileGeneratorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                  TileGeneration tile = terrain.GetComponent<TileGeneration>();
                  float3 offset = new float3(xAdd,0,yAdd);
                  //Logic des tiles
                  tile.mapOffset = offset;
                  tile.xSize = xTileSize;
                  tile.ySize = yTileSize;
                allChunk.Add(new Chunk(x,y,tile));
                tile.StartGeneration();
            }
        }
        baseChunkGenerated = true;    
    }

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
        Debug.Log("Update Chunk");
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
                    }
                    chunk.tileScript.isTileEnabled = false;
                    chunk.tileScript.tileRenderLogic();
                }
                if(asFindChunk == false){
                    adjacentChunkList.Add(generateChunk(x,y));;
                }
            } 
        }
        foreach (Chunk adjacentChunk in adjacentChunkList)
        {
            adjacentChunk.tileScript.isTileEnabled = true;
            adjacentChunk.tileScript.tileRenderLogic();
        }

    }

    private Chunk generateChunk(int x, int y)
    {
        int xAdd = x * (yTileSize*10);
        int yAdd = y * (yTileSize*10);
        GameObject terrain = Instantiate(tileGeneratorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        TileGeneration tile = terrain.GetComponent<TileGeneration>();
        float3 offset = new float3(xAdd,0,yAdd);
        //Logic des tiles
        tile.mapOffset = offset;
        tile.xSize = xTileSize;
        tile.ySize = yTileSize;
        Chunk generatedChunk = new Chunk(x,y,tile);
        allChunk.Add(generatedChunk);
        tile.StartGeneration();
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

}
