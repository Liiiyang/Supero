using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject roomController;
    public TextMeshProUGUI currencyText, potionText, liveCounterText;
    public int stage;
    public float initialHealth_p, initialHealth_e, initialHealth_b, regenHealth, currentHealth_p, currentHealth_e, currentHealth_b;
    public int currentExp, totalExp, expGained_e, expGained_b, expGained_p;
    public float totalStamina_p,totalStamina_e,totalStamina_b;
    public float currentStamina_p, currentStamina_e, currentStamina_b;
    public float stamina_p, stamina_e, stamina_b;
    public float regenRate_p, regenRate_e, regenRate_b;
    public float attackDamage_p, attackDamage_e,attackDamage_b;
    public int currency_p, currency_e, currency_b, saved_currency, current_currency;
    public bool rebuild = false;
    public int liveCounter,currentPotions;

    private GameObject[] resetEnemies, resetObstacles;
    private GameObject player;

    void Awake()
    {
        initialHealth_p = 400f;
        totalExp = 500;
        currentExp = 0;

        initialHealth_e = 73f;
        expGained_e = 10;

        initialHealth_b = 100f;
        expGained_b = 550;

        totalStamina_p = 93f;
        attackDamage_p = 73f;
        stamina_p = 19f;
        regenRate_p = 40f;

        totalStamina_e = 400f;
        attackDamage_e = 60f;
        stamina_e = 60f;
        regenRate_e = 30f;

        totalStamina_b = 1000f;
        attackDamage_b = 80f;
        stamina_b = 120f;
        regenRate_b = 40f;

        currency_p = 0;
        current_currency = 0;
        saved_currency = 0;
        currency_e = 100;
        currency_b = 2000;

        currentPotions = 5;
        regenHealth = 250f;
        currentHealth_p = initialHealth_p;
        currentHealth_e = initialHealth_e;
        currentHealth_b = initialHealth_b;

        currentStamina_p = totalStamina_p;
        currentStamina_e = totalStamina_e;
        currentStamina_b = totalStamina_b;
        liveCounter = 3;

    }
    // Start is called before the first frame update
    void Start()
    {
        stage = 1;
        player = GameObject.Find("Player");
        liveCounterText.text = "0" + liveCounter.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //if boss have been defeated and the player has interacted with the portal,
        //re-instantiate RoomController to generate new map
        if (rebuild)
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                SceneManager.LoadScene("NewGame");
            }
            Destroy(GameObject.Find("poralMessageExit(Clone)"));
            liveCounter = 3;
            resetEnemies = GameObject.FindGameObjectsWithTag("enemy");
            resetObstacles = GameObject.FindGameObjectsWithTag("obstacle");
            currentHealth_p = initialHealth_p;
            currentStamina_p = totalStamina_p;
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

            initialHealth_b += 1000f;
            expGained_b += 300;

            totalStamina_e += 10f;
            attackDamage_e += 10f;
            stamina_e += 10f;
            regenRate_e += 15f;


            totalStamina_b += 500f;
            attackDamage_b += 40f;
            stamina_b += 50f;
            regenRate_b += 20f;

            currency_e += 60;
            currency_b += 1000;

            currentPotions += 2;
            regenHealth += 70;
        }

        if (currentExp >= totalExp)
        {
            currentExp -= totalExp;
            totalExp += 50;
            initialHealth_p += 100f;
            totalStamina_p += 2f;
            attackDamage_p += 20f;
            stamina_p += 4f;
            regenRate_p += 20f;           
        }

        // Currency Mechanics showing slow increment of currency in the UI
        if (current_currency == 0)
        {
            currencyText.text = current_currency.ToString();
        }
        if (current_currency < currency_p)
        {
            float ScoreIncrement = Time.deltaTime * 200;
            Debug.Log("Currency: " + ScoreIncrement.ToString());
            current_currency += (Mathf.RoundToInt(ScoreIncrement));
            if (current_currency > currency_p)
            {
                current_currency = currency_p;
            }
            currencyText.text = current_currency.ToString();
            
        }

        potionText.text = currentPotions.ToString();

        liveCounterText.text = "0" + liveCounter.ToString();
        
    }

    IEnumerator currencyTrackingEffect()
    {
        currencyText.text = currency_p.ToString();
        yield return new WaitForSeconds(1f);
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(roomController);
    }
}
