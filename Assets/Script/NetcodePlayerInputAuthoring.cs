using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Unity.Mathematics;

//Authoring component for setting up the player input
public class NetcodePlayerInputAuthoring : MonoBehaviour
{
    public class Baker : Baker<NetcodePlayerInputAuthoring>
    {
        public override void Bake(NetcodePlayerInputAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new NetcodePlayerInput());
        }
    }
}

public struct NetcodePlayerInput : IInputComponentData
{
    //Storing the direction of player input
    public float2 InputVector;
}
