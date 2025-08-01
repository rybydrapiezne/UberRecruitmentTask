using System;
using Unity.Entities;
using Unity.NetCode;

[GhostComponent]
public struct Speed : IComponentData
{
    [GhostField] public float value;
    [GhostField] public float actualValue;
}
