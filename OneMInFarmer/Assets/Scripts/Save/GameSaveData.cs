using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    [SerializeField] private int _dayPlayed;
    [SerializeField] private List<AnimalSaveData> _animalDatas;
    [SerializeField] private WalletSaveData _walletSaveData;
    [SerializeField] private List<PlotSaveData> _plotDatas;
    [SerializeField] private PlotStatusSaveData _plotStatusSaveData;
    [SerializeField] private MaxAnimalStatusSaveData _maxAnimalStatusSaveData;
    [SerializeField] private ScoreSaveData _scoreSaveData;
    [SerializeField] private int _debtPaidCount;

    public List<AnimalSaveData> GetAnimalSaveDatas => _animalDatas;
    public WalletSaveData GetWalletSaveData => _walletSaveData;
    public List<PlotSaveData> GetPlotSaveDatas => _plotDatas;
    public PlotStatusSaveData GetPlotStatusSaveData => _plotStatusSaveData;
    public MaxAnimalStatusSaveData GetMaxAnimalStatusSaveData => _maxAnimalStatusSaveData;
    public ScoreSaveData GetScoreSaveData => _scoreSaveData;
    public int GetDayPlayed => _dayPlayed;
    public int GetDebtPaidCount => _debtPaidCount;
    public GameSaveData(int dayPlayed,int debtPaidCount, MaxAnimalStatusSaveData maxAnimalStatusSaveData, List<AnimalSaveData> animalSaveDatas, WalletSaveData walletSaveData, PlotStatusSaveData plotStatusSaveData, List<PlotSaveData> plotSaveDatas, ScoreSaveData scoreSaveData)
    {
        _dayPlayed = dayPlayed;
        _maxAnimalStatusSaveData = maxAnimalStatusSaveData;
        _animalDatas = new List<AnimalSaveData>(animalSaveDatas);
        _walletSaveData = walletSaveData;
        _plotDatas = new List<PlotSaveData>(plotSaveDatas);
        _plotStatusSaveData = plotStatusSaveData;
        _scoreSaveData = scoreSaveData;
        _debtPaidCount = debtPaidCount;
    }
}
