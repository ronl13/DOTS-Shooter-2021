using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Enable ECS")]
    public bool UseECS = false;
    public bool spreadShot = false;

    [Header("Shooting Parameters")]
    public float fireRate = .1f;
    public int spreadAmount = 20;

    [Header("REQUIRED Components")]
    public Transform shootPoint;
    public GameObject bulletPrefab;

    [Header("Current Bullets")]
    public int currentBullets;

    float timer;

    EntityManager manager;
    Entity bulletEntity;
    BlobAssetStore blobAssetStore;

    private void Awake() {
        //World.DefaultGameObjectInjectionWorld = the current active world
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);
        //bullet prefab will be converted into bullet entity
    }
    
    void Start()
    {
        
        //bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, 
        //GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore()));
        
        //the gameobject conversion settings set the destination world to the current world
        timer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.T)) spreadShot = spreadShot == true ? false : true;

        if (Input.GetKey(KeyCode.Mouse0) && timer >= fireRate)      //when player can shoot and presses shoot button
        {
            Vector3 bulletRotation = shootPoint.rotation.eulerAngles;
            bulletRotation.x = 0f;

            if (spreadShot)
            {
                if (UseECS) SpawnBulletSpreadECS(bulletRotation);
                else SpawnBulletSpread(bulletRotation);
            }
            else
            {
                if (UseECS) SpawnBulletECS(bulletRotation);
                else SpawnBullet(bulletRotation);
            }

            timer = 0f;
        }
    }

    void SpawnBullet(Vector3 rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;

        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = Quaternion.Euler(rotation);

        //bullet counter stuff
        currentBullets += 1;
        Invoke("RemoveBulletFromCounter", 2f);
    }

    void SpawnBulletSpread(Vector3 rotation)
    {
        int max = spreadAmount / 2;
        int min = -max;

        Vector3 tempRot = rotation;

        for (int i = min; i < max; i++)
        {
            tempRot.x = (rotation.x + 3 * i) % 360;

            for (int y = min; y < max; y++)
            {
                tempRot.y = (rotation.y + 3 * y) % 360;

                GameObject bullet = Instantiate(bulletPrefab) as GameObject;

                bullet.transform.position = shootPoint.position;
                bullet.transform.rotation = Quaternion.Euler(tempRot);

                //bullet counter stuff
                currentBullets += 1;
                Invoke("RemoveBulletFromCounter", 2f);
            }
        }
    }

    //ECS-----------------------------------------------------------------------------------------------------

    void SpawnBulletECS(Vector3 rotation)
    {
        Entity bullet = manager.Instantiate(bulletEntity);

        manager.SetComponentData(bullet, new Translation { Value = shootPoint.position });
        manager.SetComponentData(bullet, new Rotation { Value = Quaternion.Euler(rotation) });

        //bullet counter stuff
        currentBullets += 1;
        Invoke("RemoveBulletFromCounter", 2f);
    }

    void SpawnBulletSpreadECS(Vector3 rotation)
    {
        int max = spreadAmount / 2;
        int min = -max;
        int totalAmount = spreadAmount * spreadAmount;

        Vector3 tempRot = rotation;

        int index = 0; //keeps track of the native array's index

        //native array holds the entities i'm looking to create
        NativeArray<Entity> bullets = new NativeArray<Entity>(totalAmount, Allocator.TempJob);  //array length = totalAmount, allocator type = temp job
        //instantiates the needed bullet entities and stores them in the array
        manager.Instantiate(bulletEntity, bullets);

        for (int x = min; x < max; x++)
        {
            tempRot.x = (rotation.x + 3 * x) % 360;

            for (int y = min; y < max; y++)
            {
                tempRot.y = (rotation.y + 3 * y) % 360;

                Entity bullet = manager.Instantiate(bulletEntity);

                manager.SetComponentData(bullets[index], new Translation { Value = shootPoint.position });
                manager.SetComponentData(bullets[index], new Rotation { Value = Quaternion.Euler(tempRot) });

                index++;    //updates index

                //bullet counter stuff
                currentBullets += 1;
                Invoke("RemoveBulletFromCounter", 2f);
            }
        }
        bullets.Dispose(); //native arrays don't know when they're done being used, this will tell the array to clean up to avoid memory leaks
    }

    void RemoveBulletFromCounter()
    {
        currentBullets -= 1;
    }
}
