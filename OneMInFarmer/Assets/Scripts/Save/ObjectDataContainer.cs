using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class ObjectDataContainer
{
    private static string _gameSaveKey = "gameSave";
    [SerializeField]
    private static List<AnimalSaveData> _animalSaveDatas = new List<AnimalSaveData>();
    [SerializeField] private static List<PlotSaveData> _plotSaveDatas = new List<PlotSaveData>();

    public static List<AnimalSaveData> GetAnimalDatas => _animalSaveDatas;
    public static List<PlotSaveData> GetPlotDatas => _plotSaveDatas;

    public static void UpdateAnimalSaveData(AnimalSaveData data)
    {
        if (data != null)
        {
            if (_animalSaveDatas.Contains(data))
            {
                int index = _animalSaveDatas.IndexOf(data);
                _animalSaveDatas[index] = data;
            }
            else
            {
                AddAnimalSaveData(data);
            }
        }
    }
    public static void AddAnimalSaveData(AnimalSaveData data)
    {
        _animalSaveDatas.Add(data);
    }
    public static void RemoveAnimalSaveData(AnimalSaveData data)
    {
        _animalSaveDatas.Remove(data);
    }
    public static void ClearAnimalSaveDatas()
    {
        _animalSaveDatas.Clear();
    }

    public static void AddPlotSaveData(PlotSaveData data)
    {
        _plotSaveDatas.Add(data);
    }
    public static void UpdatePlotSaveData(PlotSaveData data)
    {
        if (data != null)
        {
            if (_plotSaveDatas.Contains(data))
            {
                int index = _plotSaveDatas.IndexOf(data);
                _plotSaveDatas[index] = data;
            }
            else
            {
                AddPlotSaveData(data);
            }
        }
    }
    public static void RemovePlotSaveData(PlotSaveData data)
    {
        _plotSaveDatas.Remove(data);
    }
    public static void ClearPlotSaveDatas()
    {
        _plotSaveDatas.Clear();
    }

    public static void SaveDatas()
    {
        /*AnimalSaveDataList animalSaveList = new AnimalSaveDataList(animalSaveDatas);
        SaveManager.Save(animalSaveDatasKey, animalSaveList);*/
    }

    public static void LoadData()
    {
        var saveLoadedJson = SaveManager.Load(_gameSaveKey);
        if (saveLoadedJson != null && saveLoadedJson != string.Empty)
        {
            /*var animalSaveDataList = JsonUtility.FromJson<AnimalSaveDataList>(animalLoadedJson);
            animalSaveDatas = animalSaveDataList.GetAnimalSaveDatas;*/
        }
        //.......//
    }
}
