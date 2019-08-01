using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Room currentRoom;

    public Room startRoom;

    public float moveSpeedWhenRoomChange;

    Vector3 targetPos;

    private bool initialize = false;

    private bool isDead = false;

    void Awake()
    {
        instance = this;
       
    }

    void Update()
    {
        UpdatePosition();
        

    }

    void UpdatePosition()
    {


        if (currentRoom == null)
        {
            return;
        }


        Vector3 targetPosition = GetCameraTargetPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeedWhenRoomChange);
    }

    Vector3 GetCameraTargetPosition()
    {
        if (initialize == false)
        {
            startRoom = currentRoom;
            initialize = true;
        }

        if (currentRoom == null)
        {
            return Vector3.zero;
        }

        reset(isDead);
        targetPos = currentRoom.GetRoomCentre();
        targetPos.z = transform.position.z;
        return targetPos;
    }

    public void reset(bool isDead)
    {
        if (isDead == true)
        {
            currentRoom = startRoom;
        }
        
    }

    public bool isSwtichScene()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
