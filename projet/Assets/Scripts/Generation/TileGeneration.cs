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
    //Variable temporaraire pour la generation des arbre
    [SerializeField]
    bool faireSpawnArbre = true;
    [SerializeField]
    int xSize;
    [SerializeField]
    int ySize;
    [SerializeField]
    float spacing;
    [SerializeField]
    Mesh planeMesh;
    [SerializeField]
    GameObject treePrefab;
    [SerializeField]
    Material[] planeMaterial;
    [SerializeField]
    int seed;
    [SerializeField]
    float3 mapOffset;
    EntityManager entityManager;
    int entityCount;

    float[,] perlinNoseGeneration ;
    float[,] perlinNoseGenerationForTree;
    // Start is called before the first frame update
    void Start()
    {   //Seed Generer aleatoirement 
        seed = UnityEngine.Random.Range(0, 300);
        noiseGenerator = new PerlinNoiseGenerator();
        noiseGenerator.pixWidth = xSize;
        noiseGenerator.pixHeight = ySize;
        perlinNoseGeneration = noiseGenerator.CalcNoise(seed);
        if(faireSpawnArbre){
            perlinNoseGenerationForTree = noiseGenerator.CalcNoise(seed*15);
        }
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
               int matType = RandomColor(x,y);
                Material randomMat = planeMaterial[matType];
                bool genererArbre = false;
                if(faireSpawnArbre){
                    genererArbre = spawnArbre(matType,x,y);
                }

                CreateEntitie(position + mapOffset, rotation,randomMat,genererArbre);
                entityCount++;
            }
        }
    }
    void CreateEntitie(float3 position,quaternion rotation, Material planeColor,bool genererArbre){
        Entity entity = entityManager.CreateEntity();

        if(genererArbre){
            //Generation de l'arbre 
            GameObject arbre = Instantiate(treePrefab, new Vector3(position.x, position.y, position.z), Quaternion.identity);
            //Application de la taille aleatoire
            int randomScale = UnityEngine.Random.Range(15, 30);
            Vector3 randomSize = new Vector3 (randomScale,randomScale,randomScale);
            arbre.transform.localScale = randomSize;
        }
        //Donne un nom a l'entiter generer
        entityManager.SetName(entity,"Case " + entityCount);

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
        if(positionNoise< 0.3){
            return 1;
        }else if(positionNoise< 0.34){
            return 2;
        }else{
            return 0;
        }
    }
    bool spawnArbre(int type,int  x, int y){
        float positionNoise = perlinNoseGenerationForTree[x,y];
        if(positionNoise< 0.36){
            if(type == 0){
                return true;
            }else{
                return false;
            }
        }else{
            return false;
        }
    }
}
