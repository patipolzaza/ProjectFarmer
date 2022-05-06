using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalSaveData : ObjectSaveData
{
    [SerializeField] private int age;
    [SerializeField] private int currentAgeSpan;
    [SerializeField] private float weight;
    [SerializeField] private int lifePoint;

    [SerializeField] private Vector3 animalScale;

    public int GetAge => age;
    public float GetWeight => weight;
    public int GetLifePoint => lifePoint;
    public int GetAgeSpan => currentAgeSpan;
    public Vector3 GetAnimalScale => animalScale;

    public AnimalSaveData(Animal animal)
    {
        UpdateData(animal);
    }

    public void UpdateData(Animal animal)
    {
        age = animal.age;
        currentAgeSpan = (int)animal.currentAgeSpan;
        weight = animal.weight;
        lifePoint = animal.lifePoint;
        animalScale = animal.GetInteractObject.transform.localScale;
    }

    public void UpdateDataInContainer()
    {
        if (indexInContainer < 0)
        {
            indexInContainer = ObjectDataContainer.AddAnimalSaveData(this);
        }
        else
        {
            ObjectDataContainer.UpdateAnimalSaveData(indexInContainer, this);
        }
    }
}
