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
    }

    public GridController grid;
    public ObjectSpawner[] objectData;

    void Start()
    {
        //grid = GetComponentInChildren<GridController>();
    }

    public void IntializeSpawning()
    {
        foreach (ObjectSpawner os in objectData)
        {
            SpawnObject(os);
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
