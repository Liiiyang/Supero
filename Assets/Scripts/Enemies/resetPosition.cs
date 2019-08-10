using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Resets enemy position
public class resetPosition : MonoBehaviour
{
    private GameObject player;
    private HealthController hc;
    private Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        //originalPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        //Debug.Log("Enemy Position: " + gameObject.transform.position.x.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");
        hc = player.GetComponent<HealthController>();

        if (hc.playerisDead)
        {
            //gameObject.transform.position = originalPos;
            gameObject.GetComponent<Animator>().enabled = true;

            
        }
        
    }
}
