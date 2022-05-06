using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSaveData : ObjectSaveData
{
    public int age { get; private set; }
    public AgeSpan currentAgeSpan { get; private set; }
    public float weight { get; private set; }
    public int lifePoint { get; private set; }

    public Vector3 animalScale { get; private set; }

    public AnimalSaveData(Animal animal)
    {
        age = animal.age;
        currentAgeSpan = animal.currentAgeSpan;
        weight = animal.weight;
        lifePoint = animal.lifePoint;
        animalScale = animal.GetInteractObject.transform.localScale;
    }

    public void UpdateDataInContainer()
    {
        if (indexInContainer < 0)
        {
            indexInContainer = ObjectDataContainer.AddData(this);
        }
        else
        {
            ObjectDataContainer.UpdateData(indexInContainer, this);
        }
    }
}
