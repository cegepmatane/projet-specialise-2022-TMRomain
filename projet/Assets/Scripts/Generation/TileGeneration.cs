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
    Material planeMaterial;
    [SerializeField]
    Material planeSecondMaterial;
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
                CreateEntitie(position, rotation);
                entityCount++;
            }
        }
    }
    void CreateEntitie(float3 position,quaternion rotation){
        Entity entity = entityManager.CreateEntity();
        int couleurRandom= UnityEngine.Random.Range(0, 2);

        Material randomMat = couleurRandom == 0 ? planeMaterial : planeSecondMaterial;

        //Donne un nom a l'entiter generer
        entityManager.SetName(entity,"Entiter Spawn " + entityCount);

        //Change la position de l'entité
        entityManager.AddComponentData(entity,new Translation{Value = position});

        //Change la rotation de l'objet
        entityManager.AddComponentData(entity,new Rotation{Value= rotation});

        //Ajout du visuel a l'entité
        entityManager.AddSharedComponentData(entity,new RenderMesh{
            mesh = planeMesh,
            material = randomMat
        });
        entityManager.AddComponentData(entity, new RenderBounds { Value = planeMesh.bounds.ToAABB() });

        //Rend l'objet relatif a la scene.
        entityManager.AddComponentData(entity,new LocalToWorld{});
    }

}
