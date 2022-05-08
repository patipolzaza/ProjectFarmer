using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour, IContainStatus
{
    public static PlotManager Instance;

    private PlotStatusSaveData _saveData;

    public Status plotSizeStatus { get; private set; }
    [SerializeField] private StatusData plotSizeStatusData;
    [SerializeField] private List<Plot> plots = new List<Plot>();
    private int latestUnlockedPlotIndex = 0;

    private void Awake()
    {
        Instance = this;

        plotSizeStatus = new Status("Plot Size", plotSizeStatusData);
    }

    private void Start()
    {
        UnlockPlots(0, plotSizeStatus.GetBaseValue);

        _saveData = new PlotStatusSaveData(plotSizeStatus);
    }

    public Status GetStatus
    {
        get
        {
            return plotSizeStatus;
        }
    }

    public void LoadSaveData(PlotStatusSaveData saveData)
    {

    }

    /// <summary>
    /// Unlock plots from plot at start index to end index.
    /// </summary>
    /// <param name="startIndex">start plot target to unlock</param>
    /// <param name="endIndex">last plot target to unlock</param>
    public void UnlockPlots(int startIndex, int endIndex)
    {
        if (startIndex >= plots.Count || startIndex > endIndex)
        {
            return;
        }

        if (startIndex < 0) { startIndex = 0; }
        if (endIndex >= plots.Count) { endIndex = plots.Count - 1; }

        int loopCount = endIndex - startIndex;
        int start = startIndex;

        if (startIndex > latestUnlockedPlotIndex) { start -= startIndex - latestUnlockedPlotIndex; }
        for (int i = start; i < start + loopCount; i++)
        {
            plots[i].Unlock();
            latestUnlockedPlotIndex++;
        }
    }

    /// <summary>
    /// Lock plots from plot at start index to end index.
    /// </summary>
    /// <param name="startIndex">start plot target to lock</param>
    /// <param name="endIndex">last plot target to lock</param>
    public void LockPlots(int startIndex, int endIndex)
    {
        if (startIndex >= plots.Count || startIndex > endIndex)
        {
            return;
        }

        if (startIndex < 0) { startIndex = 0; }
        if (endIndex >= plots.Count) { endIndex = plots.Count - 1; }

        int loopCount = endIndex - startIndex;
        int start = startIndex;

        if (startIndex > latestUnlockedPlotIndex) { start -= startIndex - latestUnlockedPlotIndex; }

        for (int i = start; i < start + loopCount; i++)
        {
            plots[i].Lock();
            latestUnlockedPlotIndex--;
        }
    }

    public void LoadPlotsSaveData(List<PlotSaveData> plotSaveDatas)
    {
        foreach (var saveData in plotSaveDatas)
        {
            int plotIndex = saveData.GetPlotIndex;
            plots[plotIndex].LoadSaveData(saveData);
        }
    }

    public void UpdateSaveData()
    {
        _saveData.UpdateSaveData(plotSizeStatus);
    }
}
