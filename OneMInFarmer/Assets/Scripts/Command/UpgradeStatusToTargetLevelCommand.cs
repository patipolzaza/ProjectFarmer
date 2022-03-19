public class UpgradeStatusToTargetLevelCommand : ICommand
{
    private bool isExecuted;
    private Status statusToUpgrade;
    private int levelBeforeUpfrade;
    private int targetLevel;

    public UpgradeStatusToTargetLevelCommand(Status statusToUpgrade, int targetLevel)
    {
        this.statusToUpgrade = statusToUpgrade;
        this.targetLevel = targetLevel;
        isExecuted = false;
    }

    public void Execute()
    {
        if (isExecuted)
        {
            return;
        }

        int cost = statusToUpgrade.GetUpgradeToTargetLevelCost(targetLevel);
        Wallet playerWallet = Player.Instance.wallet;

        if (playerWallet.coin >= cost)
        {
            playerWallet.LoseCoin(cost);

            while (statusToUpgrade.currentLevel < targetLevel)
            {
                statusToUpgrade.Upgrade();

                if (statusToUpgrade.IsReachMaxLevel)
                {
                    break;
                }
            }
        }

        isExecuted = true;
    }

    public void Undo()
    {
        if (isExecuted)
        {
            Wallet wallet = Player.Instance.wallet;

            while (statusToUpgrade.currentLevel > levelBeforeUpfrade)
            {
                statusToUpgrade.Downgrade();
                wallet.EarnCoin(statusToUpgrade.GetUpgradeCost);
            }

            isExecuted = false;
        }
    }
}
