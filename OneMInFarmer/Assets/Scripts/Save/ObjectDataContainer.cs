using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class ObjectDataContainer
{
  [SerializeField]
  private static List<AnimalSaveData> _animalSaveDatas = new List<AnimalSaveData>();
  [SerializeField] private static List<PlotSaveData> _plotSaveDatas = new List<PlotSaveData>();
  [SerializeField] private static WalletSaveData _walletSaveData;
  [SerializeField] private static PlotStatusSaveData _plotStatusSaveData;
  [SerializeField] private static MaxAnimalStatusSaveData _maxAnimalStatusSaveData;
  [SerializeField] private static ScoreSaveData _scoreSaveData;

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

  public static void UpdateWalletSaveData(WalletSaveData walletSaveData)
  {
    _walletSaveData = walletSaveData;
  }

  public static void ClearWalletSaveData()
  {
    _walletSaveData = null;
  }

  public static void UpdatePlotStatusSaveData(PlotStatusSaveData plotStatusSaveData)
  {
    _plotStatusSaveData = plotStatusSaveData;
  }

  public static void ClearPlotStatusSaveData()
  {
    _plotStatusSaveData = null;
  }

  public static void UpdateMaxAnimalStatusSaveData(MaxAnimalStatusSaveData maxAnimalStatusSaveData)
  {
    _maxAnimalStatusSaveData = maxAnimalStatusSaveData;
  }

  public static void ClearMaxAnimalStatusSaveData()
  {
    _maxAnimalStatusSaveData = null;
  }

  public static void UpdateScoreSaveData(ScoreSaveData scoreSaveData)
  {
    _scoreSaveData = scoreSaveData;
  }

  public static void ClearScoreSaveData()
  {
    _scoreSaveData = null;
  }

  public static void ClearAllSaveData(string saveKey)
  {
    ClearAnimalSaveDatas();
    ClearMaxAnimalStatusSaveData();
    ClearPlotSaveDatas();
    ClearPlotStatusSaveData();
    ClearScoreSaveData();
    ClearWalletSaveData();

    PlayerPrefs.DeleteKey(saveKey);
    PlayerPrefs.Save();
  }

  public static void SaveDatas(string saveKey, int day, int debtPaidCount)
  {
    GameSaveData saveData = new GameSaveData(day, debtPaidCount, _maxAnimalStatusSaveData, _animalSaveDatas, _walletSaveData, _plotStatusSaveData, _plotSaveDatas, _scoreSaveData);
    SaveManager.Save(saveKey, saveData);
  }

  /// <summary>
  /// Load all save data by key.
  /// </summary>
  /// <param name="saveKey"></param>
  /// <returns>Day played in save data.</returns>
  public static int LoadDatas(string saveKey)
  {
    var saveLoadedJson = SaveManager.Load(saveKey);
    if (saveLoadedJson != null && saveLoadedJson != string.Empty)
    {
      GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(saveLoadedJson);
      _animalSaveDatas = new List<AnimalSaveData>(saveData.GetAnimalSaveDatas);
      _plotSaveDatas = new List<PlotSaveData>(saveData.GetPlotSaveDatas);
      _plotStatusSaveData = saveData.GetPlotStatusSaveData;
      _walletSaveData = saveData.GetWalletSaveData;
      _maxAnimalStatusSaveData = saveData.GetMaxAnimalStatusSaveData;
      _scoreSaveData = saveData.GetScoreSaveData;

      Player.Instance.wallet.LoadSaveData(_walletSaveData);
      PlotManager.Instance.LoadSaveData(_plotStatusSaveData);
      PlotManager.Instance.LoadPlotsSaveData(_plotSaveDatas);
      AnimalFarmManager.Instance.LoadSaveData(_maxAnimalStatusSaveData);
      AnimalFarmManager.Instance.LoadAnimalDatas(_animalSaveDatas);
      ScoreManager.Instance.LoadSaveData(_scoreSaveData);
      DebtManager.Instance.LoadData(saveData.GetDebtPaidCount);

      return saveData.GetDayPlayed;
    }

    ClearAllSaveData(saveKey);
    return 1;
  }



}
