using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
public class ChunkManagerScript : MonoBehaviour
{
    [SerializeField]
    GameObject tileGeneratorPrefab;
    [SerializeField]
    int xTileSize;
    [SerializeField]
    int yTileSize;

    [SerializeField]
    int mapSize =3;


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
                  float3 offset = new float3(xAdd,yAdd,0);
                  tile.xSize = xTileSize;
                  tile.ySize = yTileSize;
                  tile.StartGeneration();
            }
        }
    }

}
