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

    private void OnEnable()
    {
        OnHighlightShowed.AddListener(ShowSellDetail);
        OnHighlightHided.AddListener(HideSellDetail);
    }

    private void OnDisable()
    {
        OnHighlightShowed.RemoveListener(ShowSellDetail);
        OnHighlightHided.RemoveListener(HideSellDetail);
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

    private void ShowSellDetail()
    {
        ISellable sellable = Player.Instance.playerHand.holdingObject as ISellable;
        if (sellable != null)
        {
            ProductSellPriceDisplayer sellPriceDisplayer = ProductSellPriceDisplayer.Instance;
            sellPriceDisplayer.SetUpText(sellable);
            sellPriceDisplayer.ShowWindow();
        }
    }

    private void HideSellDetail()
    {
        ProductSellPriceDisplayer sellPriceDisplayer = ProductSellPriceDisplayer.Instance;
        sellPriceDisplayer.HideWindow();
    }
}
