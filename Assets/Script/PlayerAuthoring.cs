using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Unity.Transforms;
public class PlayerAuthoring : MonoBehaviour
{
    public float Speed;
    public GameObject playerPrefab;
    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Player());
            PresentationGO presentation = new PresentationGO();
            presentation.PlayerPrefab = authoring.playerPrefab;
            AddComponentObject(entity,presentation);
            if (authoring.Speed > 0)
            {
                Speed speed = default;
                speed.value = authoring.Speed;
                speed.actualValue = 0;
                AddComponent(speed);
            }
        }
    }
}

public class PresentationGO : IComponentData
{
    public GameObject PlayerPrefab;
}

public class TransformGO : ICleanupComponentData
{
    public Transform Transform;
}

public class AnimatorGO : IComponentData
{
    public Animator Animator;
}
public struct Player : IComponentData
{
    
}


