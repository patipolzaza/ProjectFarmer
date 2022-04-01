using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopForSell : Interactable
{
    public static ShopForSell Instance { get; private set; }
    private ItemContainer itemContainer;
    public bool isFinishSellingProcess { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();
        itemContainer = new ItemContainer();

        Instance = this;
    }

    public bool PutItemInContainer(IValuable valuable)
    {
        if (valuable != null)
        {
            GameObject pickableObject = valuable.GetObject();
            itemContainer.Put(valuable);
            pickableObject.SetActive(false);
        }

        return true;
    }

    public void SellAllItemsInContainer()
    {
        isFinishSellingProcess = false;
        StartCoroutine(SellItemInContainerProcess());
    }

    private IEnumerator SellItemInContainerProcess()
    {
        int loopCount = itemContainer.GetItemCount;

        for (int i = 0; i < loopCount; i++)
        {
            IValuable valuableObj = itemContainer.PickItemInStash;
            if (valuableObj != null)
            {

                valuableObj.Sell();
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.25f);
        }

        isFinishSellingProcess = true;
    }
}
