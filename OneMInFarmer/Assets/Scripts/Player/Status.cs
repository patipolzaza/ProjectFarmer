using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : IUpgradable
{
    public string statusName;
    [SerializeField] private StatusData statusData;
    public int currentLevel;

    public Status(string name, StatusData statusData)
    {
        statusName = name;
        this.statusData = statusData;
        currentLevel = 1;
    }

    public int GetValue
    {
        get
        {
            return statusData.valuePerLevel * currentLevel;
        }
    }

    public void Upgrade()
    {
        if (!IsReachMaxLevel)
        {
            currentLevel++;
        }
    }

    public void Downgrade()
    {
        if (currentLevel > 1)
        {
            currentLevel--;
        }
    }

    public void ResetLevel()
    {
        currentLevel = 1;
    }

    public bool IsReachMaxLevel
    {
        get
        {
            return currentLevel == statusData.maxLevel;
        }
    }

    public int GetUpgradeCost
    {
        get
        {
            return Mathf.CeilToInt(statusData.startUpgradeCost * (statusData.upgradeCostMultiplierPerLevel * currentLevel));
        }
    }
}
