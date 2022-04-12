public class UpgradeStatusCommand : ICommand
{
    private Status statusToUpgrade;
    private bool isExecuted;
    public UpgradeStatusCommand(Status statusToUpgrade)
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
            statusToUpgrade.Downgrade();
            playerWallet.EarnCoin(statusToUpgrade.GetUpgradeCost);
            isExecuted = false;
        }
    }
}
