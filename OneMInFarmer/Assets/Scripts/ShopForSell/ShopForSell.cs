using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopForSell : Interactable
{
    public static ShopForSell Instance { get; private set; }
    public bool isFinishSellingProcess { get; private set; } = false;
    public int totalSoldPrice { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }

    public bool PutItemInContainer(ISellable valuable)
    {
        if (valuable != null)
        {
            if (valuable is Animal)
            {
                AnimalFarmManager.Instance.RemoveAnimal((Animal)valuable);
            }

            totalSoldPrice += valuable.Sell();
        }

        return true;
    }

    public void ResetTotalSoldPrice()
    {
        totalSoldPrice = 0;
    }
}
