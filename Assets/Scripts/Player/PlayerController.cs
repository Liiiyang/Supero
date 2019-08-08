using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool activateChest;
    public GameObject chest;
    private Rigidbody2D rb2d;
    private Vector2 moveVelocity;
    private string attack_button, action_button, heal_button;
    private GameObject hiddenRoom, gameManager, player;
    private GameObject rightRoom,leftRoom, topRoom, bottomRoom;
    private int x, y;
    private GameManager gm;
    private HealthController hc;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
        player = GameObject.Find("Player");
        hc = player.GetComponent<HealthController>();
        attack_button = "Attack";
        action_button = "Action";
        heal_button = "Heal";
    }

    void Update()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        moveVelocity = movement.normalized * speed;

        //Debug.Log("Player current x: " + gameObject.transform.position.x.ToString() + " Player current y: " + gameObject.transform.position.y.ToString());

        hiddenRoom = GameObject.FindWithTag("hiddenRoom");

        if (hiddenRoom != null)
        {
            Debug.Log("Hidden Room name: " + hiddenRoom.name);
            var coordinates = Regex.Matches(hiddenRoom.name, @"[+-]?\d+(\.\d+)?");
            Debug.Log("X: " + coordinates[0]);
            Debug.Log("Y: " + coordinates[1]);
            x = int.Parse(coordinates[0].Value);
            y = int.Parse(coordinates[1].Value);
            StartCoroutine(FindCorrectRoom());
        }

        if (Input.GetButtonDown(heal_button))
        {
            if (gm.currentPotions != 0)
            {
                gm.currentPotions -= 1;
                if ((gm.initialHealth_p - gm.currentHealth) > gm.regenHealth)
                {
                    gm.currentHealth += gm.regenHealth;
                    hc.SetHealthUI();
                }
                else
                {
                    gm.currentHealth = gm.initialHealth_p;
                    hc.SetHealthUI();
                }

            }
            
        }
    }

    IEnumerator FindCorrectRoom()
    {
        yield return new WaitForSeconds(0.5f);
        //Finding the correct rooms that are beside the Hidden Room
        if (GameObject.Find("-Normal " + (x + 1).ToString() + ", " + y.ToString()) != null)
        {
            rightRoom = GameObject.Find("-Normal " + (x + 1).ToString() + ", " + y.ToString());
            Debug.Log("Correct Right Room: " + rightRoom.name);
        }
        if (GameObject.Find("-Normal " + (x - 1).ToString() + ", " + y.ToString()) != null)
        {
            leftRoom = GameObject.Find("-Normal " + (x - 1).ToString() + ", " + y.ToString());
            Debug.Log("Correct Left Room: " + leftRoom.name);
        }
        if (GameObject.Find("-Normal " + x.ToString() + ", " + (y + 1).ToString()) != null)
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

    void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + moveVelocity * Time.fixedDeltaTime);        
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log("Collided: " + other.gameObject.name);
        //Activate Hidden Room and set wall to inactive to reveal door, set trigger to false
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

        if (Input.GetButtonDown(action_button) && other.gameObject.name == "ghost(Clone)")
        {
            Destroy(other.gameObject);
            gameObject.GetComponent<HealthController>().oddDeaths = false;
            gm.currency_p += gm.saved_currency;
        }

        if (Input.GetButtonDown(action_button) && other.gameObject.name == "shopkeeper")
        {
            //Activate Shop Menu
            Debug.Log("Shop Activated");
        }
    }    
}
