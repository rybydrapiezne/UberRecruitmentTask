using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
[UnityEngine.Scripting.Preserve]
public class GameBootstrap : ClientServerBootstrap
{
    public override bool Initialize(string worldName)
    {
        AutoConnectPort = 7979;
        return base.Initialize(worldName);
    }
}
