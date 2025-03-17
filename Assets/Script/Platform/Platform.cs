using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] prefabList; // List of prefabs to spawn
    public Transform player; // Reference to the player
    public float spawnDistance = 40f; // Distance from the player to spawn objects
    public float despawnDistance = 20f; // Distance behind the player to despawn objects
    public int poolSize = 2; // Size of the object pool

    public Transform GroundPosition;
    public Transform parent;

    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private List<GameObject> activeObjects = new List<GameObject>();
    private int currentPrefabIndex = 0; // Track the current prefab index
    float initialDistance = 0;

    void Start()
    {
        InitializePool();
    }

    void Update()
    {
        SpawnObjects();
        CheckDespawn();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            // Spawn prefabs in sequence
            GameObject obj = Instantiate(prefabList[currentPrefabIndex]);
            obj.transform.localPosition = GroundPosition.position + new Vector3(0, 0, GroundPosition.position.z + initialDistance);
            obj.transform.SetParent(parent);
            obj.SetActive(false);
            objectPool.Enqueue(obj);

            // Move to the next prefab in the list
            currentPrefabIndex = (currentPrefabIndex + 1) % prefabList.Length;
        }
    }

    void SpawnObjects()
    {
        if (objectPool.Count > 0)
        {
            GameObject obj = objectPool.Dequeue();
            obj.transform.position = GroundPosition.position + new Vector3(0, 0, initialDistance);
            initialDistance += 10;
            obj.SetActive(true);
            activeObjects.Add(obj);
            foreach (var item in obj.GetComponent<PlatformItems>().PlatFormItems)
            {
                item.SetActive(true);
            } 
            // Move to the next prefab in the list
            currentPrefabIndex = (currentPrefabIndex + 1) % prefabList.Length;
        }
    }

    void CheckDespawn()
    {
        for (int i = activeObjects.Count - 1; i >= 0; i--)
        {
            if (activeObjects[i].transform.position.z < player.position.z - despawnDistance)
            {
                RecycleObject(activeObjects[i]);
                activeObjects.RemoveAt(i);
            }
        }
    }

    void RecycleObject(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}