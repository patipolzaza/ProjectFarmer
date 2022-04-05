using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopForSell : Interactable
{
    public static ShopForSell Instance { get; private set; }
    private ItemContainer itemContainer;
    public bool isFinishSellingProcess { get; private set; } = false;
    public int totalSoldPrice { get; private set; }
    [SerializeField] private TextMeshProUGUI priceTMPro;

    protected override void Awake()
    {
        base.Awake();
        itemContainer = new ItemContainer();

        Instance = this;
    }

    public bool PutItemInContainer(IValuable valuable)
    {
        Debug.Log("Selling");
        if (valuable != null)
        {
            valuable.Sell();
   
            //itemContainer.Put(valuable);
            //pickableObject.SetActive(false);
        }

        return true;
    }

    public void SellAllItemsInContainer()
    {
        isFinishSellingProcess = false;
        SellItemInContainerProcess();
    }

    private void SellItemInContainerProcess()
    {
        int loopCount = itemContainer.GetItemCount;

        for (int i = 0; i < loopCount; i++)
        {
            IValuable valuableObj = itemContainer.PickItemInStash;
            if (valuableObj != null)
            {

                int soldPrice = valuableObj.Sell();
                totalSoldPrice += soldPrice;
            }
            else
            {
                break;
            }
        }

        isFinishSellingProcess = true;
    }

    public void ResetTotalSoldPrice()
    {
        totalSoldPrice = 0;
    }

    public void ShowPrice(int price)
    {
        priceTMPro.text = price.ToString();
    }
    public override void HideObjectHighlight()
    {
        sr.color = defaultColor;
        priceTMPro.text = null;
        priceTMPro.gameObject.SetActive(false);
    }
    public override void ShowObjectHighlight()
    {
        if (isInteractable)
        {
            sr.color = highlightColor;
            priceTMPro.gameObject.SetActive(true);
        }
        else
        {
            sr.color = defaultColor;
            priceTMPro.gameObject.SetActive(false);
        }
    }
}
