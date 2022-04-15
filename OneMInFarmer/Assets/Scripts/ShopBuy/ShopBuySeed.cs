using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopBuySeed : ShopBuyBase
{
    [SerializeField] protected TextMeshProUGUI DisplayTextStacksItem;
    protected int itemStack;

    [SerializeField] private Item itemInStock;

    protected override void Awake()
    {
        base.Awake();

        interactEvent.AddListener(BuyItemInStock);
    }
    public override void AddNewItemInStock(PickableObject newItem)
    {
        if (newItem is Item)
        {
            itemInStock = (Item)newItem;
            Item prepareItem = (Item)newItem;
            itemStack = (int)Random.Range(3, 6);
            itemPirce = itemStack * prepareItem.GetItemData.purchasePrice;
            UpdateDisplayShop();
            base.AddNewItemInStock(newItem);
        }
    }

    public override void BuyItemInStock(Player player)
    {
        base.BuyItemInStock(player);
        Debug.Log("BuyItemInStock");
        if (itemInStock == null)
        {
            return;
        }
        Wallet playerWallet = Player.Instance.wallet;
        if (playerWallet.coin >= itemPirce)
        {
            playerWallet.LoseCoin(itemPirce);

            Item itemBought = Instantiate(itemInStock, new Vector3(0, 0, 0), Quaternion.identity);
            itemBought.SetCurrentStackNumber(itemStack);
            player.PickUpItem(itemBought);
        }


    }

    protected override void UpdateDisplayShop()
    {
        DisplaySpriteIconItem.sprite = itemInStock.GetItemData.Icon;
        DisplayTextStacksItem.text = itemStack.ToString();
        DisplayTextPirce.text = itemPirce.ToString();
    }
}
