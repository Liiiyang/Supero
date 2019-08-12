using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInformation
{
    public string name;
    public int x, y;
}

public class RoomController : MonoBehaviour
{

    public static RoomController instance;
    public List<Room> roomList = new List<Room>();
    public bool clearList = false;
    public bool spawnBossHiddenRoom = false;
    public bool isBossRoomCreated = false;

    private string currentRoomName = "Room";

    Room currentRoom;

    RoomInformation currentLoadRoomData;

    Queue<RoomInformation> roomQueue = new Queue<RoomInformation>();

    

    bool isLoadingRoom = false;

    bool createBossRoom = false;

    bool createShopRoom = false;

    bool createHiddenRoom = false;

    bool updatedRoom = false;

    bool finishedQueueing = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
 
            //LoadRoom("One", 0, 0);
            //LoadRoom("Two", 1, 0);
            //LoadRoom("Two", -1, 0);
            //LoadRoom("Two", 0, 1);
            //LoadRoom("Two", 0, -1);
    }

    void Update()
    {
        UpdateRoomQueue();

    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if (roomQueue.Count == 0)
        {
            if (!createBossRoom)
            {
                StartCoroutine(CreateBossRoom());
                
            }
            else if (!createHiddenRoom)
            {
                StartCoroutine(CreateHiddenRoom());
            }
            else if (createBossRoom && createHiddenRoom && !updatedRoom)
            {
                foreach (Room room in roomList)
                {
                    room.RemoveUnconnectedDoors();
                }
                updatedRoom = true;
            }
            return;
        }

        currentLoadRoomData = roomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator CreateBossRoom()
    {
        createBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if(roomQueue.Count == 0 || finishedQueueing)
        {
            Room bossRoom = roomList[roomList.Count - 1];
            if((bossRoom.x>1 ||bossRoom.x<-1) || (bossRoom.y>1 || bossRoom.y<-1))
            {
                Debug.Log("Boss Room");
                Room tempRoom = new Room(bossRoom.x, bossRoom.y);
                Destroy(bossRoom.gameObject);
                var roomToRemove = roomList.Single( r => r.x  == tempRoom.x && r.y == tempRoom.y);
                roomList.Remove(roomToRemove);
                LoadRoom("Boss", tempRoom.x, tempRoom.y);
                isBossRoomCreated = true;
            }
        }
    }
    IEnumerator CreateHiddenRoom()
    {
        createHiddenRoom = true;
        yield return new WaitForSeconds(3f);
        if(roomQueue.Count == 0 && isBossRoomCreated)
        {
            Room hiddenRoom = roomList[roomList.Count - 2];
            if((hiddenRoom.x<2 ||hiddenRoom.x>-2) && (hiddenRoom.y<2 || hiddenRoom.y>-2))
            {
                Room tempRoom = new Room(hiddenRoom.x, hiddenRoom.y);
                Destroy(hiddenRoom.gameObject);
                var roomToRemove = roomList.Single( r => r.x  == tempRoom.x && r.y == tempRoom.y);
                roomList.Remove(roomToRemove);
                LoadRoom("Hidden", tempRoom.x, tempRoom.y);
            }
        }
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (checkRoom(x, y))
        {
            return;
        }
        RoomInformation newRoom = new RoomInformation();
        newRoom.name = name;
        newRoom.x = x;
        newRoom.y = y;

        roomQueue.Enqueue(newRoom);

        finishedQueueing = true;


    }

    IEnumerator LoadRoomRoutine(RoomInformation info)
    {
        string roomName = currentRoomName + info.name;
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
       
        if(!checkRoom(currentLoadRoomData.x, currentLoadRoomData.y))
        {
            room.transform.position = new Vector3(
            currentLoadRoomData.x * room.width,
            currentLoadRoomData.y * room.height,
            0
            );

            room.x = currentLoadRoomData.x;
            room.y = currentLoadRoomData.y;
            room.name = currentRoom + "-" + currentLoadRoomData.name + " " + room.x + ", " + room.y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (roomList.Count == 0)
            {
                CameraController.instance.currentRoom = room;
            }

            roomList.Add(room);  
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool checkRoom(int x, int y)
    {
        return roomList.Find( item => item.x == x && item.y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return roomList.Find( item => item.x == x && item.y == y);
    }

    public void onPlayerEnterRoom(Room room)
    {
        CameraController.instance.currentRoom = room;
        currentRoom = room;
    }
}

