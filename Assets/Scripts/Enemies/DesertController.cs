using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertController : MonoBehaviour
{
    private bool facingLeft;
    private float horizontal;
    private Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        facingLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = transform.position - lastPosition;
        Flip(velocity.x);
        Debug.Log(velocity.x);
        lastPosition = transform.position;
    }
    

    private void Flip(float horizontal)
    {
        if (horizontal < 0 && !facingLeft || horizontal>0 && facingLeft)
        {
            facingLeft = !facingLeft;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
