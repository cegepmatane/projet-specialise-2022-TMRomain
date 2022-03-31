using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
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

    [SerializeField]
    Transform playerTransform;
    private List<Chunk> allChunk = new List<Chunk>();

    public void Start(){
        GenerateBaseChunk();
    }
    private void GenerateBaseChunk(){
        for (int x = 0; x < mapSize; x++)
        {
            int xAdd = x * (xTileSize*10);
            for (int y = 0; y < mapSize; y++)
            {
                int yAdd = y * (yTileSize*10);
                  GameObject terrain = Instantiate(tileGeneratorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                  TileGeneration tile = terrain.GetComponent<TileGeneration>();
                  float3 offset = new float3(xAdd,0,yAdd);
                  //Logic des tiles
                  tile.mapOffset = offset;
                  tile.xSize = xTileSize;
                  tile.ySize = yTileSize;
                    if(x == 1 && y == 1){
                        tile.isTileEnabled= true;
                    }
                allChunk.Add(new Chunk(xAdd,yAdd,tile));
                tile.StartGeneration();
                  
            }
        }
    }


    

}
