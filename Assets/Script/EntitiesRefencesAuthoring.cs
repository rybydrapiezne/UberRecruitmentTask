using Unity.Entities;
using UnityEngine;

//Authoring component that holds references to prefabs that are going to be spawned
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
            //Assigning entity prefab references to fields of the entity
            AddComponent(entity, new EntitiesReferences
            {
                Player1PrefabEntity = GetEntity(authoring.player1Prefab, TransformUsageFlags.Dynamic),
                Player2PrefabEntity = GetEntity(authoring.player2Prefab, TransformUsageFlags.Dynamic),
                CoinPrefabEntity = GetEntity(authoring.coinPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}
//Used for storing references to prefab entities
public struct EntitiesReferences : IComponentData
{
    public Entity Player1PrefabEntity;
    public Entity Player2PrefabEntity;
    public Entity CoinPrefabEntity;
}
