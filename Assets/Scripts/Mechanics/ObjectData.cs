using UnityEngine;

[CreateAssetMenu(fileName = "Object.asset", menuName = "Objects/Object")]
public class ObjectData : ScriptableObject
{
    public GameObject objectsToSpawn;

    public int minSpawn;

    public int maxSpawn;
}
