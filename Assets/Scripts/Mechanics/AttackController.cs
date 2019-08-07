using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls the Attack of Player, Enemy and Boss, also accounts for stamina
public class AttackController : MonoBehaviour
{
    public Transform attackPos;
    public LayerMask designatedEnemy;
    public Color fullStaminaColor = Color.green;
    public Color zeroStaminaColor = Color.gray;
    public Slider slider;
    public Image FillImage; 

    private float stamina,regenRate;
    private float attackDamage;
    private float attackRange, totalStamina, currentStamina;
    private string attack_button;
    private float StaminaRegenTimer = 0.0f;
    private const float StaminaTimeToRegen = 3.0f;

    private GameObject gameManager;
    private GameManager gm;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
        if (gameObject.tag == "enemy")
        {
            attackDamage = gm.attackDamage_e;
            stamina = gm.stamina_e;
            totalStamina = gm.totalStamina_e; 
            regenRate = gm.regenRate_e;
        }
        else if (gameObject.tag == "boss")
        {
            attackDamage = gm.attackDamage_b;
            stamina = gm.stamina_b;
            totalStamina = gm.totalStamina_b;
            regenRate = gm.regenRate_b;
        }
        else if (gameObject.tag == "Player")
        {
            attackDamage = gm.attackDamage_p;
            stamina = gm.stamina_p;
            totalStamina = gm.totalStamina_p;
            regenRate = gm.regenRate_p;
        }
    }
    void Start()
    {
        attack_button = "Attack";
        currentStamina = totalStamina;
    }
    // Update is called once per frame
    void Update()
    {
        var testHealth = gameObject.GetComponent<HealthController>();
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gameObject.tag == "Player")
            {
                testHealth.TakeDamage(100f);
            }
            
        }

        if (Input.GetButtonDown(attack_button))
        {
            currentStamina -= stamina;
            SetStaminaUI();
            StaminaRegenTimer = 0.0f;
        }
        else
        {
            if (StaminaRegenTimer >= StaminaTimeToRegen)
            {
                currentStamina = Mathf.Clamp(currentStamina + (regenRate * Time.deltaTime), 0.0f, totalStamina);
                SetStaminaUI();
            }

            else
            {
                StaminaRegenTimer += Time.deltaTime;
            }
        }

        // Can only attack with sufficient stamina else there is a cooldown
        if (currentStamina != 0)
        {
            Collider2D[] attackEnemy = Physics2D.OverlapCircleAll(attackPos.position, attackRange, designatedEnemy);
            for (int i = 0; i < attackEnemy.Length; i++)
            {
                if (Input.GetButtonDown(attack_button))
                {
                    attackEnemy[i].GetComponent<HealthController>().TakeDamage(attackDamage);
                    currentStamina -= stamina;
                    SetStaminaUI();
                }
                else if (gameObject.tag == "enemy" || gameObject.tag == "boss")
                {
                    attackEnemy[i].GetComponent<HealthController>().TakeDamage(attackDamage);
                    currentStamina -= stamina;
                    SetStaminaUI();
                    StaminaRegenTimer = 0.0f;
                }
            }
        }
        else
        {
            if (StaminaRegenTimer >= StaminaTimeToRegen)
            {
                currentStamina = Mathf.Clamp(currentStamina + (regenRate * Time.deltaTime), 0.0f, totalStamina);
                SetStaminaUI();
            }

            else
            {
                StaminaRegenTimer += Time.deltaTime;
            }
        }
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
