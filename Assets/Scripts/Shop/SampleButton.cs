using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleButton : MonoBehaviour
{

    public Button button;
    public Text nameLabel;
    public Text priceLabel;
    public Image iconImage;

    public Item item;
    //private ShopScrollList scrollList;
    private TestShopScrollList scrollList;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(HandleClick);
    }

    //public void Setup(Item currentItem, ShopScrollList currentScrollList)
    public void Setup(Item currentItem, TestShopScrollList currentScrollList)
    {
        item = currentItem;
        nameLabel.text = item.itemName;
        priceLabel.text = item.price.ToString();
        iconImage.sprite = item.icon;

        scrollList = currentScrollList;
    }

    public void HandleClick()
    {
        scrollList.TryTransferItemToOtherShop(item);

    }
}
