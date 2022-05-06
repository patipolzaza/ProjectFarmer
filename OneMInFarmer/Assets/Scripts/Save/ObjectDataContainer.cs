using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class ObjectDataContainer
{
    private static int lastIndex = 0;
    [SerializeField]
    private static List<AnimalSaveData> animalSaveDatas = new List<AnimalSaveData>();

    private static string animalSaveDatasKey = "animalSaveDatas";

    public static List<AnimalSaveData> GetDatas => animalSaveDatas;

    public static void UpdateAnimalSaveData(int index, AnimalSaveData newData)
    {
        animalSaveDatas[index] = newData;
    }

    public static int AddAnimalSaveData(AnimalSaveData data)
    {
        animalSaveDatas.Add(data);
        lastIndex++;
        return lastIndex;
    }

    public static void Remove(int index)
    {
        animalSaveDatas.RemoveAt(index);
    }

    public static void ClearDatas()
    {
        animalSaveDatas.Clear();
    }

    public static void SaveDatas()
    {
        AnimalSaveDataList animalSaveList = new AnimalSaveDataList(animalSaveDatas);
        SaveManager.Save(animalSaveDatasKey, animalSaveList);
    }

    public static void LoadData()
    {
        var animalLoadedJson = SaveManager.Load(animalSaveDatasKey);
        if (animalLoadedJson != null && animalLoadedJson != string.Empty)
        {
            var animalSaveDataList = JsonUtility.FromJson<AnimalSaveDataList>(animalLoadedJson);
            animalSaveDatas = animalSaveDataList.GetAnimalSaveDatas;
        }
        //.......//
    }
}
