using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePlotSizeCommand : ICommand
{
    private bool isExecuted = false;
    private Status statusToUpgrade = null;

    public UpgradePlotSizeCommand(Status statusToUpgrade)
    {
        this.statusToUpgrade = statusToUpgrade;
        isExecuted = false;
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

                FarmManager.Instance.UnlockPlots();

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
            playerWallet.EarnCoin(statusToUpgrade.GetUpgradeCost);
            statusToUpgrade.Downgrade();
            isExecuted = false;
        }
    }
}
