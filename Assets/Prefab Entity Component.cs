using Unity.Entities;

//this script will convert prefabs into entities

[GenerateAuthoringComponent]
public struct PrefabEntityComponent : IComponentData
{
    public Entity prefabEntity;
}
