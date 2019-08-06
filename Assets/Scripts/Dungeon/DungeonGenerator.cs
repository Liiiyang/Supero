using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGenerator : MonoBehaviour
{
    public DungeonData dungeonData;

    public bool spawnNewMap = false;
    private List<Vector2Int> dungeonRooms;
    private float spawnTimer = 0.0f;
    private const float stopTimer = 0.01f;
    
    
    

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
