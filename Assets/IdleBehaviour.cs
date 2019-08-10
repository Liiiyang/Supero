using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    public float startWaitTime;
    private float waitTime;
    private float distanceToPlayer;
    private float angle;
    private Transform playerPosition;
    public float rangeToPlayer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isIdle", true);
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        waitTime = startWaitTime;
        Physics2D.queriesStartInColliders = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (waitTime <= 0)
        {
            animator.SetBool("isIdle", false);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }

        distanceToPlayer = Vector3.Distance((Vector2)animator.transform.position, (Vector2)playerPosition.transform.position);
        angle = Vector2.Angle(animator.transform.right, playerPosition.position - animator.transform.position);
        
        if (animator.transform.localScale.x < 0)
        {
            RaycastHit2D hitinfo = Physics2D.Raycast(animator.transform.position, animator.transform.right, rangeToPlayer);
            if (hitinfo.collider != null)
            {
                Debug.DrawLine(animator.transform.position, hitinfo.point, Color.red);
                if (hitinfo.collider.CompareTag("Player"))
                {
                    animator.SetBool("isFollowing", true);
                }
            }
        }
        else
        {
            RaycastHit2D hitinfo = Physics2D.Raycast(animator.transform.position, -animator.transform.right, rangeToPlayer);
            if (hitinfo.collider != null)
            {
                Debug.DrawLine(animator.transform.position, hitinfo.point, Color.red);
                if (hitinfo.collider.CompareTag("Player"))
                {
                    animator.SetBool("isFollowing", true);
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
