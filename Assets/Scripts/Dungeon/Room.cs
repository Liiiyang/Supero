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

    public GameObject leftDoorh, rightDoorh, topDoorh, bottomDoorh;

    private GameObject leftRoom, rightRoom,topRoom,bottomRoom;

    public Room(int x1, int y1)
    {
        x = x1;
        y = y1;
    }

    public List<Door> doors = new List<Door>();
    // Start is called before the first frame update

    void Awake()
    {
        leftDoorh.SetActive(false);
        rightDoorh.SetActive(false);
        topDoorh.SetActive(false);
        bottomDoorh.SetActive(false);
    }
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


        if (name.Contains("Hidden") && !updatedDoors)
        {
            hiddenDoors();
            RemoveUnconnectedDoors();
            updatedDoors = true;


        }

        StartCoroutine(BossRemoveDoor());
        //StartCoroutine(ShopRemoveDoor());

        //Debug.Log(gameObject.name.ToString() + "Leftwall x: " + leftwall.transform.position.x.ToString() + " Leftwall y: " + leftwall.transform.position.y.ToString());
        //Debug.Log(leftwall.transform.position.x.GetType());

        

        
    }

    IEnumerator ShopRemoveDoor()
    {
        yield return new WaitForSeconds(0.5f);
        if (name.Contains("Shop") && !updatedDoors)
        {
            //RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    IEnumerator BossRemoveDoor()
    {
        yield return new WaitForSeconds(0.5f);
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

    private void hiddenDoors()
    {
        for (int i = 0; i < RoomController.instance.roomList.Count; i++)
        {
            if(!RoomController.instance.roomList[i].name.Contains("Boss"))
            {
                if (((x + 1 == RoomController.instance.roomList[i].x) && (y == RoomController.instance.roomList[i].y)))
                {
                    rightRoom = GameObject.Find(RoomController.instance.roomList[i].name);
                    Debug.Log(RoomController.instance.roomList[i].name);
                    rightRoom.GetComponent<Room>().leftDoorh.SetActive(true);
                    rightRoom.transform.Find("leftDoorh").gameObject.GetComponent<Collider2D>().isTrigger = false;

                }
                else if (((x - 1 == RoomController.instance.roomList[i].x) && (y == RoomController.instance.roomList[i].y))
    )
                {
                    leftRoom = GameObject.Find(RoomController.instance.roomList[i].name);
                    Debug.Log(RoomController.instance.roomList[i].name);
                    leftRoom.GetComponent<Room>().rightDoorh.SetActive(true); ;
                    leftRoom.transform.Find("rightDoorh").gameObject.GetComponent<Collider2D>().isTrigger = false;

                }
                else if (((x == RoomController.instance.roomList[i].x) && (y + 1 == RoomController.instance.roomList[i].y))
    )
                {
                    topRoom = GameObject.Find(RoomController.instance.roomList[i].name);
                    Debug.Log(RoomController.instance.roomList[i].name);
                    topRoom.GetComponent<Room>().bottomDoorh.SetActive(true);
                    topRoom.transform.Find("bottomDoorh").gameObject.GetComponent<Collider2D>().isTrigger = false;

                }
                else if (((x == RoomController.instance.roomList[i].x) && (y - 1 == RoomController.instance.roomList[i].y))
    )
                {
                    bottomRoom = GameObject.Find(RoomController.instance.roomList[i].name);
                    Debug.Log(RoomController.instance.roomList[i].name);
                    bottomRoom.GetComponent<Room>().topDoorh.SetActive(true);
                    bottomRoom.transform.Find("topDoorh").gameObject.GetComponent<Collider2D>().isTrigger = false;

                }
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
