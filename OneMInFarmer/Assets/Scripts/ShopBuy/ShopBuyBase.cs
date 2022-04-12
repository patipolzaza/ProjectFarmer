using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopBuyBase : Interactable
{
    protected int itemPirce;
    protected int itemStack;
    [SerializeField] protected SpriteRenderer DisplaySpriteIconItem;
    [SerializeField] protected TextMeshProUGUI DisplayTextPirce;
    [SerializeField] protected TextMeshProUGUI DisplayTextStacksItem;

    protected override void Awake()
    {
        base.Awake();
    }

    public virtual void AddNewItemInStock(PickableObject newItem)
    {
      
    }

    public virtual void BuyItemInStock(Player player)
    {
 
    }

    protected virtual void UpdateDisPlayShop()
    {
        
    }
}
