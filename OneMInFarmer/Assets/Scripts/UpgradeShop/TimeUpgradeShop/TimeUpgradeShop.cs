using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimeUpgradeShop : MonoBehaviour
{
    private UpgradeShop upgradeShop;
    [SerializeField] private TimeUpgradeShopUI ui;
    private bool isSelectedTargetLevel;
    private int currentChosenLevel; //Level - 2 = button index on ui.

    private ICommand lastestCommand;

    private Status statusToUpgrade;

    private int[] upgradeCosts;

    public UnityEvent OnUpgradedStatus;

    public bool isReadied { get; private set; } = false;

    private void Awake()
    {
        upgradeShop = GetComponent<UpgradeShop>();
        StartCoroutine(SetupShop());
    }

    IEnumerator SetupShop()
    {
        yield return new WaitUntil(() => Timer.Instance);
        statusToUpgrade = Timer.Instance.timeStatus;

        yield return new WaitUntil(() => InitialUpgradeCosts());
        yield return new WaitUntil(() => statusToUpgrade != null);
        yield return new WaitUntil(() => SetupShopButtons());
        //Wait until each function finished setup.

        isReadied = true;
    }

    private bool SetupShopButtons()
    {
        Status status = statusToUpgrade;
        int loopCount = ui.upgradeButtonsCount;

        for (int i = 0; i < loopCount; i++)
        {
            int statusLevel = i + 2;
            int index = i;
            string buttonText = $"+{status.GetValueAtLevel(statusLevel) - status.GetBaseValue}s";
            string costText = $"Cost: {upgradeCosts[index]}";
            ui.SetUpgradeButtonText(index, buttonText, costText);

            ui.AddButtonAction(index, delegate { SelectTargetLevel(statusLevel); });
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
    }

    private void UpgradeExtraTime()
    {
        ICommand command = new UpgradeStatusToTargetLevelCommand(statusToUpgrade, currentChosenLevel);
        if (command.Execute())
        {
            lastestCommand = command;
            OnUpgradedStatus?.Invoke();
            isSelectedTargetLevel = true;
        }
        else
        {
            ui.SetExtraTimeUpgradeButtonInteractable(currentChosenLevel - 2, true);
        }

    }

    private void ChangeTargetLevel(int oldLevel, int newLevel)
    {
        int oldButtonIndex = oldLevel - 2;

        ui.ChangeUpgradeTarget(oldButtonIndex);

        currentChosenLevel = newLevel;
    }

    public void ResetUpgrade()
    {
        if (lastestCommand != null)
        {
            lastestCommand.Undo();
            if (isSelectedTargetLevel)
            {
                ChangeTargetLevel(currentChosenLevel, 0);
            }
            isSelectedTargetLevel = false;
        }
    }

    public void UpdateShopUpgradeButtons()
    {
        StartCoroutine(UpdateUpgradeButtonsInteractable());
    }

    private IEnumerator UpdateUpgradeButtonsInteractable()
    {
        yield return new WaitUntil(() => statusToUpgrade != null);
        //ui.SetAllExtraTimeUpgradeButtonsInteractable(false);
        int maxCost = statusToUpgrade.GetUpgradeToTargetLevelCost(statusToUpgrade.GetMaxLevel);

        yield return new WaitUntil(() => upgradeShop.playerCoinInMemory == Player.Instance.wallet.coin);
        int playerCoin = upgradeShop.playerCoinInMemory;


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
                    ui.SetExtraTimeUpgradeButtonInteractable(i, false);
                }
            }
        }

        isReadied = true;
    }
}
