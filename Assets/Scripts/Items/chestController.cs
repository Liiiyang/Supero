using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Controls the opening of chest
public class chestController : MonoBehaviour
{
    public Transform chest_center;
    public Vector3 chest_size;
    public LayerMask p;
    //public TextMeshProUGUI gainedText;
    public GameObject canvas;
    public AudioSource coins;

    private Animator chestAnimation;
    private GameObject gameManager;
    private GameManager gm;

    private string action_button;
    private GameObject message;
    private int gained, test;
    private bool once = false;

    void Start()
    {
        action_button = "Action";
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(action_button))
        {
            

            Collider2D[] player = Physics2D.OverlapBoxAll(chest_center.position, chest_size, p);
            for (int i = 0; i < player.Length; i++)
            {
                if (player[i].name == "Player")
                {
                    gameObject.GetComponent<Animator>().Play("chest");
                    coins.Play();
                    //Instantiate an item here
                    
                    gained = Random.Range(100, 500);
                    //test = gained;
                    //Debug.Log("Gained: " + gained.ToString());
                    gm.currency_p += gained;
                    if (!once)
                    {
                        message = Instantiate(canvas, transform.position, Quaternion.identity) as GameObject;
                        message.transform.Find("gained").GetComponent<TextMeshProUGUI>().text = "Gained " + gained + " Currency";
                        once = true;
                        StartCoroutine(destroyMessage());
                    }


                }
                
            }
        }

    }

    IEnumerator destroyMessage()
    {
        yield return new WaitForSeconds(2f);
        Destroy(message);
        once = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(chest_center.position, chest_size);
    }
}
