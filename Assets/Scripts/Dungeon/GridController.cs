﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;

    public string name;

    [System.Serializable]
    public struct Grid
    {
        public int columns, rows;
        public float verticalOffset, horizontalOffset;
    }

    public Grid grid;

    public GameObject gridTile;
    public List<Vector2> availablePoints = new List<Vector2>();

    public bool StopSpawning = false;

    void Awake()
    {
        room = GetComponentInParent<Room>();
        grid.columns = room.width - 12;
        grid.rows = room.height - 13;
        StartCoroutine(DelaySpawns());
        
    }

    void Update()
    {
        GameObject rc = GameObject.Find("RoomController");
        if (GetComponentInParent<ObjectGenerator>().spawning && !StopSpawning)
        {
            Debug.Log("Spawn Boss");
            if (gameObject.tag == "bossRoom" && rc.GetComponent<RoomController>().isBossRoomCreated)
            {
                GeneratedGrid();
                StopSpawning = true;
            }           
        }
        
    }

    IEnumerator DelaySpawns()
    {
        yield return new WaitForSeconds(2f);
        if (gameObject.tag != "bossRoom")
        {
            GeneratedGrid();
        }

    }

    public void GeneratedGrid()
    {
        grid.verticalOffset += room.transform.localPosition.y;
        grid.horizontalOffset += room.transform.localPosition.x;

        for (int y = 0; y < grid.rows; y++)
        {
            for(int x=0; x<grid.columns;x++)
            {
                GameObject go = Instantiate(gridTile,transform);
                go.GetComponent<Transform>().position = new Vector2(x - (grid.columns - grid.horizontalOffset), y - (grid.rows - grid.verticalOffset));
                go.name = "X: " + x + ", Y: " + y;
                availablePoints.Add(go.transform.position);
                //Deactivate grid map, defines objects spawn space
                go.SetActive(false);
            }
        }

        GetComponentInParent<ObjectGenerator>().IntializeSpawning();
        
    }
}
