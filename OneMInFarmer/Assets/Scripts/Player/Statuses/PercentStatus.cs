using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentStatus : Status
{
    public PercentStatus(string name, PercentStatusData statusData) : base(name, statusData)
    {
        base.statusData = statusData;
    }

    public int GetPercentageUpgradeValue => Mathf.CeilToInt((currentLevel - 1) * statusData.extraValuePerLevel);

    public int GetPercentageUpgradeValueAtLevel(int targetLevel) => Mathf.CeilToInt((targetLevel - 1) * statusData.extraValuePerLevel);

    public override int GetValue
    {
        get
        {
            PercentStatusData msData = statusData as PercentStatusData;
            int baseValue = msData.baseValue;

            float statusValue = baseValue + ((currentLevel - 1) * msData.extraValuePerLevel);

            return Mathf.FloorToInt(statusValue);
        }
    }

    public override int GetValueAtLevel(int level)
    {
        PercentStatusData msData = statusData as PercentStatusData;
        int baseValue = msData.baseValue;

        float statusValue = baseValue + ((level - 1) * baseValue * msData.extraValuePerLevel);

        return Mathf.FloorToInt(statusValue);
    }
}
