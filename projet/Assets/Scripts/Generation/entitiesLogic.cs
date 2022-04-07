using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

public partial class entitiesLogic : MonoBehaviour
{
    public static entitiesLogic instance = null;
    EntityManager entityManager;
    private void Awake() {
        if(instance == null){
            instance=this;
        }else if(instance != this){
            Destroy(gameObject);
        }
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    }
    public void disableEntityInChunk(Entity[] entities,EntityManager manager){
        foreach (Entity entity in entities)
        {
            manager.SetEnabled(entity, false);
        }
    }
    public void enableEntityInChunk(Entity[]  entities,EntityManager manager){

        foreach (Entity entity in entities)
        {
            manager.SetEnabled(entity, true);
        }
    }
}
