using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject roomController, saveProgressText, levelupText;
    public GameObject[] inventory;
    public TextMeshProUGUI currencyText, potionText, liveCounterText, stagetext;
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
    public AudioSource levelMusic;
    public List<Item> currentShopList;

    private GameObject[] resetEnemies, resetObstacles;
    private GameObject player;
    private string save_button;
    private GameObject InstantiatedsaveMessage, InstantiatedLevelupMessage;
    private bool spawnMessageOnce = false;
    private bool spawnLevelupOnce = false;
    private AudioSource[] audioSources;
    private bool test = false;

    void Awake()
    {

        Debug.Log("Save Test: " + StaticSaveFile.save);
        if (StaticSaveFile.save == "save")
        {
            playerData data = SaveSystem.LoadPlayer();
            stage = data.stage;
        }
        else
        {
            stage = 1;
        }

        initialHealth_p = ((stage - 1) * 100f) + 400f;
        totalExp = ((stage - 1) * 50) + 500;
        currentExp = 0;

        initialHealth_e = ((stage - 1) * 50f) + 73f;
        expGained_e = 50;

        initialHealth_b = 100f;
        expGained_b = 50;

        totalStamina_p = ((stage - 1)* 2f)+ 93f;
        attackDamage_p = ((stage - 1)* 20f)+ 73f;
        stamina_p = ((stage - 1)* 4f)+ 19f;
        regenRate_p =((stage - 1)* 20f)+ 40f;

        totalStamina_e = ((stage - 1)* 10f) + 400f;
        attackDamage_e = ((stage - 1)* 10f) + 30f;
        stamina_e = ((stage - 1)*  10f) + 60f;
        regenRate_e = ((stage - 1)* 15f) + 30f;

        totalStamina_b = ((stage - 1)* 500f) + 1000f;
        attackDamage_b = ((stage - 1)* 40f) + 80f;
        stamina_b = ((stage - 1)* 50f) + 120f;
        regenRate_b = ((stage - 1)* 20f) + 40f;

        currency_p = 0;
        current_currency = 0;
        saved_currency = 0;
        currency_e = ((stage - 1)* 10)+  10;
        currency_b = ((stage - 1)* 100) + 100;

        currentPotions = ((stage - 1)* 2) + 5;
        regenHealth = ((stage - 1)+ 70) + 250f;

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
        player = GameObject.Find("Player");
        liveCounterText.text = "0" + liveCounter.ToString();
        save_button = "Save";
        levelMusic.GetComponent<AudioSource>();
        levelMusic.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < currentShopList.Count; i++)
            {
                if (!test)
                {
                    if (currentShopList[i].itemName == "Health Potion")
                    {
                        currentPotions += 1;
                        potionText.text = currentPotions.ToString();
                        currency_p -= currentShopList[i].price;
                    }
                    else
                    {
                        if (currentShopList[0].itemName != "Health Potion")
                        {
                            inventory[i].AddComponent<Image>().sprite = currentShopList[i].icon;
                            currency_p -= currentShopList[i].price;
                        }
                        else
                        {
                            inventory[i-1].AddComponent<Image>().sprite = currentShopList[i].icon;
                            currency_p -= currentShopList[i].price;
                        }
                       

                    }
                }

            }
            currencyText.text = currency_p.ToString();
            test = true;
            
        }
           
        if (SceneManager.GetActiveScene().name == "NewGame")
        {
            stagetext.text = "0" + stage.ToString();
        }
        if (Input.GetButtonDown("Save"))
        {

            if (!spawnMessageOnce)
            {
                InstantiatedsaveMessage = Instantiate(saveProgressText, transform.position, Quaternion.identity) as GameObject;
                SaveSystem.SavePlayer(this);
                spawnMessageOnce = true;
                StartCoroutine(destroyMessage());
            }


        }

        //if boss have been defeated and the player has interacted with the portal,
        //re-instantiate RoomController to generate new map
        if (rebuild)
        {
            var bossMusic = GameObject.Find("Player");
            audioSources = bossMusic.GetComponents<AudioSource>();
            audioSources[3].Pause();
            levelMusic.Play();
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
            SaveSystem.SavePlayer(this);
        }

        if (currentExp >= totalExp)
        {
            if (!spawnLevelupOnce)
            {
                InstantiatedLevelupMessage = Instantiate(levelupText, transform.position, Quaternion.identity) as GameObject;
                spawnLevelupOnce = true;
                Invoke("destroyLevelupMessage", 2f);
            }
            if (stage != 1)
            {
                currentExp -= totalExp;
                totalExp += ((stage - 1) * 50);
                initialHealth_p += ((stage - 1)* 100f);
                totalStamina_p += ((stage - 1)* 2f);
                attackDamage_p += ((stage - 1)* 20f);
                stamina_p += ((stage - 1)* 4f);
                regenRate_p += ((stage - 1)* 20f); 
            }
          
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

    IEnumerator destroyMessage()
    {
        yield return new WaitForSeconds(2f);
        Destroy(InstantiatedsaveMessage);
        spawnMessageOnce = false;
    }

    private void destroyLevelupMessage()
    {
        Destroy(InstantiatedLevelupMessage);
        
    }

   

}
