using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalDetailDisplayer : MonoBehaviour
{
    [SerializeField] private Animal _animal;
    [SerializeField] private TextMesh _ageSpanText;
    [SerializeField] private TextMesh _weightText;

    private void SetUpDetail()
    {
        SetAgeSpanText(_animal.currentAgeSpan.ToString());
        SetWeightText(_animal.weight);
    }

    private void SetWeightText(float weight)
    {
        _weightText.text = $"W: {weight} kg";
    }

    private void SetAgeSpanText(string newText)
    {
        _ageSpanText.text = newText;
    }

    public void Show()
    {
        SetUpDetail();

        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
