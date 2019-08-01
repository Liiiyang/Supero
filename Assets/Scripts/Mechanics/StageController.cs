using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Controls the levelling mechancis
public class StageController : MonoBehaviour
{
    public bool spawnBoss;

    private bool pressed = false;

    private bool nextLevel;
    void Awake()
    {


    }
    // Start is called before the first frame update
    void Start()
    {
        spawnBoss = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Player")
        {
            nextLevel = GetComponent<HealthController>().isBossDead;
            if (GetComponent<HealthController>().isBossDead)
            {
                Debug.Log("Boss is dead");
            }
            
        }

        if (pressed)
        {
            Debug.Log("pressed Once");
        }        
    }

    

    void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log("Collided: " + other.gameObject.name);
        if (Input.GetKey(KeyCode.E))
        {
            //Spawns the Boss the first time it is pressed
            if (!pressed)
            {
                spawnBoss = true;
                pressed = true;
            }
            else if (pressed && nextLevel)
            {
                //Boss is defeated and go to next level
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Debug.Log("New Stage");
            }
        }
    }
}
