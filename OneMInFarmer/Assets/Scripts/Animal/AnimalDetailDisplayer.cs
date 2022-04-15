using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimalDetailDisplayer : MonoBehaviour
{
    [SerializeField] private Animal animal;

    [SerializeField] private TMP_Text _ageSpanText;
    [SerializeField] private TMP_Text _weightText;

    private void UpdateUI()
    {
        if (animal == null) { return; }

        SetAgeSpanText(animal.currentAgeSpan.ToString());
        SetWeightText(animal.weight);
    }

    private void SetAgeSpanText(string newText)
    {
        _ageSpanText.text = newText;
    }

    private void SetWeightText(float weight)
    {
        _weightText.text = $"W: {weight.ToString("0.##")} kg";
    }

    public void Show()
    {
        UpdateUI();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
