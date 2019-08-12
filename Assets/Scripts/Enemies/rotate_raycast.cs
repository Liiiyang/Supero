using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_raycast : MonoBehaviour
{
    // rotating raycast
    public float rotationSpeed;
    public float distance;
    public bool foundPlayer = false;

    // projectile
    public GameObject projectile;
    private Transform player;
    private float timeBtwShots;
    public float startTimeBtwShots;
    private Vector2 position;


    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (foundPlayer)
        {
            if (timeBtwShots < 0)
            {
                position = new Vector2(transform.position.x, transform.position.y + 1);
                Instantiate(projectile, position, Quaternion.identity);
                timeBtwShots = startTimeBtwShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }
        }
        else {
            
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance);

            if (hitInfo.collider != null)
            {
                Debug.DrawLine(transform.position, hitInfo.point, Color.red);
                if (hitInfo.collider.CompareTag("Player"))
                {
                    foundPlayer = true;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + transform.right* distance, Color.green);
            }
        }
    }
}
