using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalInStockDisplayer : MonoBehaviour
{
    [SerializeField] private ShopBuyAnimal shop;

    [SerializeField] private Image _iconDisplayer;
    [SerializeField] private TMP_Text _productNameText;
    [SerializeField] private TMP_Text _sellPriceText;
    [SerializeField] private TMP_Text _purchasePriceText;

    public void SetUpUI()
    {
        Animal animal = shop.animalInStock;

        if (animal)
        {
            _iconDisplayer.gameObject.SetActive(true);
            SetDisplayIcon(animal.GetAnimalShopIcon);
            SetProductNameText(animal.GetAnimalName);
            SetSellPriceText($"Sell: {animal.GetSellPricePerKilo}/kg");
            SetPurchasePriceText($"Cost: {animal.GetPurchasePrice}");
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
        _productNameText.gameObject.SetActive(true);
        _purchasePriceText.gameObject.SetActive(true);
        _sellPriceText.gameObject.SetActive(true);
    }

    private void HideTexts()
    {
        _productNameText.gameObject.SetActive(false);
        _purchasePriceText.gameObject.SetActive(false);
        _sellPriceText.gameObject.SetActive(false);
    }
}
