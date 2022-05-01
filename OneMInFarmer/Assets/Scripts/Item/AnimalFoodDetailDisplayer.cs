using UnityEngine;
using TMPro;

public class AnimalFoodDetailDisplayer : WindowUIBase
{
    [SerializeField] private GameObject _meatFoodTypePanel;
    [SerializeField] private GameObject _plantFoodTypePanel;

    [SerializeField] private TMP_Text _weightGainValueText;

    [SerializeField] private GameObject _sellPricePanel;
    [SerializeField] private TMP_Text _sellPriceValueText;

    public static AnimalFoodDetailDisplayer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowUI(IAnimalEdible animalFood)
    {
        SetUpUI(animalFood);
        ShowWindow();
    }

    private void SetUpUI(IAnimalEdible animalFood)
    {
        SetWeightGainValueText(animalFood.GetWeightGain);

        if (animalFood.GetFoodType.Equals(FoodType.Meat))
        {
            SetActiveMeatTypePanel(true);
        }
        else
        {
            SetActivePlantTypePanel(true);
        }

        if (animalFood is ISellable)
        {
            ISellable sellable = animalFood as ISellable;
            SetActiveSellPricePanel(true);
            SetSellPriceValueText(sellable.GetSellPrice);
        }
    }

    private void SetWeightGainValueText(float weightGain)
    {
        _weightGainValueText.text = weightGain.ToString("0.##");
    }

    private void SetActiveMeatTypePanel(bool value)
    {
        _meatFoodTypePanel.SetActive(value);
    }

    private void SetActivePlantTypePanel(bool value)
    {
        _plantFoodTypePanel.SetActive(value);
    }

    private void SetActiveSellPricePanel(bool value)
    {
        _sellPricePanel.SetActive(value);
    }

    private void SetSellPriceValueText(int newSellPriceValue)
    {
        _sellPriceValueText.text = newSellPriceValue.ToString();
    }

    /// <summary>
    /// This method is will showed without setup please use ShowUI(AnimalFood) to show this ui with seted detail.
    /// </summary>
    public override void ShowWindow()
    {
        base.ShowWindow();
    }

    public override void HideWindow()
    {
        base.HideWindow();

        SetActiveMeatTypePanel(false);
        SetActivePlantTypePanel(false);
        SetActiveSellPricePanel(false);
    }
}
