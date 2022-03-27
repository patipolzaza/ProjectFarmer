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

            float statusValue = baseValue + ((currentLevel - 1) * baseValue * msData.extraValuePerLevel);

            return Mathf.FloorToInt(statusValue);
        }
    }

    public override int GetValueAtLevel(int level)
    {
        MoveSpeedStatusData msData = statusData as MoveSpeedStatusData;
        int baseValue = msData.baseValue;

        float statusValue = baseValue + ((level - 1) * baseValue * msData.extraValuePerLevel);

        return Mathf.FloorToInt(statusValue);
    }
}
