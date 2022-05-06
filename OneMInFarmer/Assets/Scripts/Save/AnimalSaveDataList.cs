using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalSaveDataList
{
    [SerializeField]
    private List<AnimalSaveData> animalSaveDatas;

    public List<AnimalSaveData> GetAnimalSaveDatas => animalSaveDatas;

    public AnimalSaveDataList(List<AnimalSaveData> animalSaveDatas)
    {
        this.animalSaveDatas = new List<AnimalSaveData>(animalSaveDatas);
    }
}
