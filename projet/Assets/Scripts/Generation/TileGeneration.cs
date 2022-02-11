using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class TileGeneration : MonoBehaviour
{
    [SerializeField]
    PerlinNoiseGenerator noiseGenerator;
    [SerializeField]
    int xSize;
    [SerializeField]
    int ySize;
    [SerializeField]
    float spacing;
    [SerializeField]
    Mesh planeMesh;
    [SerializeField]
    Material[] planeMaterial;
    EntityManager entityManager;
    int entityCount;

    float[,] perlinNoseGeneration ;
    // Start is called before the first frame update
    void Start()
    {
        noiseGenerator = new PerlinNoiseGenerator();
        noiseGenerator.pixWidth = xSize;
        noiseGenerator.pixHeight = ySize;
        perlinNoseGeneration = noiseGenerator.CalcNoise();
        entityCount = 0;
        GenerateTerrain();
    }

    void GenerateTerrain(){
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        for (int x = 0; x < xSize;x++)
        {
            for (int y = 0; y < ySize; y++)
            {
               float3 position = new float3(x*spacing,0f,y*spacing);

               quaternion rotation = quaternion.Euler(
                   0f*Mathf.Deg2Rad,
                    0f*Mathf.Deg2Rad,
                   0f* Mathf.Deg2Rad
               ); 
                Material randomMat = planeMaterial[RandomColor(x,y)];

                CreateEntitie(position, rotation,randomMat);
                entityCount++;
            }
        }
    }
    void CreateEntitie(float3 position,quaternion rotation, Material planeColor){
        Entity entity = entityManager.CreateEntity();


        //Donne un nom a l'entiter generer
        entityManager.SetName(entity,"Entiter Spawn " + entityCount);

        //Change la position de l'entité
        entityManager.AddComponentData(entity,new Translation{Value = position});

        //Change la rotation de l'objet
        entityManager.AddComponentData(entity,new Rotation{Value= rotation});

        //Ajout du visuel a l'entité
        entityManager.AddSharedComponentData(entity,new RenderMesh{
            mesh = planeMesh,
            material = planeColor
        });
        entityManager.AddComponentData(entity, new RenderBounds { Value = planeMesh.bounds.ToAABB() });

        //Rend l'objet relatif a la scene.
        entityManager.AddComponentData(entity,new LocalToWorld{});
    }
    int RandomColor(int  x, int y){
        float positionNoise = perlinNoseGeneration[x,y];
        Debug.Log(positionNoise);
        if(positionNoise< 0.3){
            return 1;
        }else if(positionNoise< 0.34){
            return 2;
        }else{
            return 0;
        }
    }
}
