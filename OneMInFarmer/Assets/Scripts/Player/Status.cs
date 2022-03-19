using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : IUpgradable
{
    public string statusName { get; private set; }
    [SerializeField] private StatusData statusData;
    public int currentLevel { get; private set; }

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
            return statusData.baseValue + statusData.extraValuePerLevel * currentLevel;
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
    public int GetUpgradeToTargetLevelCost(int targetLevel)
    {
        int cost = 0;
        if (targetLevel < currentLevel)
        {
            return 0;
        }

        if (targetLevel > statusData.maxLevel)
        {
            targetLevel = statusData.maxLevel;
        }

        int levelDiff = targetLevel - currentLevel;
        int level = currentLevel;

        for (int i = 0; i < levelDiff; i++)
        {
            cost += Mathf.CeilToInt(statusData.startUpgradeCost * (statusData.upgradeCostMultiplierPerLevel * level));
            level++;
        }

        return cost;
    }
}
