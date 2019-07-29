using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstaclesList : MonoBehaviour
{
    [System.Serializable]
    public struct Obstacles
    {
        public GameObject obstacle;
        public float weight;
    }

    public List<Obstacles> obstacleList = new List<Obstacles>();

    float totalWeight;

    void Awake()
    {
        totalWeight = 0;
        foreach (var Obstacles in obstacleList)
        {
            totalWeight += Obstacles.weight;
        }
    }
    void Start()
    {
        float pick = Random.value * totalWeight;
        int chosenIndex = 0;
        float cumulativeWeight = obstacleList[0].weight;

        while (pick > cumulativeWeight && chosenIndex < obstacleList.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += obstacleList[chosenIndex].weight;
        }

        GameObject i = Instantiate(obstacleList[chosenIndex].obstacle, transform.position, Quaternion.identity) as GameObject;

    }
}
