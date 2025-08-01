using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Mathematics;
using UnityEngine;

//System for handling player input, running in the GhostInputSystemGroup
[UpdateInGroup(typeof(GhostInputSystemGroup))]
partial struct NetcodePlayerInputSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkStreamInGame>();
        state.RequireForUpdate<NetcodePlayerInput>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<NetcodePlayerInput> netcodePlayerInput in SystemAPI.Query<RefRW<NetcodePlayerInput>>()
                     .WithAll<GhostOwnerIsLocal>())
        {
            //Basic player movement logic setting up the InputVector with direction
            netcodePlayerInput.ValueRW.InputVector = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}
