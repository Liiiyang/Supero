using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttacks : MonoBehaviour
{
    public Transform attackPos;
    public Vector3 projectile_size;
    public LayerMask p;

    private Animator chestAnimation;

    private float attackDamage = 10f;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Collider2D[] player = Physics2D.OverlapBoxAll(attackPos.position, projectile_size, p);
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i].name == "Player")
            {
                Debug.Log("Projectile Attacks!");
                player[i].GetComponent<HealthController>().TakeDamage(attackDamage);
            }

        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, projectile_size);
    }
}
