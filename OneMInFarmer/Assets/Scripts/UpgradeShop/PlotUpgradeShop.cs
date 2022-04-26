using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotUpgradeShop : PermanentUpgradeShop
{
    protected override ICommand GetUpgradeCommand
    {
        get
        {
            return new UpgradePlotSizeCommand(_statusToUpgrade);
        }
    }
}
