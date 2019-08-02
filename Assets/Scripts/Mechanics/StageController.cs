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

    public Collider2D col_bottom;
    public Collider2D col_left;
    public Collider2D col_up;
    public Collider2D col_right;

    private GameObject bossRoom;
    private StageController sc;

    void Awake()
    {


    }
    // Start is called before the first frame update
    void Start()
    {
        spawnBoss = false;

        if (gameObject.tag != "bossRoom")
        {
            if (col_bottom == null && col_left == null || col_up == null || col_right == null)
            {
                return;
            }
        }
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
        if (Input.GetKey(KeyCode.E) && other.gameObject.name == "portal")
        {
            //Spawns the Boss the first time it is pressed, and all doors will be closed
            if (!pressed)
            {
                spawnBoss = true;
                pressed = true;

                var bossRoom = GameObject.FindWithTag("bossRoom").GetComponent<StageController>();

                bossRoom.spawnBoss = true;

                bossRoom.col_bottom.isTrigger = false;
                bossRoom.col_up.isTrigger = false;
                bossRoom.col_left.isTrigger = false;
                bossRoom.col_right.isTrigger = false;

            }
            else if (pressed && nextLevel)
            {
                //Boss is defeated and go to next level
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Debug.Log("New Stage");
                // level tracking and also scale the attack
            }
        }

        //Activate Hidden Room
        if (Input.GetKey(KeyCode.E) && other.gameObject.name == "hiddenDoor")
        {

        }
    }
}
