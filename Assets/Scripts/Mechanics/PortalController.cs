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
    public GameObject portalMessage, portalMessageExit;
    public bool spawnPortalMessageOnce;
    private AudioSource[] audioSources;

    private GameObject bossRoom;
    private string action_button;
    private bool nextLevel;
    private bool pressed = false;
    private bool spawned = false;
    private GameObject InstantiatedportalMessage, InstantiatedportalMessageExit ;
    private GameObject player;
    private HealthController hc;
    private Transform playerPosition;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float msDistance;
    private GameObject bossRoomPosition;
    private Room roomScript;
    


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

        spawnPortalMessageOnce = false;

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

        var bossMusic = GameObject.Find("Player");
        audioSources = bossMusic.GetComponents<AudioSource>();
        if (!audioSources[3].isPlaying)
        {
            bossRoomPosition = GameObject.FindGameObjectWithTag("bossRoom");
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
            if (bossRoomPosition != null && spawned)
            {
                roomScript = bossRoomPosition.GetComponent<Room>();
                minX = roomScript.leftwall.transform.position.x + 20;
                maxX = roomScript.rightwall.transform.position.x - 20;
                minY = roomScript.bottomwall.transform.position.y + 20;
                maxY = roomScript.topwall.transform.position.y - 20;
                if (playerPosition.position.x > minX && playerPosition.position.x < maxX && playerPosition.position.y > minY && playerPosition.position.y < maxY)
                {
                    var levelMusic = GameObject.Find("GameManager").GetComponent<AudioSource>();
                    levelMusic.Pause();
                    //audioSources[3].Play();

                }
            }
            

        }
        
        
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log("Collided: " + other.gameObject.name);
        if (gameObject.name == "Player")
        {
            if (other.gameObject.name == "portal" && !spawnPortalMessageOnce)
            {
                if (!nextLevel)
                {
                    InstantiatedportalMessage = Instantiate(portalMessage, transform.position, Quaternion.identity) as GameObject;
                    spawnPortalMessageOnce = true;
                }
                else
                {
                    InstantiatedportalMessageExit = Instantiate(portalMessageExit, transform.position, Quaternion.identity) as GameObject;
                    spawnPortalMessageOnce = true;
                }

            }

            if (other.gameObject.name == "portal" && Input.GetButtonDown(action_button))
            {
                //Spawns the Boss the first time it is pressed, and all doors will be closed
                if (!pressed)
                {
                    var bossMusic = GameObject.Find("Player");
                    audioSources = bossMusic.GetComponents<AudioSource>();
                    audioSources[3].Play();
                    var levelMusic = GameObject.Find("GameManager").GetComponent<AudioSource>();
                    levelMusic.Pause();
                    spawnBoss = true;
                    pressed = true;
                    spawned = true;

                    var bossRoom = GameObject.FindWithTag("bossRoom").GetComponent<PortalController>();

                    bossRoom.spawnBoss = true;

                    bossRoom.col_bottom.isTrigger = false;
                    bossRoom.col_up.isTrigger = false;
                    bossRoom.col_left.isTrigger = false;
                    bossRoom.col_right.isTrigger = false;

                }
            }
            if (pressed && nextLevel)
            {
                if (other.gameObject.name == "portal" && Input.GetButtonDown(action_button))
                {
                    //Boss is defeated and go to next level
                    this.GetComponent<Animator>().enabled = true;
                    this.GetComponent<Animator>().SetBool("enter", true);
                    Invoke("nextStage", 2f);


                }


            }
        }
        
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "portal")
        {
            if (!nextLevel)
            {
                Destroy(InstantiatedportalMessage);
                var player = GameObject.Find("Player").GetComponent<PortalController>();
                player.spawnPortalMessageOnce = false;
            }
            else
            {
                Destroy(InstantiatedportalMessageExit);
                var player = GameObject.Find("Player").GetComponent<PortalController>();
                player.spawnPortalMessageOnce = false;
            }
        }
    }

    private void nextStage()
    {
        var gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<GameManager>().rebuild = true;
    }


}
