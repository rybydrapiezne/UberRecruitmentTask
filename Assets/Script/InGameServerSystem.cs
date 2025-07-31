using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

partial struct InGameServerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
        state.RequireForUpdate<NetworkId>();
    }
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        
        var coinQuery=state.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<Coin>());
        var playerQuery = state.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<Player>());
        
        int playerCount = playerQuery.CalculateEntityCount();
        int coinCount = coinQuery.CalculateEntityCount();
        
        foreach ((RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest, Entity entity) in SystemAPI
                     .Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<InGameRequestRpc>().WithEntityAccess())
        {
            entityCommandBuffer.AddComponent<NetworkStreamInGame>(receiveRpcCommandRequest.ValueRO.SourceConnection);
            Debug.Log("Client connected to Server");
            Entity playerPrefab = playerCount == 0 ? entitiesReferences.Player1PrefabEntity : entitiesReferences.Player2PrefabEntity;
            Entity playerEntity = entityCommandBuffer.Instantiate(playerPrefab);
            entityCommandBuffer.SetComponent(playerEntity, LocalTransform.FromPosition(new float3(
                UnityEngine.Random.Range(-5,+5),0,0
                )));
            NetworkId networkId = SystemAPI.GetComponent<NetworkId>(receiveRpcCommandRequest.ValueRO.SourceConnection);
            entityCommandBuffer.AddComponent(playerEntity,new GhostOwner
            {
                NetworkId = networkId.Value,
            });
            
            entityCommandBuffer.AppendToBuffer(receiveRpcCommandRequest.ValueRO.SourceConnection,new LinkedEntityGroup
            {
                Value = playerEntity,
            });
            if (playerCount == 0 && coinCount == 0)
            {
                Entity coinEntity = entityCommandBuffer.Instantiate(entitiesReferences.CoinPrefabEntity);
                var prefabTransform = SystemAPI.GetComponent<LocalTransform>(entitiesReferences.CoinPrefabEntity);
                entityCommandBuffer.SetComponent(coinEntity, new LocalTransform
                {
                    Position = new float3(UnityEngine.Random.Range(-5f, 5f), 1.0f, 0),
                    Rotation = prefabTransform.Rotation,
                    Scale = prefabTransform.Scale
                });
            }
            entityCommandBuffer.DestroyEntity(entity);

        }
        entityCommandBuffer.Playback(state.EntityManager);
    }


}

