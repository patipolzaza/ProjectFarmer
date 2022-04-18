using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProductSellPriceDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _sellPriceText;

    public void SetSellPriceText(string newText)
    {
        _sellPriceText.text = newText;
    }

    public void SetUpText(ISellable sellable)
    {
        if (sellable != null)
        {
            SetSellPriceText($"Sell: {sellable.GetSellPrice}");
        }
        else
        {
            SetSellPriceText("");
        }
    }

    public void ShowSellPriceText()
    {
        ISellable sellable = Player.Instance.playerHand.holdingObject as ISellable;
        SetUpText(sellable);

        gameObject.SetActive(true);
    }

    public void HideSellPriceText()
    {
        gameObject.SetActive(false);
    }
}
