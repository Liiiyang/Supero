using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the opening of chest
public class chestController : MonoBehaviour
{
    public Transform chest_center;
    public Vector3 chest_size;
    public LayerMask p;

    private Animator chestAnimation;

    private string action_button;

    void Start()
    {
        action_button = "Action";
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(action_button))
        {

            Collider2D[] player = Physics2D.OverlapBoxAll(chest_center.position, chest_size, p);
            for (int i = 0; i < player.Length; i++)
            {
                if (player[i].name == "Player")
                {
                    gameObject.GetComponent<Animator>().Play("chest");
                    //Instantiate an item here
                }
                
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(chest_center.position, chest_size);
    }
}
