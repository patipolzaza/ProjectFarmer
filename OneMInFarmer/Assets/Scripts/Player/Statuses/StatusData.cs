using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Status/Data", fileName = "Status Data")]
public class StatusData : ScriptableObject
{
    public string statusName;
    public int maxLevel = 5;
    public int baseValue = 1;
    public int extraValuePerLevel = 1;
    public int startUpgradeCost = 10;
    public int upgradeCostPerLevel = 5;
    public float extraCostPercentagePerLevel = 0;
}
