using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls the Health of Player, Enemy and Boss
public class HealthController : MonoBehaviour
{
    public float initialHealth;

    public GameObject[] oldgameObjects;
    public GameObject[] savedGameObjects;
    public bool isBossDead;
    public Color fullHealthColor = Color.red;
    public Color zeroHealthColor = Color.gray;  
    public Slider slider; 
    public Image FillImage;
    public int currentExp, totalExp,expGained;

    private float currentHealth;
    private bool isDead;
    private bool saved = false;
    private GameObject camera;

    Vector3 startPos;

    void Awake()
    {
        if (gameObject.tag == "enemy")
        {
            initialHealth = 50f;
            expGained = 10;
        }
        else if (gameObject.tag == "boss")
        {
            initialHealth = 50f;
            expGained = 550;
        }
        else if (gameObject.tag == "Player")
        {
            initialHealth = 100f;
        }

        
    }

    void Start()
    {
        startPos = transform.position;
        currentHealth = initialHealth;
        Debug.Log(gameObject.name + " Starting Health: " + currentHealth.ToString());
        isBossDead = false;
        totalExp = 500;
        currentExp = 0;
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

        if (currentExp >= totalExp)
        {
            currentExp -= totalExp;
            totalExp += 50;
            initialHealth += 50f;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (gameObject.tag == "Player")
            {
                currentExp += 550;
            }

        }
    }

    IEnumerator stopSaving()
    {
        yield return new WaitForSeconds(3);
        saved = true;
    }

    private void OnEnable()
    {
        currentHealth = initialHealth;
        isDead = false;
        SetHealthUI();
        
    }

    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        slider.value = currentHealth;
        FillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / initialHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        //Debug.Log(gameObject.name + " current Health: " + currentHealth.ToString());
        SetHealthUI();
        if (currentHealth <= 0f && !isDead)
        {

            OnDeath();
            
        }

    }


    private void OnDeath()
    {
        //If its the player, respawn back to start point, else just deactivate it
        if (gameObject.tag == "Player")
        {
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
            transform.position = startPos;
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
            currentExp += expGained;

            var player = GameObject.Find("Player");

            player.GetComponent<HealthController>().isBossDead = true;

            var bossRoom = GameObject.FindWithTag("bossRoom").GetComponent<StageController>();

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
                currentExp += expGained;
            }
        }
        
    }
}
