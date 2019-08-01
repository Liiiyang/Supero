using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the Attack of Player, Enemy and Boss, also accounts for stamina
public class AttackController : MonoBehaviour
{
    public float attackDamage;

    public Transform attackPos;
    public LayerMask designatedEnemy;
    public float attackRange;

    private float stamina;
    public float totalStamina;
    private float cooldown;
    void Awake()
    {
        if (gameObject.tag == "enemy")
        {
            attackDamage = 5f;
            stamina = 5f;
            totalStamina = 5f; 
            cooldown = 0.01f;
        }
        else if (gameObject.tag == "boss")
        {
            attackDamage = 50f;
            stamina = 50f;
            totalStamina = 50f;
            cooldown = 0.1f;
        }
        else if (gameObject.tag == "Player")
        {
            attackDamage = 50f;
            stamina = 10f;
            totalStamina = 70f;
            cooldown = 5f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        var testHealth = gameObject.GetComponent<HealthController>();
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gameObject.tag == "Player")
            {
                testHealth.TakeDamage(70f);
            }
            
        }

        // Can only attack with sufficient stamina else there is a cooldown
        if (stamina < totalStamina)
        {
            Collider2D[] attackEnemy = Physics2D.OverlapCircleAll(attackPos.position, attackRange, designatedEnemy);
            for (int i = 0; i < attackEnemy.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    attackEnemy[i].GetComponent<HealthController>().TakeDamage(attackDamage);
                    totalStamina -= stamina;
                }
                else if (gameObject.tag == "enemy" || gameObject.tag == "boss")
                {
                    attackEnemy[i].GetComponent<HealthController>().TakeDamage(attackDamage);
                    totalStamina -= stamina;
                }
            }
        }
        else
        {
            totalStamina += cooldown;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
