using UnityEngine;
using TMPro;

public class AnimalFoodDetailDisplayer : WindowUIBase
{
    [SerializeField] private GameObject _meatFoodTypePanel;
    [SerializeField] private GameObject _plantFoodTypePanel;

    [SerializeField] private TMP_Text _weightGainValueText;

    public static AnimalFoodDetailDisplayer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowUI(AnimalFood animalFood)
    {
        SetUpUI(animalFood);
        ShowWindow();
    }

    private void SetUpUI(AnimalFood animalFood)
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
    }
}
