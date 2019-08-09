using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Controls the Attack of Player, Enemy and Boss, also accounts for stamina
public class AttackController : MonoBehaviour
{
    public Transform attackPos;
    public TextMeshProUGUI floatingText;
    public float attackRange;
    public LayerMask designatedEnemy;
    public Color fullStaminaColor = Color.green;
    public Color zeroStaminaColor = Color.gray;
    public Slider slider;
    public Image FillImage; 

    private float stamina,regenRate;
    private float attackDamage;
    private float totalStamina, currentStamina;
    private string attack_button;
    private float StaminaRegenTimer = 0.0f;
    private const float StaminaTimeToRegen = 3.0f;

    private GameObject gameManager;
    private GameManager gm;
    private RectTransform rect;

    private float timebtwAttack;
    private float startTimebtwAttack = 0.8f;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
        rect = floatingText.GetComponent<RectTransform>();
        if (gameObject.tag == "enemy")
        {
            attackDamage = gm.attackDamage_e;
            stamina = gm.stamina_e;
            totalStamina = gm.totalStamina_e;
            slider.maxValue = totalStamina;
            regenRate = gm.regenRate_e;
            currentStamina = gm.currentStamina_e;
        }
        else if (gameObject.tag == "boss")
        {
            attackDamage = gm.attackDamage_b;
            stamina = gm.stamina_b;
            totalStamina = gm.totalStamina_b;
            slider.maxValue = totalStamina;
            regenRate = gm.regenRate_b;
            currentStamina = gm.currentStamina_e;
        }
        else if (gameObject.tag == "Player")
        {
            attackDamage = gm.attackDamage_p;
            stamina = gm.stamina_p;
            totalStamina = gm.totalStamina_p;
            slider.maxValue = totalStamina;
            regenRate = gm.regenRate_p;
            currentStamina = gm.currentStamina_p;
        }
    }
    void Start()
    {
        attack_button = "Attack";
    }
    // Update is called once per frame
    void Update()
    {
        var testHealth = gameObject.GetComponent<HealthController>();
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gameObject.tag == "Player")
            {
                testHealth.TakeDamage(10f);
            }
            
        }

        //Player are free to use their attack and have regen
        if (gameObject.tag == "Player")
        {
            if (Input.GetButtonDown(attack_button))
            {
                currentStamina -= gm.stamina_p;
                gm.currentStamina_p = currentStamina;
                SetStaminaUI();
                StaminaRegenTimer = 0.0f;
            }
            else
            {
                if (StaminaRegenTimer >= StaminaTimeToRegen)
                {
                    currentStamina = Mathf.Clamp(currentStamina + (regenRate * Time.deltaTime), 0.0f, totalStamina);
                    gm.currentStamina_p = currentStamina;
                    SetStaminaUI();
                }

                else
                {
                    StaminaRegenTimer += Time.deltaTime;
                }
            }

            if (gm.currentHealth_p == 0)
            {
                currentStamina = gm.totalStamina_p;
            }
        }

        //Player can only attack if he have enough stamina
        if (gm.currentStamina_p > 0)
        {
            Debug.Log("Current Stamina: " + gm.currentStamina_p.ToString());
            Collider2D[] attackEnemy = Physics2D.OverlapCircleAll(attackPos.position, attackRange, designatedEnemy);
            for (int i = 0; i < attackEnemy.Length; i++)
            {
                if (Input.GetButtonDown(attack_button))
                {
                    StartCoroutine(showFloatingText(attackEnemy[i], attackDamage));
                    attackEnemy[i].GetComponent<HealthController>().TakeDamage(attackDamage);
                    currentStamina -= stamina;
                    SetStaminaUI();
                }
            }
        }

        //Enemy can only attack if they have enough stamina
        if (gm.currentStamina_e > 0 && timebtwAttack <= 0)
        {
            Debug.Log("Current Stamina: " + gm.currentStamina_e.ToString());
            Collider2D[] attackEnemy = Physics2D.OverlapCircleAll(attackPos.position, attackRange, designatedEnemy);
            for (int i = 0; i < attackEnemy.Length; i++)
            {
                if (gameObject.tag == "enemy")
                {
                    // Can only attack with sufficient stamina else there is a cooldown
                    if (gameObject.GetComponent<Animator>().GetBool("isAttacking"))
                    {
                        attackEnemy[i].GetComponent<HealthController>().TakeDamage(attackDamage);
                    }
                    currentStamina -= stamina;
                    gm.currentStamina_e = currentStamina;
                    SetStaminaUI();
                    StaminaRegenTimer = 0.0f;
                }
            }
            timebtwAttack = startTimebtwAttack;
        }
        else
        {
            timebtwAttack -= Time.deltaTime;
        }

        //Stamina Regen for enemy
        if (gameObject.tag == "enemy")
        {
            if (StaminaRegenTimer >= StaminaTimeToRegen)
            {
                currentStamina = Mathf.Clamp(currentStamina + (regenRate * Time.deltaTime), 0.0f, totalStamina);
                gm.currentStamina_e = currentStamina;
                SetStaminaUI();
            }

            else
            {
                StaminaRegenTimer += Time.deltaTime;
            }
        } 

        // boss can only attack if they have enough stamina
        if (gm.currentStamina_b > 0)
        {
            Debug.Log("Current Stamina: " + gm.currentStamina_e.ToString());
            Collider2D[] attackEnemy = Physics2D.OverlapCircleAll(attackPos.position, attackRange, designatedEnemy);
            for (int i = 0; i < attackEnemy.Length; i++)
            {
                if (gameObject.tag == "boss")
                {
                    // Can only attack with sufficient stamina else there is a cooldown
                    attackEnemy[i].GetComponent<HealthController>().TakeDamage(attackDamage);
                    currentStamina -= stamina;
                    gm.currentStamina_b = currentStamina;
                    SetStaminaUI();
                    StaminaRegenTimer = 0.0f;
                }
            }
        }

        //Stamina Regen for boss
        if (gameObject.tag == "boss")
        {
            if (StaminaRegenTimer >= StaminaTimeToRegen)
            {
                currentStamina = Mathf.Clamp(currentStamina + (regenRate * Time.deltaTime), 0.0f, totalStamina);
                gm.currentStamina_b = currentStamina;
                SetStaminaUI();
            }

            else
            {
                StaminaRegenTimer += Time.deltaTime;
            }
        } 
       

    }

    //Show damage points when player hits the enemies
    IEnumerator showFloatingText(Collider2D Enemy, float attackDamage)
    {

        Enemy.GetComponent<AttackController>().floatingText.GetComponent<Animator>().Play("attackPopup",  0, 0f);
        Enemy.GetComponent<AttackController>().floatingText.text = Mathf.RoundToInt(attackDamage).ToString();
        yield return new WaitForSeconds(0.2f);

    }


    private void SetStaminaUI()
    {
        // Adjust the value and colour of the slider.
        slider.value = currentStamina;
        FillImage.color = Color.Lerp(zeroStaminaColor, fullStaminaColor, currentStamina / totalStamina);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
