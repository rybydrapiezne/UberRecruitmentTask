using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

public class CoinAuthoring : MonoBehaviour
{
    public GameObject coinPrefab;
    public class Baker : Baker<CoinAuthoring>
    {
        public override void Bake(CoinAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Coin());
        }
    }
}

public struct Coin : IComponentData
{
    
}