using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool rebuild = false;
    public int stage;
    public GameObject roomController;
    public float initialHealth_p, initialHealth_e, initialHealth_b;
    public int currentExp, totalExp, expGained_e, expGained_b, expGained_p;
    public float totalStamina_p,totalStamina_e,totalStamina_b;
    public float currentStamina_p, currentStamina_e, currentStamina_b;
    public float stamina_p, stamina_e, stamina_b;
    public float regenRate_p, regenRate_e, regenRate_b;
    public float attackDamage_p, attackDamage_e,attackDamage_b;

    private GameObject[] resetEnemies, resetObstacles;
    private GameObject player;

    void Awake()
    {
        initialHealth_p = 100f;
        totalExp = 500;
        currentExp = 0;

        initialHealth_e = 50f;
        expGained_e = 10;

        initialHealth_b = 50f;
        expGained_b = 550;

        totalStamina_p = 100f;
        attackDamage_p = 50f;
        stamina_p = 40f;
        regenRate_p = 10f;

        totalStamina_e = 5f;
        attackDamage_e = 5f;
        stamina_e = 5f;
        regenRate_e = 0.01f;

        totalStamina_b = 50f;
        attackDamage_b = 0.01f;
        stamina_b = 50f;
        regenRate_b = 0.1f;
    }
    // Start is called before the first frame update
    void Start()
    {
        stage = 1;
        player = GameObject.Find("Player");




    }

    // Update is called once per frame
    void Update()
    {
        //if boss have been defeated and the player has interacted with the portal,
        //re-instantiate RoomController to generate new map
        if (rebuild)
        {
            resetEnemies = GameObject.FindGameObjectsWithTag("enemy");
            resetObstacles = GameObject.FindGameObjectsWithTag("obstacle");
            for (var i = 0; i < resetEnemies.Length; i++)
            {
                if (resetEnemies[i] != null)
                {
                    Destroy(resetEnemies[i]);
                }

            }



            for (var i = 0; i < resetObstacles.Length; i++)
            {
                if (resetObstacles[i] != null)
                {
                    Destroy(resetObstacles[i]);
                }

            }
            Destroy(GameObject.Find("RoomController"));
            //Return Player to the start room
            player.transform.position = new Vector3(0, 0, 0);
            StartCoroutine(spawn());
            rebuild = false;
            stage += 1;

            //Difficulty will increase as the stage progresses
            initialHealth_e += 50f;
            expGained_e -= 5;

            initialHealth_b += 50f;
            expGained_b -= 10;

            totalStamina_e += 5f;
            attackDamage_e += 5f;
            stamina_e += 5f;
            regenRate_e += 0.01f;


            totalStamina_b += 50f;
            attackDamage_b += 0.01f;
            stamina_b += 50f;
            regenRate_b += 0.1f;


        }

        if (currentExp >= totalExp)
        {
            currentExp -= totalExp;
            totalExp += 50;
            initialHealth_p += 50f;

            totalStamina_p += 50f;
            attackDamage_p += 20f;
            stamina_p += 50f;
            regenRate_p += 10f;

            
        }
        
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(roomController);
    }
}
