using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    MenuButtonController menuButtonController;
    [SerializeField]
    Animator animator;
    [SerializeField]
    AnimatorFunctions animatorFunctions;
    [SerializeField]
    int thisIndex;

    private GameObject playerUI;
    private static MenuButton instance = null;
    private int stage;
    private float initialHealth;

    // Update is called once per frame
    void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1)
            {
                animator.SetBool("pressed", true);
            }
            else if (animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;

                playerUI = GameObject.Find("playerRest");
                playerUI.GetComponent<Animator>().SetBool("startPressed", true);
                if (gameObject.name == "Quit")
                {
                    Application.Quit();
                }
                StartCoroutine(startGame());
                

            }
        }
        else
        {
            animator.SetBool("selected", false);
        }

        
    }

    IEnumerator startGame()
    {
        yield return new WaitForSeconds(3f);
        if (gameObject.name == "NewGame")
        {
            Destroy(GameObject.Find("MainMenu"));
            SceneManager.LoadScene("NewGame");

        }
        else if (gameObject.name == "Tutorial")
        {
            Destroy(GameObject.Find("MainMenu"));
            SceneManager.LoadScene("Tutorial");

        }
        else if (gameObject.name == "Continue")
        {
            Destroy(GameObject.Find("MainMenu"));
            StaticSaveFile.save = "save";
            SceneManager.LoadScene("NewGame");
        }
       

    }

}
