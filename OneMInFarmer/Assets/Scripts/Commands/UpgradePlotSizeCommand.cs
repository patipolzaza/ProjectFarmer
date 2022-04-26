using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePlotSizeCommand : ICommand
{
    private bool isExecuted = false;
    private int latestUnlockIndex = 0;
    private Status statusToUpgrade = null;

    public UpgradePlotSizeCommand(Status statusToUpgrade)
    {
        this.statusToUpgrade = statusToUpgrade;
        isExecuted = false;

        latestUnlockIndex = statusToUpgrade.GetValue;
    }

    public bool Execute()
    {
        if (!statusToUpgrade.IsReachMaxLevel)
        {
            Wallet playerWallet = Player.Instance.wallet;
            if (playerWallet.coin >= statusToUpgrade.GetUpgradeCost)
            {
                playerWallet?.LoseCoin(statusToUpgrade.GetUpgradeCost);
                statusToUpgrade.Upgrade();
                isExecuted = true;

                PlotManager.Instance.UnlockPlots(latestUnlockIndex, statusToUpgrade.GetValue);

                return true;
            }
        }

        return false;
    }

    public void Undo()
    {
        if (isExecuted)
        {
            Wallet playerWallet = Player.Instance.wallet;
            PlotManager.Instance.LockPlots(latestUnlockIndex, statusToUpgrade.GetValue);
            statusToUpgrade.Downgrade();
            playerWallet.EarnCoin(statusToUpgrade.GetUpgradeCost);
            isExecuted = false;
        }
    }
}
