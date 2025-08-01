using Unity.Entities;
using UnityEngine;

//Monobehaviour class servig as authoring component for creating coin entities
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
//Tag for identifying Coin entities without storing data
public struct Coin : IComponentData
{

}