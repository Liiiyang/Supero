using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

//Controls the conditions to go to the next level
public class PortalController : MonoBehaviour
{
    public Collider2D col_bottom;
    public Collider2D col_left;
    public Collider2D col_up;
    public Collider2D col_right;
    public bool spawnBoss;

    private GameObject bossRoom;
    private GameObject hiddenRoom;
    private GameObject rightRoom,leftRoom, topRoom, bottomRoom;
    private bool rightOpen, leftOpen, topOpen, bottomOpen;

    private int x, y;

    private string attack_button, action_button;
    private bool nextLevel;
    private bool pressed = false;

    void Awake()
    {


    }
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

        attack_button = "Attack";
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
       
        if (gameObject.tag == "hiddenRoom")
        {
            Debug.Log("Hidden Room name: " + gameObject.name);
            var coordinates = Regex.Matches(gameObject.name, @"[+-]?\d+(\.\d+)?");
            Debug.Log("X: " + coordinates[0]);
            Debug.Log("Y: " + coordinates[1]);
            x = int.Parse(coordinates[0].Value);
            y = int.Parse(coordinates[1].Value);
            StartCoroutine(FindCorrectRoom());
        }


        if (pressed)
        {
            Debug.Log("pressed Once");
        }

    }

    IEnumerator FindCorrectRoom()
    {
        yield return new WaitForSeconds(0.5f);
        //Finding the correct rooms that are beside the Hidden Room
        if(GameObject.Find("-Normal " + (x + 1).ToString() + ", " + y.ToString())!=null)
        {
            rightRoom = GameObject.Find("-Normal " + (x + 1).ToString() + ", " + y.ToString());
            Debug.Log("Correct Right Room: " + rightRoom.name);
        }
        if(GameObject.Find("-Normal " + (x - 1).ToString() + ", " + y.ToString()) !=null)
        {
            leftRoom = GameObject.Find("-Normal " + (x - 1).ToString() + ", " + y.ToString());
            Debug.Log("Correct Left Room: " + leftRoom.name);
        }
        if (GameObject.Find("-Normal " + x.ToString() + ", " + (y+1).ToString()) !=null)
        {
            topRoom = GameObject.Find("-Normal " + x.ToString() + ", " + (y + 1).ToString());
            Debug.Log("Correct Top Room: " + topRoom.name);
        }
        if (GameObject.Find("-Normal " + x.ToString() + ", " + (y - 1).ToString()) != null)
        {
            bottomRoom = GameObject.Find("-Normal " + x.ToString() + ", " + (y - 1).ToString());
            Debug.Log("Correct Bottom Room: " + bottomRoom.name);
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

        //Activate Hidden Room and set Wall to inactive to reveal door, set trigger to false
        if (Input.GetButtonDown(attack_button) && other.gameObject.name == "rightDoorh")
        {
            Debug.Log("Right Door Deactivated! ");
            var rightDoor = GameObject.FindWithTag("rightDoorh");
            rightDoor.GetComponent<Collider2D>().isTrigger = true;
            rightDoor.SetActive(false);
        }
        if (Input.GetButtonDown(attack_button) && other.gameObject.name == "topDoorh")
        {

            Debug.Log("Top Door Deactivated!");
            var topDoor = GameObject.FindWithTag("topDoorh");
            topDoor.GetComponent<Collider2D>().isTrigger = true;
            topDoor.SetActive(false);
        }
        if (Input.GetButtonDown(attack_button) && other.gameObject.name == "leftDoorh")
        {

            Debug.Log("Left Door Deactivated! ");
            var leftDoor = GameObject.FindWithTag("leftDoorh");
            leftDoor.GetComponent<Collider2D>().isTrigger = true;
            leftDoor.SetActive(false);
        }
        if (Input.GetButtonDown(attack_button) && other.gameObject.name == "bottomDoorh")
        {

            Debug.Log("Bottom Door Deactivated!");
            var bottomDoor = GameObject.FindWithTag("bottomDoorh");
            bottomDoor.GetComponent<Collider2D>().isTrigger = true;
            bottomDoor.SetActive(false);
        }
    }
}
