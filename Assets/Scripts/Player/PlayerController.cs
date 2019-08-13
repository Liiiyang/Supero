using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool activateChest;
    public GameObject chest, shop;
    public Color fullHealthColor = Color.red;
    public Color zeroHealthColor = Color.gray;
    public Slider slider;
    public Image FillImage;
    public GameObject chestMessage, currencyMessage, hiddenMessage, shopMessage;
    public AudioSource heal_Sound, portal_Sound;
    public Animator playerAnimator;

    private Rigidbody2D rb2d;
    private Vector2 moveVelocity;
    private string attack_button, action_button, heal_button, shop_button;
    private GameObject hiddenRoom, gameManager, player;
    private GameObject rightRoom,leftRoom, topRoom, bottomRoom;
    private int x, y;
    private GameManager gm;
    private HealthController hc;
    private bool facingRight;
    private GameObject InstantiatedchestMessage, InstantiatedcurrencyMessage, InstantiatedhiddenMessage, InstantiatedshopMessage, Instantiatedshop;
    private bool spawnChestMessageOnce, spawnCurrencyMessageOnce, spawnhiddenMessageOnce, spawnshopMessageOnce, spawnShop, displayShop;
    private Material rightSide;
    

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
        attack_button = "Attack";
        action_button = "Action";
        heal_button = "Heal";
        shop_button = "Shop";
        facingRight = true;
        spawnShop = true;
        displayShop = false;
        
        //rightSide = this.GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        StartCoroutine(destroyAnimation());
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Controls walking animation for player
        if (moveHorizontal > 0 || moveVertical > 0)
        {
            playerAnimator.SetBool("walk", true);
            if (moveHorizontal > moveVertical) { playerAnimator.SetFloat("speed", moveHorizontal); }
            else { playerAnimator.SetFloat("speed", moveVertical); }
        }
        else
        {
            playerAnimator.SetBool("walk", false);
        }

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
                if ((gm.currentHealth_p + gm.regenHealth) <=gm.initialHealth_p)
                {
                    gm.currentHealth_p += gm.regenHealth;
                    SetHealthUI();
                    heal_Sound.Play();
                }
                else
                {
                    gm.currentHealth_p = gm.initialHealth_p;
                    SetHealthUI();
                    heal_Sound.Play();
                }

            }
            
        }

        if (Input.GetButtonDown(shop_button))
        {
            Destroy(Instantiatedshop);
        }

        
    }
    IEnumerator destroyAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        portal_Sound.Stop();
    }

    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        slider.value = gm.currentHealth_p;
        FillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, gm.currentHealth_p / gm.initialHealth_p);
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
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb2d.MovePosition(rb2d.position + moveVelocity * Time.fixedDeltaTime);
        Flip(moveHorizontal);      
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            //this.GetComponent<SpriteRenderer>().material = leftSide;
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log("Collided: " + other.gameObject.name);
        //Show Hidden Wall Prompt Message
        if ((other.gameObject.name == "rightDoorh" || other.gameObject.name == "topDoorh"
            || other.gameObject.name == "leftDoorh" || other.gameObject.name == "bottomDoorh") && !spawnhiddenMessageOnce)
        {
            InstantiatedhiddenMessage = Instantiate(hiddenMessage, transform.position, Quaternion.identity) as GameObject;
            spawnhiddenMessageOnce = true;
        }

        if (other.gameObject.name == "shopkeeper" && !spawnshopMessageOnce)
        {
            InstantiatedshopMessage = Instantiate(shopMessage, transform.position, Quaternion.identity) as GameObject;
            spawnshopMessageOnce = true;
        }
        if(other.gameObject.name =="ghost(Clone)" &&!spawnCurrencyMessageOnce)
        {
            InstantiatedcurrencyMessage = Instantiate(currencyMessage, transform.position, Quaternion.identity) as GameObject;
            spawnCurrencyMessageOnce = true;
        }
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

        if (Input.GetButtonDown(action_button) && other.gameObject.name == "shopkeeper" && !displayShop)
        {
            //Activate Shop Menu
            Debug.Log("Shop Activated");
            Instantiatedshop = Instantiate(shop, transform.position, Quaternion.identity) as GameObject;
            displayShop = true;
            //spawnShop = false;


        }

        if (other.gameObject.name == "chest(Clone)" && !spawnChestMessageOnce)
        {
            InstantiatedchestMessage = Instantiate(chestMessage, transform.position, Quaternion.identity) as GameObject;
            spawnChestMessageOnce = true;

        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "chest(Clone)")
        {
            Destroy(InstantiatedchestMessage);
            spawnChestMessageOnce = false;
        }

        if (other.gameObject.name == "ghost(Clone)")
        {
            Destroy(InstantiatedcurrencyMessage);
            spawnCurrencyMessageOnce = false;
        }
        if (other.gameObject.name == "rightDoorh" || other.gameObject.name == "topDoorh"
            || other.gameObject.name == "leftDoorh" || other.gameObject.name == "bottomDoorh")
        {
            Destroy(InstantiatedhiddenMessage);
            spawnhiddenMessageOnce = false;
        }
        if (other.gameObject.name == "shopkeeper" )
        {
            Destroy(InstantiatedshopMessage);
            spawnshopMessageOnce = false;
        }
    }
}
