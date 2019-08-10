using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb2d;
    private Vector2 moveVelocity;

    private bool facingRight;


    void Start()
    {
        facingRight = true;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        moveVelocity = movement.normalized * speed;

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb2d.MovePosition(rb2d.position + moveVelocity * Time.fixedDeltaTime);
        Flip(moveHorizontal);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        
    }

    private void Flip(float horizontal)
    {
        if (horizontal>0 && !facingRight || horizontal<0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }



    
}
