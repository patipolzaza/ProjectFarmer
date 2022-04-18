using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductPricesDisplayer : WindowUIBase
{
    [SerializeField] private TMP_Text _sellPriceText;
    [SerializeField] private TMP_Text _purchasePriceText;

    [SerializeField] private GameObject _sellField;
    [SerializeField] private GameObject _buyField;

    public void SetUpUI(IBuyable product)
    {
        SetActiveBuyField(true);
        SetActiveSellField(true);

        if (product != null)
        {
            if (product is Animal)
            {
                Animal animal = product as Animal;
                SetSellPriceText($"{animal.GetSellPricePerKilo}/kg");
                SetPurchasePriceText($"{animal.GetBuyPrice}");
            }
            else if (product is Seed)
            {
                Seed seed = product as Seed;
                SeedData seedData = seed.GetItemData as SeedData;

                SetSellPriceText($"{seed.GetProduct.GetSellPrice}/pc.");
                SetPurchasePriceText($"{seed.GetBuyPrice}");
            }
            else if (product is AnimalFood)
            {
                AnimalFood food = product as AnimalFood;
                AnimalFoodData foodData = food.GetItemData as AnimalFoodData;
                SetActiveSellField(false);

                SetPurchasePriceText($"{food.GetBuyPrice}");
            }
        }
        else
        {
            SetSellPriceText("");
            SetPurchasePriceText("");
        }
    }

    public void SetSellPriceText(string newText)
    {
        _sellPriceText.text = newText;
    }

    public void SetPurchasePriceText(string newText)
    {
        _purchasePriceText.text = newText;
    }

    public void SetActiveBuyField(bool value)
    {
        _buyField.SetActive(value);
    }

    public void SetActiveSellField(bool value)
    {
        _sellField.SetActive(value);
    }
}
