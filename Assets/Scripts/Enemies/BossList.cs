using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossList : MonoBehaviour
{
    [System.Serializable]
    public struct Bosses
    {
        public GameObject boss;
        public float weight;
    }
    public List<Bosses> bossList = new List<Bosses>();

    float totalWeight;

    void Awake()
    {

        totalWeight = 0;
        for (int i = 0; i < bossList.Count; i++)
        {

            totalWeight += bossList[Random.Range(0,bossList.Count)].weight;
        }

    }
    void Start()
    {
        float pick = Random.value * totalWeight;
        int chosenIndex = 0;
        float cumulativeWeight = bossList[0].weight;

        while (pick > cumulativeWeight && chosenIndex < bossList.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += bossList[chosenIndex].weight;
        }

        GameObject i = Instantiate(bossList[chosenIndex].boss, transform.position, Quaternion.identity) as GameObject;

    }
}
