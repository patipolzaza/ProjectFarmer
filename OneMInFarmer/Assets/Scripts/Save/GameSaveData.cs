using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    [SerializeField] private List<AnimalSaveData> _animalDatas;
    [SerializeField] private WalletSaveData _walletSaveData;
    [SerializeField] private List<PlotSaveData> _plotDatas;
    [SerializeField] private PlotStatusSaveData _plotStatusSaveData;
    [SerializeField] private MaxAnimalStatusSaveData _maxAnimalStatusSaveData;

    public List<AnimalSaveData> GetAnimalSaveDatas => _animalDatas;
    public WalletSaveData GetWalletSaveData => _walletSaveData;
    public List<PlotSaveData> GetPlotSaveDatas => _plotDatas;
    public PlotStatusSaveData GetPlotStatusSaveData => _plotStatusSaveData;
    public MaxAnimalStatusSaveData GetMaxAnimalStatusSaveData => _maxAnimalStatusSaveData;

    public GameSaveData(MaxAnimalStatusSaveData maxAnimalStatusSaveData, List<AnimalSaveData> animalSaveDatas, WalletSaveData walletSaveData, PlotStatusSaveData plotStatusSaveData, List<PlotSaveData> plotSaveDatas)
    {
        _maxAnimalStatusSaveData = maxAnimalStatusSaveData;
        _animalDatas = new List<AnimalSaveData>(animalSaveDatas);
        _walletSaveData = walletSaveData;
        _plotDatas = new List<PlotSaveData>(plotSaveDatas);
        _plotStatusSaveData = plotStatusSaveData;
    }
}
