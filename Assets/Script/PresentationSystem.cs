using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

//System used for hybrid approach to animating characters in ECS based games
//It's managing the presentation of entity by using gameobject and synchronizing it with the entity
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial struct PresentationSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecbBOS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (presentation, entity) in SystemAPI.Query<PresentationGO>().WithEntityAccess())
        {
            GameObject go = GameObject.Instantiate(presentation.PlayerPrefab);
            ecbBOS.AddComponent(entity, new TransformGO() { Transform = go.transform });
            ecbBOS.AddComponent(entity, new AnimatorGO { Animator = go.GetComponent<Animator>() });
            ecbBOS.RemoveComponent<PresentationGO>(entity);
        }

        foreach (var (goTransform, goAnimator, transform, speed) in SystemAPI
                        .Query<TransformGO, AnimatorGO, RefRO<LocalTransform>, RefRO<Speed>>())
        {
            goTransform.Transform.position = transform.ValueRO.Position;
            goTransform.Transform.rotation = transform.ValueRO.Rotation;
            goAnimator.Animator.SetFloat("Speed", speed.ValueRO.actualValue);
        }
    }
}
