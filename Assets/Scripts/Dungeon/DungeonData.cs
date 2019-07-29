using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData.asset", menuName = "DungeonData/Dungeon Data")]
public class DungeonData : ScriptableObject
{
    public int numberofCrawlers, iterationMin, iterationMax;
}
