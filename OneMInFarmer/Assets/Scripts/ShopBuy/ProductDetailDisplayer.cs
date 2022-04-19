using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductDetailDisplayer : WindowUIBase
{
    public static ProductDetailDisplayer Instance { get; private set; }

    [Header("Sell Field")]
    [SerializeField] private GameObject _sellField;
    [SerializeField] private TMP_Text _sellPriceText;

    [Header("Buy Field")]
    [SerializeField] private GameObject _buyField;
    [SerializeField] private TMP_Text _purchasePriceText;

    [Header("Food Type Field")]
    [SerializeField] private GameObject _plantFoodTypeField;
    [SerializeField] private GameObject _meatFoodTypeField;

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

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public void SetUpUI(IBuyable product)
    {
        Debug.Log("Called from " + transform.root.name);
        if (product != null)
        {
            Debug.Log(_buyField);
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

                    SetSellPriceText($"{seed.GetProduct.GetSellPrice}/pc.");
                    SetPurchasePriceText(seed.GetBuyPrice);

                    SetActivePlantHarvestField(true);
                    SetPlantHarvestCountText(seedData.countHarvest);
                }
                else if (product is AnimalFood)
                {
                    AnimalFood food = product as AnimalFood;
                    SetPurchasePriceText(food.GetBuyPrice);

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

        base.HideWindow();
    }
}
