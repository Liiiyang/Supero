using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBehaviour : StateMachineBehaviour
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
    private GameObject[] roomList;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        roomList = GameObject.FindGameObjectsWithTag("Room");
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        Transform animatorPos = animator.transform;
        GameObject moveSpotGO = new GameObject();
        moveSpot = moveSpotGO.transform;
        foreach (GameObject room in roomList)
        {
            Room roomScript = room.GetComponent<Room>();
            minX = roomScript.leftDoor.transform.position.x;
            maxX = roomScript.rightDoor.transform.position.x;
            minY = roomScript.bottomDoor.transform.position.y;
            maxY = roomScript.topDoor.transform.position.y;
            if (animatorPos.position.x > minX && animatorPos.position.x < maxX && animatorPos.position.y > minY && animatorPos.position.y < maxY)
            {
                moveSpot.position = new Vector2(Random.Range(minX + 5, maxX - 5), Random.Range(minY + 5, maxY - 5));
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((moveSpot.position.x == 0 && moveSpot.position.y == 0) || moveSpot.position.x == null || moveSpot.position.y == null)
        {
            animator.SetBool("isIdle", true);
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