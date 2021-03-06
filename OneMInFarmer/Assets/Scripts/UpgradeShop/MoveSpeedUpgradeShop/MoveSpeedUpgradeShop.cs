using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MoveSpeedUpgradeShop : MonoBehaviour
{
    private UpgradeShop upgradeShop;
    [SerializeField] private MoveSpeedUpgradeShopUI ui;
    private bool isSelectedTargetLevel;
    private int currentChosenLevel;

    private ICommand lastestCommand;

    private PercentStatus _statusToUpgrade;

    public UnityEvent OnUpgradedStatus;

    [SerializeField] private int[] targetUpgradeLevels = { 2, 4 };
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
            int buttonIndex = GetIndexFromLevel(currentChosenLevel);

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
        ICommand command = new UpgradeStatusToTargetLevelCommand(_statusToUpgrade, currentChosenLevel);
        if (command.Execute())
        {
            lastestCommand = command;
            isSelectedTargetLevel = true;
            OnUpgradedStatus?.Invoke();
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
        yield return new WaitUntil(() => Player.Instance);
        _statusToUpgrade = Player.Instance.moveSpeedStatus;
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
            if (i + 2 > _statusToUpgrade.GetMaxLevel)
            {
                break;
            }

            upgradeCosts[i] = _statusToUpgrade.GetUpgradeToTargetLevelCost(targetUpgradeLevels[i]);
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
            statusValue = _statusToUpgrade.GetPercentageUpgradeValueAtLevel(targetUpgradeLevels[i]);
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
        yield return new WaitUntil(() => _statusToUpgrade != null);
        //ui.SetAllUpgradeButtonsInteractable(false);
        int maxCost = _statusToUpgrade.GetUpgradeToTargetLevelCost(_statusToUpgrade.GetMaxLevel);

        yield return new WaitUntil(() => upgradeShop.playerCoinInMemory == Player.Instance.wallet.coin);
        int playerCoin = upgradeShop.playerCoinInMemory;

        if (maxCost <= playerCoin)
        {
            ui.SetAllUpgradeButtonsInteractable(true);
        }
        else
        {
            int cost;

            if (isSelectedTargetLevel)
            {
                playerCoin += upgradeCosts[GetIndexFromLevel(currentChosenLevel)];
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
                    if (i != GetIndexFromLevel(currentChosenLevel))
                    {
                        ui.SetUpgradeButtonInteractable(i, true);
                    }
                }
                else
                {
                    ui.SetUpgradeButtonInteractable(i, false);
                }
            }
        }

        isReadied = true;
    }

    private int GetIndexFromLevel(int level)
    {
        if (level > 0)
        {
            for (int i = 0; i < targetUpgradeLevels.Length; i++)
            {
                if (level == targetUpgradeLevels[i])
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public void ResetUpgrade()
    {
        lastestCommand?.Undo();
        if (isSelectedTargetLevel)
        {
            ChangeTargetLevel(GetIndexFromLevel(currentChosenLevel), 0);
        }
        isSelectedTargetLevel = false;
    }
}
