using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour {

    public static ObjectPoolManager instance;

    public GameObject standardHorizonalBulletPrefab, sphereBulletPrefab, sparkPrefab;

    public int numStandardHorizonalBulletsToSpawn, numSphereBulletsToSpawn, numSparksToSpawn;

    public static List<GameObject> standardHorizontalBulletObjectPool, sphereBulletObjectPool, sparkObjectPool;

    private GameObject pooledObjectFolder;

    // Use this for initialization
    private void Start()
    {
        instance = this;
        pooledObjectFolder = GameObject.Find("InitialPooledObjects");

        // Standard horizontal bullet object pool
        standardHorizontalBulletObjectPool = new List<GameObject>();

        for (var i = 1; i <= numStandardHorizonalBulletsToSpawn; i++)
        {
            var stdHorizontalBullet = (GameObject) Instantiate(standardHorizonalBulletPrefab);

            SetParentTransform(stdHorizontalBullet);

            stdHorizontalBullet.SetActive(false);
            standardHorizontalBulletObjectPool.Add(stdHorizontalBullet);
        }

        // Sphere bullet object pool
        sphereBulletObjectPool = new List<GameObject>();

        for (var i = 1; i <= numSphereBulletsToSpawn; i++)
        {
            var sphereBullet = (GameObject)Instantiate(sphereBulletPrefab);

            SetParentTransform(sphereBullet);

            sphereBullet.SetActive(false);
            sphereBulletObjectPool.Add(sphereBullet);
        }

        // Spark object pool
        sparkObjectPool = new List<GameObject>();

        for (var i = 1; i <= numSparksToSpawn; i++)
        {
            var spark = (GameObject)Instantiate(sparkPrefab);

            SetParentTransform(spark);

            spark.SetActive(false);
            sparkObjectPool.Add(spark);
        }
    }

    private void SetParentTransform(GameObject gameObjectRef)
    {
        if (pooledObjectFolder != null)
        {
            gameObjectRef.transform.parent = pooledObjectFolder.transform;
        }
    }

    public GameObject GetUsableStandardHorizontalBullet()
    {
        var obj = (from item in standardHorizontalBulletObjectPool
                   where item.activeSelf == false
                   select item).FirstOrDefault();

        if (obj != null)
        {
            return obj;
        }

        Debug.Log("<color=orange>WARNING: Ran out of reusable std horizontal bullet objects! Now instantiating a new one</color>");
        var stdBullet = (GameObject)Instantiate(instance.standardHorizonalBulletPrefab);

        SetParentTransform(stdBullet);

        stdBullet.SetActive(false);
        standardHorizontalBulletObjectPool.Add(stdBullet);

        stdBullet.name = stdBullet.name + "_INSTANTIATED";

        return stdBullet;
    }

    public GameObject GetUsableSphereBullet()
    {
        var obj = (from item in sphereBulletObjectPool
                   where item.activeSelf == false
                   select item).FirstOrDefault();

        if (obj != null)
        {
            return obj;
        }

        Debug.Log("<color=orange>WARNING: Ran out of reusable sphere bullet objects! Now instantiating a new one</color>");
        var sphereBullet = (GameObject)Instantiate(instance.sphereBulletPrefab);

        SetParentTransform(sphereBullet);

        sphereBullet.SetActive(false);
        sphereBulletObjectPool.Add(sphereBullet);

        sphereBullet.name = sphereBullet.name + "_INSTANTIATED";

        return sphereBullet;
    }

    public GameObject GetUsableSparkParticle()
    {
        var obj = (from item in sparkObjectPool
                   where item.activeSelf == false
                   select item).FirstOrDefault();

        if (obj != null)
        {
            return obj;
        }

        Debug.Log("<color=orange>WARNING: Ran out of reusable spark objects! Now instantiating a new one</color>");
        var sparkParticle = (GameObject)Instantiate(instance.sparkPrefab);

        SetParentTransform(sparkParticle);

        sparkParticle.SetActive(false);
        sparkObjectPool.Add(sparkParticle);

        sparkParticle.name = sparkParticle.name + "_INSTANTIATED";

        return sparkParticle;
    }
}
