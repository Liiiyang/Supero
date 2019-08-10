using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Room currentRoom;

    public float moveSpeedWhenRoomChange;


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
        if (currentRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPosition = currentRoom.GetRoomCentre();
        targetPosition.z = transform.position.z;

        return targetPosition;
    }

    public bool isSwtichScene()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
