using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the Health of Player, Enemy and Boss
public class HealthController : MonoBehaviour
{
    public float initialHealth;

    public GameObject[] oldgameObjects;
    public GameObject[] savedGameObjects;

    private float currentHealth;
    private bool isDead;
    private bool saved = false;

    public bool isBossDead;
    private GameObject camera;

    Vector3 startPos;

    void Awake()
    {
        if (gameObject.tag == "enemy")
        {
            initialHealth = 50f;
        }
        else if (gameObject.tag == "boss")
        {
            initialHealth = 50f;
        }
        else if (gameObject.tag == "Player")
        {
            initialHealth = 70f;
        }

        
    }

    void Start()
    {
        startPos = transform.position;
        currentHealth = initialHealth;
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
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " current Health: " + currentHealth.ToString());

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
                savedGameObjects[i].SetActive(false);
            }
            camera = GameObject.Find("Main Camera");
            camera.GetComponent<CameraController>().reset(true);
            transform.position = startPos;
            for (var i = 0; i < savedGameObjects.Length; i++)
            {
                savedGameObjects[i].SetActive(true);
            } 
        }
        else if (gameObject.tag == "boss")
        {
            
            //If boss dies, despawn and send bool to player so he can toggle checkpoint again to 
            //go next stage
            gameObject.SetActive(false);

            var player = GameObject.Find("Player");

            player.GetComponent<HealthController>().isBossDead = true;

            
        }
        else
        {
            gameObject.SetActive(false);

        }
        
    }
}
