using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]

partial struct CoinPickupSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        var coinQuery = SystemAPI.QueryBuilder().WithAll<Coin, LocalTransform>().Build();
        if (coinQuery.IsEmpty)
            return;
        var coinEntities = coinQuery.ToEntityArray(state.WorldUpdateAllocator);
        var coinEntity = coinEntities[0];
        var coinTransform = SystemAPI.GetComponent<LocalTransform>(coinEntity);
            foreach (var (playerTransform, playerEntity) in SystemAPI.Query<RefRO<LocalTransform>>()
                         .WithAll<Player>().WithEntityAccess())
            {
                float distance = math.distance(playerTransform.ValueRO.Position,coinTransform.Position);
                if (distance < 1.5f)
                {
                    var prefabTransform = SystemAPI.GetComponent<LocalTransform>(entitiesReferences.CoinPrefabEntity);
                    entityCommandBuffer.SetComponent(coinEntity, new LocalTransform
                    {
                        Position = new float3(UnityEngine.Random.Range(-5f, 5f), 1.0f, UnityEngine.Random.Range(-5f, 5f)),
                        Rotation = prefabTransform.Rotation,
                        Scale = prefabTransform.Scale
                    });
                    Debug.Log($"Player {playerEntity.Index} collected the coin!");
                    break;
                }
        }
        
        entityCommandBuffer.Playback(state.EntityManager);

    }
    
}
