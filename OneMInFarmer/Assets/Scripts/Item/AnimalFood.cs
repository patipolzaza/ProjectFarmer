using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimalFood : Item, IBuyable, IAnimalConsumable
{
    public FoodType GetFoodType => ((AnimalFoodData)ItemData).foodType;

    public float GetWeightGain => ((AnimalFoodData)ItemData).weightGain;

    public int GetBuyPrice => ItemData.purchasePrice;
    public GameObject GetObject() => gameObject;
    public Sprite GetIcon => ItemData.Icon;

    private void OnValidate()
    {
        if (ItemData != null && ItemData.GetType() != typeof(AnimalFoodData))
        {
            ItemData = null;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        //AddTargetType(typeof(Animal));
    }

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

    public bool Feed(Animal targetToFeed)
    {
        if (targetToFeed.TakeFood(this))
        {
            currentStack--;
            SetParent(targetToFeed.transform);
            SetLocalPosition(Vector3.zero, false, false, false, false);
            gameObject.SetActive(false);

            return true;
        }

        return false;
    }

    public bool Buy(Player player)
    {
        Wallet playerWallet = player.wallet;
        int price = GetBuyPrice;

        if (playerWallet.coin >= price)
        {
            playerWallet.LoseCoin(price);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShowDetail()
    {
        AnimalFoodDetailDisplayer foodDetailDisplayer = AnimalFoodDetailDisplayer.Instance;
        foodDetailDisplayer.ShowUI(this);
    }

    private void HideDetail()
    {
        AnimalFoodDetailDisplayer foodDetailDisplayer = AnimalFoodDetailDisplayer.Instance;
        foodDetailDisplayer.HideWindow();
    }
}
