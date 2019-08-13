using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public int price = 999;
    public float damage = 1f;
    public float addHealth = 1f;
    public float healHealth = 1f;
}

public class TestShopScrollList : MonoBehaviour
{
    public List<GameObject> gameobjectShopList;
    public List<Item> currentShopList;
    public Transform contentPanel;
    public Text myGoldDisplay;
    public GameObject prefab;
    public float gold = 20f;
    public TestShopScrollList otherShop;
    private GameObject gameManager;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
        if (gameObject.tag == "inventory")
        {
            gold = gm.currency_p;
        }
        for (int i = 0; i < currentShopList.Count; i++)
        {
            Item item = currentShopList[i];
            GameObject newButton = (GameObject)GameObject.Instantiate(prefab);
            newButton.transform.SetParent(contentPanel, false);

            SampleButton sampleButton = newButton.GetComponent<SampleButton>();
            sampleButton.Setup(item, this);
            gameobjectShopList.Add(newButton);
            print(gameobjectShopList.Count);
        }
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        myGoldDisplay.text = "Gold: " + gold.ToString();
    }

    private void AddButtons(Item item)
    {
        currentShopList.Add(item);
        GameObject newButton = (GameObject)GameObject.Instantiate(prefab);
        newButton.transform.SetParent(contentPanel, false);

        SampleButton sampleButton = newButton.GetComponent<SampleButton>();
        sampleButton.Setup(item, this);
        gameobjectShopList.Add(newButton);
        RefreshDisplay();
        gm.currentShopList = currentShopList;
        

    }

    private void RemoveButtons(Item item)
    {
        currentShopList.Remove(item);
        for (int i = 0; i<gameobjectShopList.Count; i++)
        {
            GameObject currentGameObject = gameobjectShopList[i];
            SampleButton currentSampleButton = currentGameObject.GetComponent<SampleButton>();
            if (currentSampleButton.item == item)
            {
                print(item.itemName);
                Destroy(currentGameObject);
                gameobjectShopList.Remove(currentGameObject);
            }
        }
        RefreshDisplay();
    }

    public void TryTransferItemToOtherShop(Item item)
    {
        if (otherShop.gold >= item.price)
        {
            gold += item.price;
            otherShop.gold -= item.price;
            //AddItem(item, otherShop);
            //RemoveItem(item, this);

            RemoveButtons(item);
            otherShop.AddButtons(item);
        }

    }

    public List<Item> GetCurrentShopList()
    {
        return currentShopList;
    }
}
