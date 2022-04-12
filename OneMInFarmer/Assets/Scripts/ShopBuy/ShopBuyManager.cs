using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyManager : MonoBehaviour
{
    [SerializeField]private ShopBuySeed[] shopBuySeeds;
    [SerializeField]private ShopBuyAnimal[] shopBuyAnimals;
    public Item[] ListItem;
    public Animal[] ListAnimal;

    private void Start()
    {
        AddItemToShop();
    }
    public void AddItemToShop()
    {
        foreach (ShopBuySeed shopBuy in shopBuySeeds)
        {
            shopBuy.AddNewItemInStock(ListItem[Random.Range(0, ListItem.Length)]);
        }
        foreach (ShopBuyAnimal shopBuy in shopBuyAnimals)
        {
            shopBuy.AddNewItemInStock(ListAnimal[Random.Range(0, ListAnimal.Length)]);
        }
    }

}
