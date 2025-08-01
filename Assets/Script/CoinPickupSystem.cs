using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

//System for handling coin piuckups by players

//Runs only on server simulation
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]

partial struct CoinPickupSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
    }

    //Marked for Burst compilation to improve performance
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //Creating temporary entity command buffer for entity modification
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

        //Building a custom query for coin entities, could be made using foreach like for the player
        var coinQuery = SystemAPI.QueryBuilder().WithAll<Coin, LocalTransform>().Build();
        if (coinQuery.IsEmpty)
            return;
        var coinEntities = coinQuery.ToEntityArray(state.WorldUpdateAllocator);
        //geting only first coin entity in the array
        var coinEntity = coinEntities[0];
        //geting transform of the coin entity
        var coinTransform = SystemAPI.GetComponent<LocalTransform>(coinEntity);

        //foreach for finding players
        foreach (var (playerTransform, playerEntity) in SystemAPI.Query<RefRO<LocalTransform>>()
                     .WithAll<Player>().WithEntityAccess())
        {
            //Calculating distance between player and coin
            float distance = math.distance(playerTransform.ValueRO.Position, coinTransform.Position);
            if (distance < 1.5f)
            {
                //When player is in desired distance the coin is moved to new location
                var prefabTransform = SystemAPI.GetComponent<LocalTransform>(entitiesReferences.CoinPrefabEntity);
                entityCommandBuffer.SetComponent(coinEntity, new LocalTransform
                {
                    //Randomizing new postion for the coin
                    Position = new float3(UnityEngine.Random.Range(-5f, 5f), 1.0f, UnityEngine.Random.Range(-5f, 5f)),
                    Rotation = prefabTransform.Rotation,
                    Scale = prefabTransform.Scale
                });
                break;
            }
        }
        //Applying all buffered commands to the EntityManager
        entityCommandBuffer.Playback(state.EntityManager);

    }

}
