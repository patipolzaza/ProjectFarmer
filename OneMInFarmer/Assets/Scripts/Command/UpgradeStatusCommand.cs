public class UpgradeStatusCommand : ICommand
{
    private Status statusToUpgrade;
    public UpgradeStatusCommand(Status statusToUpgrade)
    {
        this.statusToUpgrade = statusToUpgrade;
    }

    public void Execute()
    {
        if (!statusToUpgrade.IsReachMaxLevel)
        {
            Wallet playerWallet = Player.Instance.wallet;
            playerWallet?.LoseCoin(statusToUpgrade.GetUpgradeCost);
            statusToUpgrade.Upgrade();
        }
    }

    public void Undo()
    {
        Wallet playerWallet = Player.Instance.wallet;
        playerWallet.EarnCoin(statusToUpgrade.GetUpgradeCost);
        statusToUpgrade.Downgrade();
    }
}
