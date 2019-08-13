using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_08_Projectile : MonoBehaviour
{
    public GameObject projectile, playerGO, bossRoom;
    private Transform player;
    // time component for projectile
    private float timeBtwShots;
    public float startTimeBtwShots;
    private Vector2 position;
    private HealthController hc;
    private Room roomScript;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }


    void Update()
    {
        bossRoom = GameObject.FindGameObjectWithTag("bossRoom");
        roomScript = bossRoom.GetComponent<Room>();
        minX = roomScript.leftwall.transform.position.x;
        maxX = roomScript.rightwall.transform.position.x;
        minY = roomScript.bottomwall.transform.position.y;
        maxY = roomScript.topwall.transform.position.y;
        if (player.position.x > minX && player.position.x < maxX && player.position.y > minY && player.position.y < maxY)
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
    }
}