using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct ObjectSpawner
    {
        public string name;

        public ObjectData objectData;

        public bool spawn;
    }

    public GridController grid;
    public ObjectSpawner[] objectData;
    public bool spawning;

    public StageController sc;

    void Awake()
    {
        sc = GetComponent<StageController>();
    }
    void Start()
    {
        
        //grid = GetComponentInChildren<GridController>();
    }

    void Update()
    {
        spawning = sc.spawnBoss;
        if (spawning)
        {
            Debug.Log("True");
        }
    }
    public void IntializeSpawning()
    {
        foreach (ObjectSpawner os in objectData)
        {
            if (spawning && os.name == "Bosses")
            {
                SpawnObject(os);
                spawning = false;
            }
            else if (os.spawn == true)
            {
                SpawnObject(os);
            }
            
        }
    }
    void SpawnObject(ObjectSpawner data)
    {
        int randomIteration = Random.Range(data.objectData.minSpawn, data.objectData.maxSpawn + 1);
        for (int i = 0; i < randomIteration; i++)
        {
            int randomPos = Random.Range(0, grid.availablePoints.Count - 1);
            GameObject go = Instantiate(data.objectData.objectsToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform) as GameObject;
            grid.availablePoints.RemoveAt(randomPos);
            Debug.Log("Object Spawned");
        }
    }
}
