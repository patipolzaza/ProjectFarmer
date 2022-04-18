using UnityEngine;
using TMPro;

public class AnimalDetailDisplayer : WindowUIBase
{
    public static AnimalDetailDisplayer Instance { get; private set; }

    [SerializeField] private TMP_Text _ageSpanText;
    [SerializeField] private TMP_Text _weightText;
    [SerializeField] private GameObject _meatIconObj;
    [SerializeField] private GameObject _plantIconObj;

    protected override void Awake()
    {
        Instance = this;
    }

    public void SetDetails(Animal animal, bool isEatMeat, bool isEatPlant)
    {
        SetAgeSpanText(animal.currentAgeSpan.ToString());
        SetWeightText(animal.weight);
        SetActiveMeatIcon(isEatMeat);
        SetActivePlantIcon(isEatPlant);
    }

    private void SetAgeSpanText(string newText)
    {
        _ageSpanText.text = newText;
    }

    private void SetWeightText(float weight)
    {
        _weightText.text = $"{weight.ToString("0.##")}";
    }
    private void SetActiveMeatIcon(bool value)
    {
        _meatIconObj.SetActive(value);
    }
    private void SetActivePlantIcon(bool value)
    {
        _plantIconObj.SetActive(value);
    }
}
