using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int width, height, x, y;
    public Door leftDoor, rightDoor, topDoor, bottomDoor;

    private bool updatedDoors = false;

    public bool destroySpawns = false;

    public Collider2D col_bottom;
    public Collider2D col_left;
    public Collider2D col_up;
    public Collider2D col_right;

    public GameObject leftwall, rightwall, topwall, bottomwall;

    public Room(int x1, int y1)
    {
        x = x1;
        y = y1;
    }

    public List<Door> doors = new List<Door>();
    // Start is called before the first frame update
    void Start()
    {
        if (RoomController.instance == null)
        {
            Debug.Log("Please Play Main Scene!");
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>();

        foreach (Door d in ds)
        {
            doors.Add(d);
            switch (d.doorType)
            {
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.right:
                    rightDoor = d;
                    break;
                case Door.DoorType.top:
                    topDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;
            }

        }
        RoomController.instance.RegisterRoom(this);   
    }

    void Update()
    {
        

        if (name.Contains("Shop") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }

        StartCoroutine(BossRemoveDoor());

        //Debug.Log(gameObject.name.ToString() + "Leftwall x: " + leftwall.transform.position.x.ToString() + " Leftwall y: " + leftwall.transform.position.y.ToString());
        //Debug.Log(leftwall.transform.position.x.GetType());

        

        
    }

    IEnumerator BossRemoveDoor()
    {
        yield return new WaitForSeconds(1);
        if (name.Contains("Boss") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    public void RemoveUnconnectedDoors()
    {
        foreach (Door door in doors)
        {
            switch (door.doorType)
            {
                case Door.DoorType.left:
                    if (GetLeft() == null)
                    {
                        door.gameObject.SetActive(false);
                        col_left.isTrigger = false;
                    }
                    break;
                case Door.DoorType.right:
                    if (GetRight() == null)
                    {
                        door.gameObject.SetActive(false);
                        col_right.isTrigger = false;
                    }
                    break;
                case Door.DoorType.top:
                    if (GetTop() == null)
                    {
                        door.gameObject.SetActive(false);
                        col_up.isTrigger = false;
                    }
                    break;
                case Door.DoorType.bottom:
                    if (GetBottom() == null)
                    {
                        door.gameObject.SetActive(false);
                        col_bottom.isTrigger = false;
                    }
                    break;
            }
        }
    }

    public Room GetLeft()
    {
        if (RoomController.instance.checkRoom(x - 1, y))
        {
            return RoomController.instance.FindRoom(x - 1, y);
        }
        return null;
    }

    public Room GetRight()
    {
        if (RoomController.instance.checkRoom(x + 1, y))
        {
            return RoomController.instance.FindRoom(x + 1, y);
        }
        return null;
    }

    public Room GetTop()
    {
        if (RoomController.instance.checkRoom(x, y+1))
        {
            return RoomController.instance.FindRoom(x, y+1);
        }
        return null;
    }

    public Room GetBottom()
    {
        if (RoomController.instance.checkRoom(x, y-1))
        {
            return RoomController.instance.FindRoom(x, y-1);
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    public Vector3 GetRoomCentre()
    {
        return new Vector3(x * width, y * height);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RoomController.instance.onPlayerEnterRoom(this);
        }
    }
}
