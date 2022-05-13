using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimalFood : Item, IBuyable, IAnimalEdible
{
    public FoodType GetFoodType => ((AnimalFoodData)ItemData).foodType;

    public float GetWeightGain => ((AnimalFoodData)ItemData).weightGain;

    public int GetBuyPrice => ((AnimalFoodData)ItemData).purchasePrice;
    public GameObject GetObject() => gameObject;
    public Sprite GetIcon => ItemData.Icon;

    public Food GetFood
    {
        get
        {
            return new Food(GetFoodType, GetWeightGain);
        }
    }

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
        if (targetToFeed.TakeFood(GetFood))
        {
            currentStack--;
            if (currentStack <= 0)
            {
                Destroy(gameObject);
                return true;
            }
            else
            {
                return false;
            }
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
