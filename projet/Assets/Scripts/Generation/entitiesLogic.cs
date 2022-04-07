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
    
}
