using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(MoveForwardSystem))]        //this system will run its update after the MoveForwardSystem
public class TimedDestroySystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem m_ecbSystem;

    protected override void OnCreate()
    {
        m_ecbSystem = World.GetExistingSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecbPackage = m_ecbSystem.CreateCommandBuffer();
        var ecb = ecbPackage.AsParallelWriter();

        float deltaTime = Time.DeltaTime;

        //schedules a job to delete all entities with the TimeToLive component after their timetolive value is 0
        //ref = read/write values, in = read only
        Entities.WithName("TimeToLive").ForEach((Entity entity, int entityInQueryIndex, ref TimeToLive timeToLive) =>
        {
            timeToLive.Value -= deltaTime;
            if (timeToLive.Value <= 0f) ecb.DestroyEntity(entityInQueryIndex, entity);
        }).ScheduleParallel();      //add this to schedule the job

        m_ecbSystem.AddJobHandleForProducer(Dependency);
    }
}
