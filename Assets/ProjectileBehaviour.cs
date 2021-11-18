using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour, IConvertGameObjectToEntity
{
    [Header ("Movement")]
    public float speed = 50f;

    [Header ("Life Time")]
    public float lifeTime = 2f;

    //Rigidbody rb;

    private void Start() {
        //rb = GetComponent<Rigidbody>();     //gets rigidbody component at the start
       // Destroy(gameObject, lifeTime);      //destroys gameobject after 2 seconds
    }

    private void Update() {
        //basic movement with rigidbody
        //Vector3 movement = transform.forward * speed * Time.deltaTime;
        //rb.MovePosition(transform.position + movement);
    }

    //converts this gameobject into an entity (IConvertGameObjectToEntity)
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        //adds move forward component
        dstManager.AddComponent(entity, typeof(MoveForwardComponent));

        //adds a movespeed value to the entity using this script's speed value
        MoveSpeed moveSpeed = new MoveSpeed { Value = speed };
        dstManager.AddComponentData(entity, moveSpeed);

        //adds a TimeToLive value to the entity using this lifetime
        TimeToLive ttl = new TimeToLive { Value = lifeTime };
        dstManager.AddComponentData(entity, ttl);
    }
}
