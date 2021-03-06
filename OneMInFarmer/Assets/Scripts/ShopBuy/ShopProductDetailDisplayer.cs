using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopProductDetailDisplayer : WindowUIBase
{
    public static ShopProductDetailDisplayer Instance { get; private set; }

    [Header("Sell Field")]
    [SerializeField] private GameObject _sellField;
    [SerializeField] private TMP_Text _sellPriceText;

    [Header("Buy Field")]
    [SerializeField] private GameObject _buyField;
    [SerializeField] private TMP_Text _purchasePriceText;

    [Header("Food Type Field")]
    [SerializeField] private GameObject _plantFoodTypeField;
    [SerializeField] private GameObject _meatFoodTypeField;

    [Header("Weight Gain Field")]
    [SerializeField] private GameObject _weightGainField;
    [SerializeField] private TMP_Text _weightGainValueText;

    [Header("Edible Food Type Field")]
    [SerializeField] private GameObject _edibleFoodTypeField;
    [SerializeField] private GameObject _edibleMeatField;
    [SerializeField] private GameObject _ediblePlantField;

    [Header("Animal Lifespan Field")]
    [SerializeField] private GameObject _lifespanField;
    [SerializeField] private TMP_Text _lifespanValueText;

    [Header("Plant Harvest Field")]
    [SerializeField] private GameObject _plantHarvestField;
    [SerializeField] private TMP_Text _plantHarvestCountText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetUpUI(IBuyable product)
    {
        if (product != null)
        {
            SetActiveBuyField(true);

            if (product != null)
            {
                if (product is Animal)
                {
                    Animal animal = product as Animal;
                    SetPurchasePriceText(animal.GetBuyPrice);

                    SetActiveSellField(true);
                    SetSellPriceText($"{animal.GetSellPricePerKilo}/kg");

                    SetActiveEdibleFoodField(true);
                    List<FoodType> edibleFoods = animal.GetEdibleFoods;
                    if (edibleFoods.Contains(FoodType.Meat))
                    {
                        SetActiveEdibleMeat(true);
                    }
                    if (edibleFoods.Contains(FoodType.Plant))
                    {
                        SetActiveEdiblePlant(true);
                    }

                    SetActiveLifespanField(true);
                    SetLifespanText(animal.GetLifespan);
                }
                else if (product is Seed)
                {
                    Seed seed = product as Seed;
                    SeedData seedData = seed.GetItemData as SeedData;

                    SetPurchasePriceText(seed.GetBuyPrice);
                    SetActiveSellField(true);
                    SetSellPriceText($"{seed.GetProduct.GetSellPrice}/pc.");

                    SetActivePlantHarvestField(true);
                    SetPlantHarvestCountText(seedData.countHarvest);
                }
                else if (product is AnimalFood)
                {
                    AnimalFood food = product as AnimalFood;
                    SetPurchasePriceText(food.GetBuyPrice);

                    SetActiveWeightGainField(true);
                    SetWeightGainValueText(food.GetWeightGain);

                    FoodType foodType = food.GetFoodType;

                    if (foodType.Equals(FoodType.Plant))
                    {
                        SetActivePlantFoodTypeField(true);
                    }
                    else if (foodType.Equals(FoodType.Meat))
                    {
                        SetActiveMeatFoodTypeField(true);
                    }
                }
            }
        }
    }

    private void SetSellPriceText(string newText)
    {
        _sellPriceText.text = newText;
    }

    private void SetPurchasePriceText(int purchasePrice)
    {
        _purchasePriceText.text = purchasePrice.ToString();
    }

    private void SetActiveBuyField(bool value)
    {
        _buyField.SetActive(value);
    }

    private void SetActiveSellField(bool value)
    {
        _sellField.SetActive(value);
    }

    private void SetActiveEdibleFoodField(bool value)
    {
        _edibleFoodTypeField.SetActive(value);
    }

    private void SetActiveEdibleMeat(bool value)
    {
        _edibleMeatField.SetActive(value);
    }

    private void SetActiveEdiblePlant(bool value)
    {
        _ediblePlantField.SetActive(value);
    }

    private void SetActiveMeatFoodTypeField(bool value)
    {
        _meatFoodTypeField.SetActive(value);
    }

    private void SetActivePlantFoodTypeField(bool value)
    {
        _plantFoodTypeField.SetActive(value);
    }

    private void SetActiveLifespanField(bool value)
    {
        _lifespanField.SetActive(value);
    }

    private void SetLifespanText(int animalLifespan)
    {
        _lifespanValueText.text = animalLifespan.ToString();
    }

    private void SetActivePlantHarvestField(bool value)
    {
        _plantHarvestField.SetActive(value);
    }

    private void SetPlantHarvestCountText(int harvestCount)
    {
        _plantHarvestCountText.text = harvestCount.ToString();
    }

    private void SetActiveWeightGainField(bool value)
    {
        _weightGainField.SetActive(value);
    }

    private void SetWeightGainValueText(float weightGain)
    {
        _weightGainValueText.text = weightGain.ToString("0.##");
    }


    public override void HideWindow()
    {
        SetActiveBuyField(false);
        SetActiveSellField(false);
        SetActiveEdibleFoodField(false);
        SetActiveEdibleMeat(false);
        SetActiveEdiblePlant(false);
        SetActivePlantFoodTypeField(false);
        SetActiveMeatFoodTypeField(false);
        SetActivePlantHarvestField(false);
        SetActiveLifespanField(false);
        SetActiveWeightGainField(false);

        base.HideWindow();
    }
}
