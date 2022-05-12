using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/Status/PercentStatusData")]
public class PercentStatusData : StatusData
{
    public PercentStatusData() { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="extraValuePerLevel">The percentage that will increase from base value per level.</param>
    public override void Init(string statusName, int maxLevel, int baseValue, int extraValuePerLevel, int upgradeCostPerLevel, int extraCostPercentage)
    {
        base.Init(statusName, maxLevel, baseValue, extraValuePerLevel, upgradeCostPerLevel, extraCostPercentage);
    }
}
