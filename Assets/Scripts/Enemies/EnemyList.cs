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

    private GameObject gameManager;
    private GameManager gm;
    private List<Enemies> enemyListSS = new List<Enemies>();
    private List<Enemies> enemyListTS = new List<Enemies>();

    void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
        Debug.Log("Found the GameManager: " + gameManager.name);
        Debug.Log("Current Stage: " + gm.stage.ToString());

        enemyListSS.Add(enemyList[0]);
        enemyListSS.Add(enemyList[1]);
        enemyListTS.Add(enemyList[0]);
        enemyListTS.Add(enemyList[1]);
        enemyListTS.Add(enemyList[2]);
        
        totalWeight = 0;
        if (gm.stage == 1)
        {
            totalWeight += enemyList[0].weight;
        }
        else if (gm.stage == 2)
        {
            for (int i = 0; i < enemyListSS.Count; i++)
            {

                totalWeight += enemyListSS[i].weight;
            }


        }
        else if (gm.stage == 3)
        {
            for (int i = 0; i < enemyListTS.Count; i++)
            {

                totalWeight += enemyListTS[i].weight;
            }
        }
        else
        {
            for (int i = 0; i < enemyList.Count; i++)
            {

                totalWeight += enemyList[i].weight;
            }
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
