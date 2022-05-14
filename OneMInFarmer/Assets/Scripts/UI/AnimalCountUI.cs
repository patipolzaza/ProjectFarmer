using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimalCountUI : WindowUIBase
{
    [SerializeField] private TMP_Text _animalCountText;
    private AnimalFarmManager _animalFarmManager;

    private void OnEnable()
    {
        if (!_animalFarmManager)
        {
            _animalFarmManager = FindObjectOfType<AnimalFarmManager>();
        }

        SetUIText(_animalFarmManager.GetCurrentAnimalCount, _animalFarmManager.GetStatus.GetValue);
        _animalFarmManager.OnValueChanged += SetUIText;
    }

    private void OnDisable()
    {
        _animalFarmManager.OnValueChanged -= SetUIText;
    }

    private void SetUIText(int animalCount, int maxAnimal)
    {
        _animalCountText.text = $"{animalCount}/{maxAnimal}";
    }
}
