using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnTest : MonoBehaviour
{
    private Room room;

    public GameObject prefab;
    private float minx, maxx;
    private float miny, maxy;
    // Start is called before the first frame update
    void Start()
    {
        room = GetComponent<Room>();
        minx = room.leftwall.transform.position.x;
        maxx = room.rightwall.transform.position.x;
        miny = room.topwall.transform.position.y;
        maxy = room.bottomwall.transform.position.y;
        //Debug.Log(gameObject.name.ToString() + "Leftwall Min x: " + minx.ToString() + " Leftwall Max x: " + maxx.ToString());

        //GameObject test = Instantiate(prefab, new Vector3(Random.Range(minx, maxx), 0, 0), Quaternion.identity) as GameObject;

        GameObject test = Instantiate(prefab, new Vector3(minx, 0, 0), Quaternion.identity) as GameObject;

        GameObject test1 = Instantiate(prefab, new Vector3(maxx, 0, 0), Quaternion.identity) as GameObject;
        GameObject test2 = Instantiate(prefab, new Vector3(0, miny, 0), Quaternion.identity) as GameObject;
        GameObject test3 = Instantiate(prefab, new Vector3(0, maxy, 0), Quaternion.identity) as GameObject;
 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
