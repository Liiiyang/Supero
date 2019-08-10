using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_08_Projectile : MonoBehaviour
{
    public GameObject projectile;
    private Transform player;
    // time component for projectile
    private float timeBtwShots;
    public float startTimeBtwShots;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }


    void Update()
    {
        if (timeBtwShots < 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}