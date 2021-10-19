using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

//this system updates all entities in the scene with the MoveForwardComponent
public class MoveForwardSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        //schedules a job to move forward over delta time
        //ref = read/write values, in = read only
        Entities.WithName("MoveForwardComponent").ForEach((ref Translation pos, ref Rotation rot, in MoveSpeed speed) =>
        {
            pos.Value = pos.Value + (deltaTime * speed.Value * math.forward(rot.Value));
        }).ScheduleParallel();      //add this to schedule the job
    }
}