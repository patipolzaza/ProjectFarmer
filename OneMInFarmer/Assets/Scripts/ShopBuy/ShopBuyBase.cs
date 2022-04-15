using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ShopBuyBase : Interactable
{
    protected int itemPirce;
    [SerializeField] protected Image DisplaySpriteIconItem;
    [SerializeField] protected TextMeshProUGUI DisplayTextPirce;

    public UnityEvent OnRestocked;
    public UnityEvent OnProductSold;

    protected override void Awake()
    {
        base.Awake();
    }

    public virtual void AddNewItemInStock(PickableObject newItem)
    {
        OnRestocked?.Invoke();
    }

    public virtual void BuyItemInStock(Player player)
    {
        OnProductSold?.Invoke();
    }

    protected virtual void UpdateDisplayShop()
    {

    }
}
