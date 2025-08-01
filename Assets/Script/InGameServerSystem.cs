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

    public void OnUpdate(ref SystemState state)
    {

        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        var coinQuery = state.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<Coin>());
        var playerQuery = state.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<Player>());
        //calculating number of players for proper player prefab assignment 
        int playerCount = playerQuery.CalculateEntityCount();
        int coinCount = coinQuery.CalculateEntityCount();

        foreach ((RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest, Entity entity) in SystemAPI
                     .Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<InGameRequestRpc>().WithEntityAccess())
        {
            //Adding NetworkStreamInGame to clients to mark them as in game
            entityCommandBuffer.AddComponent<NetworkStreamInGame>(receiveRpcCommandRequest.ValueRO.SourceConnection);
            Debug.Log("Client connected to Server");

            //Correct prefab assignment based on number of players
            Entity playerPrefab = playerCount == 0 ? entitiesReferences.Player1PrefabEntity : entitiesReferences.Player2PrefabEntity;
            Entity playerEntity = entityCommandBuffer.Instantiate(playerPrefab);

            //Randomized spawn position of a player
            entityCommandBuffer.SetComponent(playerEntity, LocalTransform.FromPosition(new float3(
                UnityEngine.Random.Range(-5, +5), 0, 0
                )));
            NetworkId networkId = SystemAPI.GetComponent<NetworkId>(receiveRpcCommandRequest.ValueRO.SourceConnection);
            //Assigning ownership of the player entity
            entityCommandBuffer.AddComponent(playerEntity, new GhostOwner
            {
                NetworkId = networkId.Value,
            });

            //Linking player entity to client's connection entity
            entityCommandBuffer.AppendToBuffer(receiveRpcCommandRequest.ValueRO.SourceConnection, new LinkedEntityGroup
            {
                Value = playerEntity,
            });

            //Spawning the first coin
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
            //Destroying the RPC entity after execution
            entityCommandBuffer.DestroyEntity(entity);

        }
        //Applying all commands to EntityManager
        entityCommandBuffer.Playback(state.EntityManager);
    }


}

