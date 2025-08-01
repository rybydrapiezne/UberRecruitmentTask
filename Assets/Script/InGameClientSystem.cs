using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

//Ensuring that the system will run only on client simulation
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct InGameClientSystem : ISystem
{
    //Ensuring that EntitiesReferences and NetworkId exists before running OnUpdate
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
        state.RequireForUpdate<NetworkId>();
    }
    //Client side logic for entering to "InGame" mode
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach ((RefRO<NetworkId> networkId, Entity entity) in SystemAPI.Query<RefRO<NetworkId>>()
                     .WithNone<NetworkStreamInGame>().WithEntityAccess())
        {
            entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);
            Debug.Log("Client set as InGame");

            Entity rpcEntity = entityCommandBuffer.CreateEntity();
            entityCommandBuffer.AddComponent(rpcEntity, new InGameRequestRpc());
            entityCommandBuffer.AddComponent(rpcEntity, new SendRpcCommandRequest());
        }
        entityCommandBuffer.Playback(state.EntityManager);
    }

}

public struct InGameRequestRpc : IRpcCommand
{

}
