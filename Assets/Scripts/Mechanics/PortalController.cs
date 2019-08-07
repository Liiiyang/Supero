using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Controls the portal mechanics 
public class PortalController : MonoBehaviour
{
    public Collider2D col_bottom;
    public Collider2D col_left;
    public Collider2D col_up;
    public Collider2D col_right;
    public bool spawnBoss;

    private GameObject bossRoom;
    private string action_button;
    private bool nextLevel;
    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
       
        if (gameObject.tag != "bossRoom")
        {
            if (col_bottom == null && col_left == null || col_up == null || col_right == null)
            {
                return;
            }
        }

        
        action_button = "Action";

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
        if (Input.GetButtonDown(action_button) && other.gameObject.name == "portal")
        {
            //Spawns the Boss the first time it is pressed, and all doors will be closed
            if (!pressed)
            {
                spawnBoss = true;
                pressed = true;

                var bossRoom = GameObject.FindWithTag("bossRoom").GetComponent<PortalController>();

                bossRoom.spawnBoss = true;

                bossRoom.col_bottom.isTrigger = false;
                bossRoom.col_up.isTrigger = false;
                bossRoom.col_left.isTrigger = false;
                bossRoom.col_right.isTrigger = false;

            }
            else if (pressed && nextLevel)
            {
                //Boss is defeated and go to next level
                var gameManager = GameObject.Find("GameManager");
                gameManager.GetComponent<GameManager>().rebuild = true;


            }
        }
    }
}
