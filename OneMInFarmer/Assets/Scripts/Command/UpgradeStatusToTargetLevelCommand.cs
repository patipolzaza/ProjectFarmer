using UnityEngine;
public class UpgradeStatusToTargetLevelCommand : ICommand
{
    private bool isExecuted;
    private Status statusToUpgrade;
    private int levelBeforeUpgrade;
    private int targetLevel;

    public UpgradeStatusToTargetLevelCommand(Status statusToUpgrade, int targetLevel)
    {
        this.statusToUpgrade = statusToUpgrade;
        levelBeforeUpgrade = this.statusToUpgrade.currentLevel;
        this.targetLevel = targetLevel;
        isExecuted = false;
    }

    public bool Execute()
    {
        if (isExecuted)
        {
            return false;
        }

        int cost = statusToUpgrade.GetUpgradeToTargetLevelCost(targetLevel);
        Wallet playerWallet = Player.Instance.wallet;

        if (playerWallet.coin >= cost)
        {
            playerWallet.LoseCoin(cost);
            while (statusToUpgrade.currentLevel < targetLevel)
            {
                if (!statusToUpgrade.IsReachMaxLevel)
                {
                    statusToUpgrade.Upgrade();
                }
                else
                {
                    break;
                }
            }

            isExecuted = true;
            return true;
        }
        else
        {
            Debug.LogError("Coin is not enough.");
            return false;
        }

    }

    public void Undo()
    {
        if (isExecuted)
        {
            Wallet wallet = Player.Instance.wallet;
            int levelDiff = Mathf.Abs(levelBeforeUpgrade - statusToUpgrade.currentLevel);

            int cost;
            for (int i = 0; i < levelDiff; i++)
            {
                statusToUpgrade.Downgrade();

                cost = statusToUpgrade.GetUpgradeCost;
                wallet.EarnCoin(cost);
            }

            isExecuted = false;
        }
    }
}
