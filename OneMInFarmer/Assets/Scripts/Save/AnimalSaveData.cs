using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalSaveData
{
  [SerializeField] private string _animalType;
  [SerializeField] private int _age;
  [SerializeField] private int _currentAgeSpan;
  [SerializeField] private float _weight;
  [SerializeField] private int _lifePoint;

  [SerializeField] private Vector3 _animalScale;
  [SerializeField] private Vector2 _animalPosition;

  public int GetAge => _age;
  public float GetWeight => _weight;
  public int GetLifePoint => _lifePoint;
  public int GetAgeSpan => _currentAgeSpan;
  public Vector3 GetAnimalScale => _animalScale;
  public Vector3 GetAnimalPosition => _animalPosition;
  public string GetAnimalType => _animalType;

  public AnimalSaveData(Animal animal)
  {
    UpdateData(animal);
  }

  public void UpdateData(Animal animal)
  {
    _animalType = animal.gameObject.name.Replace("(Clone)", "");
    _age = animal.age;
    _currentAgeSpan = (int)animal.currentAgeSpan;
    _weight = animal.weight;
    _lifePoint = animal.lifePoint;
    _animalScale = animal.GetInteractObject.transform.localScale;
    _animalPosition = animal.GetInteractObject.transform.position;

    ObjectDataContainer.UpdateAnimalSaveData(this);
  }
}
