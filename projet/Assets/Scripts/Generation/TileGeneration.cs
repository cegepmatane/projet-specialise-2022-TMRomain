using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
struct SharedGrouping : ISharedComponentData
{
    public int Group;
}
public class TileGeneration : MonoBehaviour
{
    [SerializeField]
    PerlinNoiseGenerator noiseGenerator;
    //Variable temporaraire pour la generation des arbre
    [SerializeField]
    bool faireSpawnArbre = true;
    [SerializeField]
    public int xSize;
    [SerializeField]
    public int ySize;
    [SerializeField]
    float spacing;
    [SerializeField]
    Mesh planeMesh;
    [SerializeField]
    GameObject treePrefab;
    Mesh treeMesh;
    Material treeMaterial;

    [SerializeField]
    Material[] planeMaterial;
    [SerializeField]
    int seed;
    [SerializeField]
    public float3 mapOffset;
    EntityManager entityManager;
    int entityCount;
    public int chunkID;
    public bool isTileEnabled = false;
    private List<Entity> allTileEntities = new List<Entity>();
    float[,] perlinNoseGeneration ;
    float[,] perlinNoseGenerationForTree;
    bool isBasePlaneCreated = false;
    bool isBaseTreeCreated = false;
    Entity baseTreeEntity = new Entity();
    Entity basePlaneEntity = new Entity();
    // Start is called before the first frame update
    // void Start(){

