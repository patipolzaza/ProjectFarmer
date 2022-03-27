using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : IUpgradable
{
    public string statusName { get; private set; }
    [SerializeField] protected StatusData statusData;
    public int currentLevel { get; private set; }

    public Status(string name, StatusData statusData)
    {
        statusName = name;
        this.statusData = statusData;
        currentLevel = 1;
    }

    public virtual int GetValue
    {
        get
        {
            return statusData.baseValue + (currentLevel - 1) * statusData.extraValuePerLevel;
        }
    }

    public virtual int GetValueAtLevel(int level)
    {
        return statusData.baseValue + (level - 1) * statusData.extraValuePerLevel;
    }

    public int GetMaxLevel
    {
        get
        {
            return statusData.maxLevel;
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
            return Mathf.CeilToInt(statusData.startUpgradeCost + ((currentLevel - 1) * statusData.upgradeCostPerLevel * statusData.extraCostPercentagePerLevel));
        }
    }

    public int GetUpgradeToTargetLevelCost(int targetLevel)
    {
        if (targetLevel < currentLevel)
        {
            return 0;
        }

        if (targetLevel > statusData.maxLevel)
        {
            targetLevel = statusData.maxLevel;
        }

        int cost = 0;
        int levelDiff = targetLevel - currentLevel;
        int level = currentLevel;

        for (int i = 0; i < levelDiff; i++)
        {
            float currentLevelCost = statusData.startUpgradeCost + ((level - 1) * statusData.upgradeCostPerLevel * statusData.extraCostPercentagePerLevel);
            cost += Mathf.CeilToInt(currentLevelCost);
            level++;
        }

        return cost;
    }
}
