using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopBuy : Interactable
{
    private ItemStack itemInStock;
    private int itemPirce;
    private int itemStack;
    [SerializeField] private SpriteRenderer DisplaySpriteIconItem;
    [SerializeField] private TextMeshProUGUI DisplayTextPirce;
    [SerializeField] private TextMeshProUGUI DisplayTextStacksItem;

    protected override void Awake()
    {
        base.Awake();

        interactEvent.AddListener(BuyItemInStock);
    }

    public void AddNewItemInStock(ItemStack newItem)
    {
        ItemStack prepareItem = newItem;
        itemStack = (int)Random.Range(3, 6);
        itemInStock = prepareItem;
        itemPirce = itemStack * prepareItem.GetItemData.purchasePrice;
        UpdateDisPlayShop();
    }

    public void BuyItemInStock(Player player)
    {
        if (itemInStock == null)
        {
            return;
        }
        ItemStack itemBought = Instantiate(itemInStock, new Vector3(0, 0, 0), Quaternion.identity);
        itemBought.SetCurrentStacks(itemStack);
        player.SetHoldingItem(itemBought);
    }

    private void UpdateDisPlayShop()
    {
        Debug.Log("UpdateDisPlayShop");
        DisplaySpriteIconItem.sprite = itemInStock.GetItemData.Icon;
        DisplayTextStacksItem.text = itemStack.ToString();
        DisplayTextPirce.text = itemPirce.ToString();
    }
}
