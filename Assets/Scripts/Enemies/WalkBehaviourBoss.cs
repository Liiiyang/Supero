using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBehaviourBoss : StateMachineBehaviour
{
    private Transform playerPosition;
    private float distanceToPlayer;
    private float angle;
    public float rangeToPlayer;
    public float speed;
    private GameObject[] moveSpotList;
    private Transform moveSpot;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float msDistance;
    private GameObject bossRoom;
    private Room roomScript;
    private PortalController pc;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossRoom = GameObject.FindGameObjectWithTag("bossRoom");
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        //Debug.Log("Player is here: " + playerPosition.position.x.ToString());
        Transform animatorPos = animator.transform;
        GameObject moveSpotGO = new GameObject();
        moveSpot = moveSpotGO.transform;
        roomScript = bossRoom.GetComponent<Room>();
        minX = roomScript.leftwall.transform.position.x;
        //Debug.Log("Left Boss wall: " + roomScript.leftwall.transform.position.x.ToString());
        maxX = roomScript.rightwall.transform.position.x;
        //Debug.Log("Right Boss wall: " + roomScript.rightwall.transform.position.x.ToString());
        minY = roomScript.bottomwall.transform.position.y;
        //Debug.Log("Bottom Boss wall: " + roomScript.bottomwall.transform.position.y.ToString());
        maxY = roomScript.topwall.transform.position.y;
        //Debug.Log("Top Boss wall: " + roomScript.topwall.transform.position.y.ToString());
        //Debug.Log("Boss X movement: " + animatorPos.position.x.ToString());
        if (animatorPos.position.x > minX && animatorPos.position.x < maxX && animatorPos.position.y > minY && animatorPos.position.y < maxY)
        {
            moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        }
        else
        {
            moveSpot.position = new Vector2(Random.Range(maxX, minX), Random.Range(maxY, minY));
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (moveSpot.position.x == 0 && moveSpot.position.y == 0)
        {
            animator.SetBool("isFollowing", true);
        }

        var player = GameObject.Find("Player");
        pc = player.GetComponent<PortalController>();

        if (pc.spawnBoss)
        {
            animator.SetBool("isFollowing", true);
        }

        animator.transform.position = Vector2.MoveTowards(animator.transform.position, moveSpot.position, speed * Time.deltaTime);
        if (Vector2.Distance(animator.transform.position, moveSpot.position) < 0.2f)
        {
            animator.SetBool("isIdle", true);
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