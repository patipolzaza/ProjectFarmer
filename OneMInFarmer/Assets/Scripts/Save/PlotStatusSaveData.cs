using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlotStatusSaveData
{
    [SerializeField] private int _plotStatusLevel;

    public int GetPlotStatusLevel => _plotStatusLevel;

    public PlotStatusSaveData(Status plotStatus)
    {
        _plotStatusLevel = plotStatus.currentLevel;
    }
}
