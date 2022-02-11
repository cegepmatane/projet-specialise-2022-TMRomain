using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
    [SerializeField]
    int Xsize = 25;
    [SerializeField]
    int Ysize= 25;
    [SerializeField]
    int seed = 1234;

    [SerializeField]
    float scale = 1.0f;
    
    public float[,] myPerlinNoise;

    void GenerateNoise(){
        int y =0;
        myPerlinNoise = new float[Xsize,Ysize];
        while (y < Ysize)
        {
            int x =0;
            while (x < Xsize)
            {
                myPerlinNoise[x,y] = Mathf.PerlinNoise(x, y);
                x++;
            }

            y++;
        }
    }

}
