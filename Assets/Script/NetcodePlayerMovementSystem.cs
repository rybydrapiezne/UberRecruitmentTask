using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct NetcodePlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkStreamInGame>(); 
    }
    //[BurstCompile] Can't be burst compiled, not really sure why
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<NetcodePlayerInput> netcodePlayerInput, RefRW<LocalTransform> localTransform, RefRW<Speed> speed, Entity entity)
                 in SystemAPI.Query<RefRW<NetcodePlayerInput>, RefRW<LocalTransform>, RefRW<Speed>>()
                     .WithAll<Simulate, GhostOwnerIsLocal>().WithEntityAccess())
        {
            float moveSpeed = speed.ValueRO.value > 0 ? speed.ValueRO.value : 1;
            float3 movementVector = new float3(netcodePlayerInput.ValueRO.InputVector.x, 0, netcodePlayerInput.ValueRO.InputVector.y);

            localTransform.ValueRW.Position += movementVector * moveSpeed * SystemAPI.Time.DeltaTime;

            float targetSpeed = math.length(movementVector) * moveSpeed;
            speed.ValueRW.actualValue =targetSpeed;

            if (math.lengthsq(movementVector) > 0.01f)
            {
                quaternion targetRotation = quaternion.LookRotation(movementVector, math.up());
                localTransform.ValueRW.Rotation = targetRotation;
            }

        }
    }
}

