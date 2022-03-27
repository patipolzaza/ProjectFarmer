using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTimeShop : MonoBehaviour
{
    private UpgradeShop upgradeShop;
    [SerializeField] private ExtraTimeShopUI ui;
    private bool isSelectedTargetLevel;
    private int currentChosenLevel; //Level - 2 = button index on ui.

    private ICommand lastestCommand;

    private Status statusToUpgrade;

    private int[] upgradeCosts;
    public bool isReadied { get; private set; } = false;

    private void Awake()
    {
        upgradeShop = GetComponent<UpgradeShop>();
        StartCoroutine(InitialSetUp());
    }

    private IEnumerator InitialSetUp()
    {
        yield return new WaitUntil(() => StatusUpgradeManager.Instance);
        statusToUpgrade = StatusUpgradeManager.Instance.extraTimeStatus;

        yield return new WaitUntil(() => InitialUpgradeCosts());
        yield return new WaitUntil(() => SetShopButtonTexts());
        //Wait until each function finished setup.

        isReadied = true;
    }

    private bool SetShopButtonTexts()
    {
        Status status = StatusUpgradeManager.Instance.extraTimeStatus;
        int statusLevel;

        for (int i = 0; i < ui.upgradeButtonsCount; i++)
        {
            statusLevel = i + 2;
            string buttonText = $"+{status.GetValueAtLevel(statusLevel)}s";
            string costText = $"Cost: {upgradeCosts[i]}";
            ui.SetUpgradeButtonText(i, buttonText, costText);
        }

        return true;
    }

    private bool InitialUpgradeCosts()
    {
        upgradeCosts = new int[statusToUpgrade.GetMaxLevel - 1];
        int loopTimes = upgradeCosts.Length;
        for (int i = 0; i < loopTimes; i++)
        {
            upgradeCosts[i] = statusToUpgrade.GetUpgradeToTargetLevelCost(i + 2);
        }

        return true;
    }

    public void SelectTargetLevel(int targetLevel)
    {
        if (isSelectedTargetLevel)
        {
            lastestCommand.Undo();
            ChangeTargetLevel(currentChosenLevel, targetLevel);
        }
        else
        {
            currentChosenLevel = targetLevel;
        }

        UpgradeExtraTime();
        isSelectedTargetLevel = true;
    }

    private void UpgradeExtraTime()
    {
        ICommand command = new UpgradeStatusToTargetLevelCommand(statusToUpgrade, currentChosenLevel);
        if (command.Execute())
        {
            lastestCommand = command;
        }
    }

    private void ChangeTargetLevel(int oldLevel, int newLevel)
    {
        int oldButtonIndex = oldLevel - 2;

        ui.ChangeUpgradeChosen(oldButtonIndex);

        currentChosenLevel = newLevel;
    }

    public void ResetUpgrade()
    {
        if (lastestCommand != null)
        {
            lastestCommand.Undo();
            ChangeTargetLevel(currentChosenLevel, 0);
            isSelectedTargetLevel = false;
        }
    }

    public IEnumerator UpdateUpgradeButtonsInteractable()
    {
        yield return new WaitUntil(() => statusToUpgrade != null);
        ui.SetAllExtraTimeUpgradeButtonsInteractable(false);
        int maxCost = statusToUpgrade.GetUpgradeToTargetLevelCost(statusToUpgrade.GetMaxLevel);

        yield return new WaitUntil(() => upgradeShop.playerCoinInMemmory == Player.Instance.wallet.coin);
        int playerCoin = upgradeShop.playerCoinInMemmory;


        if (maxCost <= playerCoin)
        {
            ui.SetAllExtraTimeUpgradeButtonsInteractable(true);
        }
        else
        {
            int cost;

            if (isSelectedTargetLevel)
            {
                playerCoin += upgradeCosts[currentChosenLevel - 2];
            }

            for (int i = 0; i < 3; i++)
            {
                cost = upgradeCosts[i];
                yield return new WaitForEndOfFrame();
                if (playerCoin >= cost)
                {
                    if (i != currentChosenLevel - 2)
                    {
                        ui.SetExtraTimeUpgradeButtonInteractable(i, true);
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
}
