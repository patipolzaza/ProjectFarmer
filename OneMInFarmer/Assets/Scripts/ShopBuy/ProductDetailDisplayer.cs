using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductDetailDisplayer : MonoBehaviour
{
    [SerializeField] private ShopBuy shop;

    [SerializeField] private Image _iconDisplayer;
    [SerializeField] private TMP_Text _productNameText;
    [SerializeField] private TMP_Text _sellPriceText;
    [SerializeField] private TMP_Text _purchasePriceText;

    [SerializeField] private GameObject _textsParent;

    public void SetUpUI()
    {
        IBuyable sellable = shop.productInStock;

        if (sellable != null)
        {
            _iconDisplayer.gameObject.SetActive(true);

            if (sellable is Animal)
            {
                Animal animal = sellable as Animal;
                SetDisplayIcon(animal.GetAnimalShopIcon);
                SetProductNameText(animal.GetAnimalName);
                SetActiveSellPriceText(true);
                SetSellPriceText($"Sell: {animal.GetSellPricePerKilo}/kg");
                SetPurchasePriceText($"Cost: {animal.GetBuyPrice}");
            }
            else if (sellable is Seed)
            {
                Seed seed = sellable as Seed;
                SeedData seedData = seed.GetItemData as SeedData;
                SetDisplayIcon(seedData.Icon);
                SetProductNameText(seedData.ItemName);
                SetActiveSellPriceText(true);
                SetSellPriceText($"Sell: {seed.GetProduct.GetSellPrice}/pc.");
                SetPurchasePriceText($"Cost: {seed.GetBuyPrice}");
            }
            else if (sellable is AnimalFood)
            {
                AnimalFood food = sellable as AnimalFood;
                AnimalFoodData foodData = food.GetItemData as AnimalFoodData;

                SetDisplayIcon(foodData.Icon);
                SetProductNameText(foodData.ItemName);
                SetActiveSellPriceText(false);
                SetPurchasePriceText($"Cost: {food.GetBuyPrice}");
            }
        }
        else
        {
            _iconDisplayer.gameObject.SetActive(false);
            SetProductNameText("");
            SetSellPriceText("");
            SetPurchasePriceText("");
        }
    }

    public void SetDisplayIcon(Sprite icon)
    {
        _iconDisplayer.sprite = icon;
    }

    public void SetProductNameText(string productName)
    {
        _productNameText.text = productName;
    }

    public void SetSellPriceText(string newText)
    {
        _sellPriceText.text = newText;
    }

    public void SetPurchasePriceText(string newText)
    {
        _purchasePriceText.text = newText;
    }

    public void SetActiveSellPriceText(bool value)
    {
        _sellPriceText.gameObject.SetActive(value);
    }

    public void Show()
    {
        ShowTexts();
    }

    public void Hide()
    {
        HideTexts();
    }

    private void ShowTexts()
    {
        _textsParent.SetActive(true);
    }

    private void HideTexts()
    {
        _textsParent.SetActive(false);
    }
}
