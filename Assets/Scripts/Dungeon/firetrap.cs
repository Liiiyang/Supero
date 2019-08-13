using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firetrap : MonoBehaviour
{
    public AudioSource firetrapSound;
    private Collider2D fireCollision;
    //private AudioSource fireSound;
    private float flameDamage;

    // Start is called before the first frame update
    void Start()
    {
        fireCollision = this.GetComponent<Collider2D>();
        //fireSound = this.GetComponent<AudioSource>();
        flameDamage = 50f;
        firetrapSound.Play();
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Who is hit: " + other.gameObject.name);
        if (other.gameObject.tag == "enemy" || other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HealthController>().TakeDamage(flameDamage);
            
        }
       
    }
}
