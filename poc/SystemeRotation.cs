using UnityEngine;
using Unity.Entities;

public class RotationSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<FonctionementRotation, Transform>().ForEach((Entity entity, FonctionementRotation rotationBehaviour, Transform transform) =>
        {
            transform.Rotate(0f, rotationBehaviour.vitesse * Time.DeltaTime, 0f);
        });
    }
}
