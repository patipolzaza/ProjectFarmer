using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : Item, ISellable
{
    public GameObject GetObject()
    {
        return gameObject;
    }

    public int GetSellPrice => ((ProductData)ItemData).sellPrice;
    public Sprite GetIcon => ItemData.Icon;

    private void OnEnable()
    {
        OnHighlightShowed.AddListener(ShowDetail);
        OnHighlightHided.AddListener(HideDetail);
    }
    private void OnDisable()
    {
        OnHighlightShowed.RemoveListener(ShowDetail);
        OnHighlightHided.RemoveListener(HideDetail);
    }

    public int Sell()
    {
        Wallet playerWallet = Player.Instance.wallet;
        int price = GetSellPrice;
        playerWallet.EarnCoin(price);
        Destroy(gameObject);

        return price;
    }

    public virtual void ShowDetail()
    {
        ProductDetailDisplayer.Instance.ShowUI(this);
    }

    public virtual void HideDetail()
    {
        ProductDetailDisplayer.Instance.HideWindow();
    }
}
