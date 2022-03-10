using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyManager : MonoBehaviour
{
    public ShopBuy[] shopBuys;
    public ItemStack[] ListItem;

    public void AddItemToShop()
    {
        foreach (ShopBuy shopBuy in shopBuys)
        {
            shopBuy.AddNewItemInStock(ListItem[Random.Range(0, ListItem.Length)]);
        }
    }

}
