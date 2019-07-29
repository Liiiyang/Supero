using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    [System.Serializable]
    public struct Enemies
    {
        public GameObject enemy;
        public float weight;
    }
    public List<Enemies> enemyList = new List<Enemies>();

    float totalWeight;

    void Awake()
    {
        totalWeight = 0;
        foreach(var Enemies in enemyList)
        {
            totalWeight += Enemies.weight;
        }
    }
    void Start()
    {
        float pick = Random.value * totalWeight;
        int chosenIndex = 0;
        float cumulativeWeight = enemyList[0].weight;

        while (pick > cumulativeWeight && chosenIndex < enemyList.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += enemyList[chosenIndex].weight;
        }

        GameObject i = Instantiate(enemyList[chosenIndex].enemy, transform.position, Quaternion.identity) as GameObject;

    }
}
