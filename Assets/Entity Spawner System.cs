using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EntitySpawnerSystem : ComponentSystem
{
    private float spawnTimer;
    private Random random;

    protected override void OnCreate()
    {
        random = new Random(56);
    }

    protected override void OnUpdate()
    {
        spawnTimer -= Time.DeltaTime;

        if (spawnTimer <= 0f)
        {
            spawnTimer = .01f;

            //spawn all entities at a random point every .5 seconds
            Entities.ForEach((ref PrefabEntityComponent prefabEntityComponent) =>
            {
                //instantiates prefabEntity with entity manager
                Entity spawnedEntity = EntityManager.Instantiate(prefabEntityComponent.prefabEntity);
                //
                EntityManager.SetComponentData(spawnedEntity, new Translation { Value = new float3(random.NextFloat(-10f, 10f), 1, random.NextFloat(-10f, 10f))});
            });
        }
    }
}
