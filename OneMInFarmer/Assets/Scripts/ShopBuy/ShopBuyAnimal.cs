using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyAnimal : ShopBuyBase
{
    [SerializeField] private Animal itemInStock;


    protected override void Awake()
    {
        base.Awake();

        interactEvent.AddListener(BuyItemInStock);
    }
    public override void AddNewItemInStock(PickableObject newItem)
    {
        if (newItem is Animal)
        {
            itemInStock = (Animal)newItem;
            Animal prepareItem = (Animal)newItem;
            itemStack = 0;
            itemPirce = itemStack * prepareItem.GetAnimalData.purchasePrice;
            UpdateDisPlayShop();
        }
    }

    public override void BuyItemInStock(Player player)
    {
        if (itemInStock == null)
        {
            return;
        }
        Wallet playerWallet = Player.Instance.wallet;
        if (playerWallet.coin >= itemPirce)
        {
            playerWallet.LoseCoin(itemPirce);
            Vector3 pos = transform.position;
            Animal AnimalBought = Instantiate(itemInStock, pos, Quaternion.identity);
            //player.PickUpItem(AnimalBought);
        }


    }

    protected override void UpdateDisPlayShop()
    {
        Debug.Log("UpdateDisPlayShop");
        DisplaySpriteIconItem.sprite = itemInStock.GetAnimalData.spriteAnimal;
        DisplayTextStacksItem.text = "";
        DisplayTextPirce.text = itemPirce.ToString();
    }
}
