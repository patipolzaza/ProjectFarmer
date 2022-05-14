using UnityEngine;

[System.Serializable]
public class PlotSaveData
{
    [SerializeField] private int _plotIndex;
    [SerializeField] private string _seedPath;
    [SerializeField] private int _plantStage;
    [SerializeField] private int _harvestCount;
    [SerializeField] private int _agePlant;
    [SerializeField] private int _dehydration;

    [SerializeField] private bool _isWither;

    public SeedData GetSeed
    {
        get
        {
            if (_seedPath != null && _seedPath != string.Empty)
            {
                SeedData seedData = Resources.Load<SeedData>(_seedPath);
                return seedData;
            }
            else
            {
                return null;
            }
        }
    }
    public int GetPlotIndex => _plotIndex;
    public int GetPlantStage => _plantStage;
    public int GetHarvestCount => _harvestCount;
    public int GetAgePlant => _agePlant;
    public int GetDehydration => _dehydration;
    public bool GetWitherStatus => _isWither;

    public PlotSaveData(Plot plot)
    {
        UpdateData(plot);
    }

    public void UpdateData(Plot plot)
    {
        _plotIndex = plot.GetPlotIndex;
        _seedPath = plot.seed?.seelResourcePath;
        _plantStage = plot.plantStage;
        _harvestCount = plot.countHarvest;
        _agePlant = plot.agePlant;
        _dehydration = plot.dehydration;
        _isWither = plot.isWither;

        ObjectDataContainer.UpdatePlotSaveData(this);
    }
}
