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
    int xSize;
    [SerializeField]
    int ySize;
    [SerializeField]
    float spacing;
    [SerializeField]
    Mesh planeMesh;
    [SerializeField]
    Material planeMaterial;

    EntityManager entityManager;
    int entityCount;
    // Start is called before the first frame update
    void Start()
    {
        entityCount = 0;
        GenerateTerrain();
    }

    void GenerateTerrain(){
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        for (int x = 0; x < xSize;x++)
        {
            for (int y = 0; y < ySize; y++)
            {
               float3 position = new float3(0f,x*spacing, y*spacing);

               quaternion rotation = quaternion.Euler(
                   0f*Mathf.Deg2Rad,
                   5f * y*Mathf.Deg2Rad,
                   5f* x * Mathf.Deg2Rad
               ); 
                CreateEntitie(position, rotation);
                entityCount++;
            }
        }
    }
    void CreateEntitie(float3 position,quaternion rotation){
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
            material = planeMaterial
        });
        //Rend l'objet relatif a la scene.
        entityManager.AddComponentData(entity,new LocalToWorld{});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
