using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedStatus : Status
{
    public MoveSpeedStatus(string name, MoveSpeedStatusData statusData) : base(name, statusData)
    {
        base.statusData = statusData;
    }

    public override int GetValue
    {
        get
        {
            MoveSpeedStatusData msData = statusData as MoveSpeedStatusData;
            int baseValue = msData.baseValue;
            return baseValue + Mathf.FloorToInt(baseValue * msData.extraValuePerLevel / 100);
        }
    }

    public override int GetVelueAtLevel(int level)
    {
        int value = 0;
        int levelDiff = level - currentLevel;

        value = GetValue;

        for (int i = 0; i < levelDiff - 1; i++)
        {
            value += Mathf.FloorToInt(value * statusData.extraValuePerLevel / 100);
        }

        return value;
    }
}
