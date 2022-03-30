using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedUpgradeShop : MonoBehaviour
{
    private UpgradeShop upgradeShop;
    [SerializeField] private MoveSpeedUpgradeShopUI ui;
    private bool isSelectedTargetLevel;
    private int currentChosenLevel; //Level - 2 = button index on ui.

    private ICommand lastestCommand;

    private MoveSpeedStatus statusToUpgrade;

    [SerializeField] private int[] targetUpgradeLevels;
    private int[] upgradeCosts;
    public bool isReadied { get; private set; } = false;

    private void Awake()
    {
        StartCoroutine(InitialSetup());
    }

    public void SelectUpgradeLevelTarget(int targetLevel)
    {
        if (isSelectedTargetLevel)
        {
            lastestCommand.Undo();
            int buttonIndex = 0;
            for (int i = 0; i < targetUpgradeLevels.Length; i++)
            {
                if (targetUpgradeLevels[i] == currentChosenLevel)
                {
                    buttonIndex = i;
                }
            }

            ChangeTargetLevel(buttonIndex, targetLevel);
        }
        else
        {
            currentChosenLevel = targetLevel;
        }

        UpgradeStatus();
    }

    private void UpgradeStatus()
    {
        ICommand command = new UpgradeStatusToTargetLevelCommand(statusToUpgrade, currentChosenLevel);
        if (command.Execute())
        {
            lastestCommand = command;
            isSelectedTargetLevel = true;
        }
    }

    private void ChangeTargetLevel(int oldButtonIndex, int newLevel)
    {
        ui.ChangeUpgradeTarget(oldButtonIndex);

        currentChosenLevel = newLevel;
    }

    private IEnumerator InitialSetup()
    {
        upgradeShop = GetComponent<UpgradeShop>();
        yield return new WaitUntil(() => StatusUpgradeManager.Instance);
        statusToUpgrade = StatusUpgradeManager.Instance.moveSpeedStatus;
        yield return new WaitUntil(() => InitialUpgradeCostsSetup());
        yield return new WaitUntil(() => SetUpShopButtons());

        isReadied = true;
    }

    private bool InitialUpgradeCostsSetup()
    {
        int loopTimes = targetUpgradeLevels.Length;
        upgradeCosts = new int[loopTimes];

        for (int i = 0; i < loopTimes; i++)
        {
            //i+2 = target level according to button[i]
            if (i + 2 > statusToUpgrade.GetMaxLevel)
            {
                break;
            }

            upgradeCosts[i] = statusToUpgrade.GetUpgradeToTargetLevelCost(targetUpgradeLevels[i]);
        }

        return true;
    }

    private bool SetUpShopButtons()
    {
        int loopCount = ui.GetButtonLength;
        string buttonText;
        string belowButtonText;
        int statusValue;

        for (int i = 0; i < loopCount; i++)
        {
            statusValue = statusToUpgrade.GetPercentageUpgradeValueAtLevel(targetUpgradeLevels[i]);
            buttonText = $"+{statusValue}%";
            belowButtonText = $"Cost: {upgradeCosts[i]}";

            ui.SetUpgradeButtonText(i, buttonText, belowButtonText);

            int targetLevel = targetUpgradeLevels[i];
            ui.AddButtonAction(i, delegate { SelectUpgradeLevelTarget(targetLevel); });
        }

        return true;
    }

    public void UpdateShopButtons()
    {
        StartCoroutine(UpdateUpgradeButtonsInteractable());
    }

    private IEnumerator UpdateUpgradeButtonsInteractable()
    {
        yield return new WaitUntil(() => statusToUpgrade != null);
        ui.SetAllUpgradeButtonsInteractable(false);
        int maxCost = statusToUpgrade.GetUpgradeToTargetLevelCost(statusToUpgrade.GetMaxLevel);

        yield return new WaitUntil(() => upgradeShop.playerCoinInMemmory == Player.Instance.wallet.coin);
        int playerCoin = upgradeShop.playerCoinInMemmory;

        if (maxCost <= playerCoin)
        {
            ui.SetAllUpgradeButtonsInteractable(true);
        }
        else
        {
            int cost;

            if (isSelectedTargetLevel)
            {
                playerCoin += upgradeCosts[currentChosenLevel - 2];
            }

            for (int i = 0; i < ui.GetButtonLength; i++)
            {
                if (upgradeCosts.Length < i)
                {
                    break;
                }

                cost = upgradeCosts[i];
                yield return new WaitForEndOfFrame();
                if (playerCoin >= cost)
                {
                    if (i != currentChosenLevel - 2)
                    {
                        ui.SetUpgradeButtonInteractable(i, true);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        isReadied = true;
    }

    public void ResetUpgrade()
    {
        lastestCommand?.Undo();
        if (isSelectedTargetLevel)
        {
            ChangeTargetLevel(currentChosenLevel, 0);
        }
        isSelectedTargetLevel = false;
    }
}
