using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_05_Projectile : MonoBehaviour
{
    public GameObject projectile;
    private Transform player;
    // time component for projectile
    private float timeBtwShots;
    public float startTimeBtwShots;
    private Vector2 position;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }


    void Update()
    {
        if (timeBtwShots < 0)
        {
            position = new Vector2(transform.position.x, transform.position.y+1);
            Instantiate(projectile, position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}