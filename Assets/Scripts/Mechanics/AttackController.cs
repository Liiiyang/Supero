using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls the Attack of Player, Enemy and Boss, also accounts for stamina
public class AttackController : MonoBehaviour
{
    public float attackDamage;

    public Transform attackPos;
    public LayerMask designatedEnemy;
    public float attackRange,totalStamina,currentStamina;

    public Color fullStaminaColor = Color.green;
    public Color zeroStaminaColor = Color.gray;
    public Slider slider;
    public Image FillImage; 

    private float stamina,regenRate;
    private string attack_button;

    private float StaminaRegenTimer = 0.0f;
    private const float StaminaTimeToRegen = 3.0f;

    void Awake()
    {
        if (gameObject.tag == "enemy")
        {
            attackDamage = 5f;
            stamina = 5f;
            totalStamina = 5f; 
            regenRate = 0.01f;
        }
        else if (gameObject.tag == "boss")
        {
            attackDamage = 0.01f;
            stamina = 50f;
            totalStamina = 50f;
            regenRate = 0.1f;
        }
        else if (gameObject.tag == "Player")
        {
            attackDamage = 50f;
            stamina = 40f;
            totalStamina = 100f;
            regenRate = 10f;
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