    //     StartGeneration(0);
    // }
    public void StartGeneration(int chunkGroup)
    {   
        chunkID=chunkGroup;
        //Recupere les valeur pour la generation des entiter arbre 
        Transform arbreTransform = treePrefab.GetComponentsInChildren<Transform>()[1];
        treeMesh = arbreTransform.GetComponent<MeshFilter>().sharedMesh;
        treeMaterial = arbreTransform.GetComponent<Renderer>().sharedMaterial;
        //Seed Generer aleatoirement 
        seed = UnityEngine.Random.Range(0, 300);
        noiseGenerator = gameObject.AddComponent(typeof(PerlinNoiseGenerator)) as PerlinNoiseGenerator;
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
        // entityManager.AddComponentData(entityTile,new WorldToLocal{});
        // entityManager.AddComponentData(entityTile,new RenderBounds{Value = new Bounds(Vector3.zero, Vector3.one * 2000).ToAABB()});

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
                if(!isBasePlaneCreated){
                    CreateEntitie(position + mapOffset, rotation,randomMat,genererArbre);
                }else{
                    cloneEntity(position+ mapOffset,randomMat,rotation,genererArbre);
                }
                entityCount++;
            }
        }
        // tileRenderLogic();
    }
    void CreateEntitie(float3 position,quaternion rotation, Material planeColor,bool genererArbre){
        
        //Generation des arbres
        if(genererArbre)
        {
            GenererArbre(position, rotation);
            //Ancien Systeme
            // //Generation de l'arbre 
            // GameObject arbre = Instantiate(treePrefab, new Vector3(position.x, position.y, position.z), Quaternion.identity);
            // //Application de la taille aleatoire
            // int randomScale = UnityEngine.Random.Range(15, 30);
            // Vector3 randomSize = new Vector3 (randomScale,randomScale,randomScale);
            // arbre.transform.localScale = randomSize;
        }
        Entity entity = entityManager.CreateEntity();
        //Donne un nom a l'entiter generer
        entityManager.SetName(entity,"Case " + chunkID);

        //Change la position de l'entité
        entityManager.AddComponentData(entity,new Translation{Value = position});

        //Change la rotation de l'objet
        entityManager.AddComponentData(entity,new Rotation{Value= rotation});

        entityManager.AddSharedComponentData(entity,new SharedGrouping{Group = chunkID});

        //Ajout du visuel a l'entité
        entityManager.AddSharedComponentData(entity,new RenderMesh{
            mesh = planeMesh,
            material = planeColor
        });
        entityManager.AddComponentData(entity, new RenderBounds { Value = planeMesh.bounds.ToAABB() });

        //Rend l'objet relatif a la scene.
        entityManager.AddComponentData(entity,new LocalToWorld{});
        basePlaneEntity = entity;
        isBasePlaneCreated = true;
        // entityManager.AddComponentData(entity, new Unity.Transforms.Parent { Value = entityTile });
    }

    private void GenererArbre(float3 position, quaternion rotation)
    {
        Transform arbreTransform = treePrefab.GetComponentsInChildren<Transform>()[0];
        Entity entityArbre = entityManager.CreateEntity();
        // entityManager.AddComponentData(entityArbre, new Parent { Value = entityTile });
        entityManager.SetName(entityArbre, "Arbre " + chunkID);
        entityManager.AddComponentData(entityArbre, new Translation { Value = position });
        entityManager.AddComponentData(entityArbre, new Rotation { Value = rotation });
        entityManager.AddSharedComponentData(entityArbre, new RenderMesh
        {
            mesh = treeMesh,
            material = treeMaterial
        });
        entityManager.AddComponentData(entityArbre, new RenderBounds { Value = treeMesh.bounds.ToAABB() });
        // entityManager.AddComponentData(entityArbre, new WorldRenderBounds { Value = treeMesh.bounds.ToAABB() });


        int randomScale = UnityEngine.Random.Range(15, 30);
        float3 randomSize = new float3(randomScale, randomScale, randomScale);
        entityManager.AddSharedComponentData(entityArbre, new SharedGrouping { Group = chunkID });

        entityManager.AddComponentData(entityArbre, new NonUniformScale { Value = randomSize });

        //Rend l'objet relatif a la scene.
        entityManager.AddComponentData(entityArbre, new LocalToWorld { });
        baseTreeEntity= entityArbre;
        isBaseTreeCreated = true;
    }

    void cloneEntity(float3 position,Material planeColor,quaternion rotation,bool genererArbre){
        Entity clonePlane = entityManager.Instantiate(basePlaneEntity);
        entityManager.SetComponentData<Translation>(clonePlane,new Translation{Value = position});
        entityManager.SetSharedComponentData<RenderMesh>(clonePlane,new RenderMesh{
            mesh = planeMesh,
            material = planeColor});
        if(genererArbre){
            if(isBaseTreeCreated){
                Entity cloneTree = entityManager.Instantiate(baseTreeEntity);
                entityManager.SetComponentData<Translation>(cloneTree,new Translation{Value = position});
            }else{
            GenererArbre(position,rotation);
            }

        }

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

    public void DisableTile(){
        // entityManager.SetEnabled(entityTile,false);
        var m_Query = entityManager.CreateEntityQuery(typeof(SharedGrouping));
        m_Query.SetSharedComponentFilter(new SharedGrouping { Group = chunkID });
        entityManager.AddComponent<Disabled>(m_Query);
        Debug.Log("Disabling Tile : " + chunkID);

    }
    public void EnableTile(){
        Debug.Log("Enabling Tile : " + chunkID);
        var m_Query = entityManager.CreateEntityQuery(typeof(Disabled),typeof(SharedGrouping));
        m_Query.SetSharedComponentFilter(new SharedGrouping { Group = chunkID });
        entityManager.RemoveComponent<Disabled>(m_Query);
        // entityManager.SetEnabled(entityTile,true);
    }
    // public void tileRenderLogic(){
    //     var m_Query = entityManager.CreateEntityQuery(typeof(SharedGrouping));
    //     m_Query.SetSharedComponentFilter(new SharedGrouping { Group = chunkID });
    //     if(!isTileEnabled){
    //         NativeArray<Entity> entities = m_Query.ToEntityArray(Allocator.TempJob) ;
    //         Entity[] realEntities = entities.ToArray();
    //         entities.Dispose();
    //         entitiesLogic.instance.disableEntityInChunk(realEntities,entityManager);
    //     }else{
    //         NativeArray<Entity> entitiesEnable = m_Query.ToEntityArray(Allocator.TempJob) ;
    //         Entity[] realEntitiesEnabled = entitiesEnable.ToArray();
    //         entitiesEnable.Dispose();
    //         entitiesLogic.instance.enableEntityInChunk(realEntitiesEnabled,entityManager);
    //     }


        // var m_Query = entityManager.CreateEntityQuery(typeof(SharedGrouping));
        // m_Query.SetSharedComponentFilter(new SharedGrouping { Group = chunkID });
        // NativeArray<Entity> entities = m_Query.ToEntityArray(Allocator.TempJob) ;
        // Entity[] realEntities = entities.ToArray();
        // entities.Dispose();

        // if(!isTileEnabled){
        //     // Debug.Log(realEntities.Length);
        //     manager.disableEntityInChunk(realEntities,entityManager);
        // }else{
        //     manager.enableEntityInChunk(realEntities,entityManager);
        // }
        // entities.Dispose();
        //  manager.changeEntityValue(m_Query,entityManager,isTileEnabled);
        //  foreach (Entity entity in allTileEntities)
        //     {
        //         // Disabled disable = entityManager.GetComponentData<Disabled>(entity);
        //         // entityManager.AddComponentData(entity,new Disabled{});
        //         //LinkedEntityGroup a essayer
        //         entityManager.SetEnabled(entity, isTileEnabled);
        //     }
        // if(!isTileEnabled){
        //     foreach (Entity entity in allTileEntities)
        //     {
        //         // Disabled disable = entityManager.GetComponentData<Disabled>(entity);
        //         // entityManager.AddComponentData(entity,new Disabled{});
        //     }
        // }else{
        //     foreach (Entity entity in allTileEntities)
        //     {
        //         // entityManager.RemoveComponent(entity,typeof(Disabled));
        //     }
        // }
}
