using UnityEngine;
using System.Collections;


public class PerlinNoiseGenerator : MonoBehaviour
{
    // Hauteur et largeur de la generation Perlin
    public int pixWidth = 100;
    public int pixHeight = 100;
    public float scale = 10F;
    //Array qui contien les valeur du Noise Generer
    private float[,] perlinArray;

    void Start()
    {
        // CalcNoise();
    }
    //Genere le Noise   
    public float[,] CalcNoise(int seed)
    {
        //Initialisation de l'array 
        perlinArray = new float[pixWidth,pixHeight];

        float y = 0.0F;

        while (y < pixHeight)
        {
            float x = 0.0F;
            while (x < pixWidth)
            {
                //Generer en fonction de la taille maximum et de la ou nous somme dans l'array 
                float xCoord =  seed  + x / pixWidth * scale;
                float yCoord =  seed  + y / pixHeight * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                perlinArray[(int)x,(int)y] = sample;
                // Debug.Log(perlinArray[(int)x,(int)y]);
                x++;
            }
            y++;
        }
        return perlinArray;
    }

}