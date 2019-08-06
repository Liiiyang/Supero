using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool rebuild = false;
    public int stage;
    public GameObject roomController;
    private GameObject[] resetEnemies, resetObstacles;
    // Start is called before the first frame update
    void Start()
    {
        stage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if boss have been defeated and the player has interacted with the portal,
        //re-instantiate RoomController to generate new map
        if (rebuild)
        {
            resetEnemies = GameObject.FindGameObjectsWithTag("enemy");
            resetObstacles = GameObject.FindGameObjectsWithTag("obstacle");
            for (var i = 0; i < resetEnemies.Length; i++)
            {
                if (resetEnemies[i] != null)
                {
                    Destroy(resetEnemies[i]);
                }

            }



            for (var i = 0; i < resetObstacles.Length; i++)
            {
                if (resetObstacles[i] != null)
                {
                    Destroy(resetObstacles[i]);
                }

            }
            Destroy(GameObject.Find("RoomController"));
            StartCoroutine(spawn());
            rebuild = false;
            stage += 1;
        }
        
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(roomController);
    }
}
