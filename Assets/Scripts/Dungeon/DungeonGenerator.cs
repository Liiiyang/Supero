using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonData dungeonData;

    private List<Vector2Int> dungeonRooms;

    private void Start()
    {
        dungeonRooms = DungeonController.GenerateDungeon(dungeonData);
        createRooms(dungeonRooms);       
    }
    private void createRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Start", 0, 0);
        foreach (Vector2Int roomLocation in rooms)
        {
            RoomController.instance.LoadRoom("Normal", roomLocation.x, roomLocation.y);
        }
    }
}
