using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls the Health of Player, Enemy and Boss
public class HealthController : MonoBehaviour
{
    public GameObject[] oldgameObjects;
    public GameObject[] savedGameObjects;
    public bool isBossDead;
    public Color fullHealthColor = Color.red;
    public Color zeroHealthColor = Color.gray;  
    public Slider slider; 
    public Image FillImage;
    public GameObject ghost;
    public bool oddDeaths;

    private int totalExp,expGained, currency_e, currency_b;
    private float currentHealth,initialHealth;
    private bool isDead;
    private bool saved = false;
    private GameObject camera,gameManager,deadPoint;
    private GameManager gm;
    private Transform OldPosition;
    
    
    Vector3 startPos;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
        if (gameObject.tag == "enemy")
        {
            initialHealth = gm.initialHealth_e;
            slider.maxValue = initialHealth;
            expGained = gm.expGained_e;
            currency_e = gm.currency_e;
            currentHealth = gm.currentHealth_e;
        }
        else if (gameObject.tag == "boss")
        {
            initialHealth = gm.initialHealth_b;
            slider.maxValue = initialHealth;
            expGained = gm.expGained_b;
            currency_b = gm.currency_b;
            currentHealth = gm.currentHealth_b;
        }
        else if (gameObject.tag == "Player")
        {
            initialHealth = gm.initialHealth_p;
            slider.maxValue = initialHealth;
            currentHealth = gm.currentHealth_p;
            totalExp = gm.totalExp;
        }

        
    }

    void Start()
    {
        startPos = transform.position;
        oddDeaths = false;
        Debug.Log(gameObject.name + " Starting Health: " + currentHealth.ToString());
        isBossDead = false;
    }

    void Update()
    {
        if (!saved)
        {
            //Backup enemy gameobject to respawn after player dies
            oldgameObjects = GameObject.FindGameObjectsWithTag("enemy");
            savedGameObjects = GameObject.FindGameObjectsWithTag("enemy");
            StartCoroutine(stopSaving());
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (gameObject.tag == "Player")
            {
                gm.currentExp += 550;
                gm.currency_p += 200;
            }

        }
    }

    IEnumerator stopSaving()
    {
        yield return new WaitForSeconds(3);
        saved = true;
    }

    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        slider.value = currentHealth;
        FillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / initialHealth);
    }

    private void OnEnable()
    {
        currentHealth = initialHealth;
        isDead = false;
        SetHealthUI();
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        gm.currentHealth_p = currentHealth;
        //Debug.Log(gameObject.name + " current Health: " + currentHealth.ToString());
        SetHealthUI();
        if (currentHealth <= 0f && !isDead)
        {
            Invoke("OnDeath", 0.6f);
                       
        }
    }

    


    private void OnDeath()
    {
        //If its the player, respawn back to start point, else just deactivate it
        if (gameObject.tag == "Player")
        {
            //Debug.Log("Death Point: X" + transform.position.x.ToString() + " Y: " + transform.position.y.ToString());
            for (var i = 0; i < oldgameObjects.Length; i++)
            {
                if (savedGameObjects[i] != null)
                {
                    savedGameObjects[i].SetActive(false);
                }               
            }
            currentHealth = initialHealth;
            SetHealthUI();

            camera = GameObject.Find("Main Camera");
            camera.GetComponent<CameraController>().reset(true);
            OldPosition = transform;
            StartCoroutine(waitDeath(transform));

            // Leaves behind a ghost sprite when player dies
            if (!oddDeaths)
            {
                if (deadPoint != null)
                {
                    Destroy(deadPoint);
                }
                deadPoint = Instantiate(ghost, OldPosition.position, Quaternion.identity) as GameObject;
                gm.saved_currency = gm.currency_p;
                gm.currentStamina_p = gm.totalStamina_p;
                gm.currentHealth_p = gm.initialHealth_p;
                gm.currency_p = 0;
                gm.current_currency = 0;
                oddDeaths = true;

            }
            else
            {
                if (deadPoint != null)
                {
                    Destroy(deadPoint);
                    deadPoint = Instantiate(ghost,OldPosition.position, Quaternion.identity) as GameObject;
                    gm.saved_currency = gm.currency_p;
                    gm.currentHealth_p = gm.initialHealth_p;
                    gm.currentStamina_p = gm.totalStamina_p;
                    gm.currency_p = 0;
                    gm.current_currency = 0;
                    oddDeaths = false;
                }
            }

            for (var i = 0; i < savedGameObjects.Length; i++)
            {
                if (savedGameObjects[i] != null)
                {
                    savedGameObjects[i].SetActive(true);
                }               
            } 

        }
        else if (gameObject.tag == "boss")
        {
            
            //If boss dies, despawn and send bool to player so he can toggle checkpoint again to 
            //go next stage
            gameObject.SetActive(false);
            gm.currentExp += expGained;
            gm.currency_p += currency_b;

            var player = GameObject.Find("Player");

            player.GetComponent<HealthController>().isBossDead = true;

            var bossRoom = GameObject.FindWithTag("bossRoom").GetComponent<PortalController>();

            bossRoom.col_bottom.isTrigger = true;
            bossRoom.col_up.isTrigger = true;
            bossRoom.col_left.isTrigger = true;
            bossRoom.col_right.isTrigger = true;           
        }
        else
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
                gm.currentExp += expGained;
                gm.currency_p += currency_e;
            }
        }
        
    }

    IEnumerator waitDeath(Transform location)
    {
        yield return new WaitForSeconds(2f);
        location.position = startPos;
        //GameObject cm = GameObject.Find("currencyMessage(Clone)");
    }
}
