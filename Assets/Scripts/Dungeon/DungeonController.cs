using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    top = 0,

    left = 1,

    down = 2,

    right = 3
};

public class DungeonController : MonoBehaviour
{
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();

    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction,Vector2Int>
    {
            {Direction.top,Vector2Int.up},
            {Direction.left,Vector2Int.left},
            {Direction.down,Vector2Int.down},
            {Direction.right,Vector2Int.right}
    };

    public static List<Vector2Int> GenerateDungeon(DungeonData dungeonData)
    {
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();

        for(int i=0; i<dungeonData.numberofCrawlers; i++)
        {
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }

        int iteration = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

        for(int i=0; i<iteration;i++)
        {
            foreach(DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                Vector2Int newPosition = dungeonCrawler.Move(directionMovementMap);
                positionsVisited.Add(newPosition);
            }
        }
        return positionsVisited;
    }
}
