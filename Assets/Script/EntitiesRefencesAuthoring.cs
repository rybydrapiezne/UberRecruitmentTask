using Unity.Entities;
using UnityEngine;

public class EntitiesReferencesAuthoring : MonoBehaviour
{
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameObject coinPrefab;
    public class Baker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences
            {
                Player1PrefabEntity = GetEntity(authoring.player1Prefab, TransformUsageFlags.Dynamic),
                Player2PrefabEntity = GetEntity(authoring.player2Prefab, TransformUsageFlags.Dynamic),
                CoinPrefabEntity = GetEntity(authoring.coinPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}

public struct EntitiesReferences : IComponentData
{
    public Entity Player1PrefabEntity;
    public Entity Player2PrefabEntity;
    public Entity CoinPrefabEntity;
}
