using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyAnimal : ShopBuyBase
{
    public Animal animalInStock { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        interactEvent.AddListener(BuyItemInStock);
    }
    public override void AddNewItemInStock(PickableObject newItem)
    {
        if (newItem is Animal)
        {
            animalInStock = (Animal)newItem;

            animalInStock = Instantiate<Animal>(animalInStock, transform.position, Quaternion.identity, transform);
            animalInStock.gameObject.SetActive(false);

            itemPirce = animalInStock.GetAnimalData.purchasePrice;
            UpdateDisPlayShop();
        }
    }

    public override void BuyItemInStock(Player player)
    {
        if (animalInStock == null)
        {
            return;
        }
        Wallet playerWallet = Player.Instance.wallet;
        if (playerWallet.coin >= itemPirce && AnimalFarmManager.Instance.AddAnimal(animalInStock))
        {
            playerWallet.LoseCoin(itemPirce);
            animalInStock.gameObject.SetActive(true);
            player.PickUpItem(animalInStock);

            animalInStock = null;
        }
    }

    protected override void UpdateDisPlayShop()
    {
        DisplaySpriteIconItem.sprite = animalInStock.GetAnimalData.inShopIcon;
        DisplayTextStacksItem.text = "";
        DisplayTextPirce.text = itemPirce.ToString();
    }
}
