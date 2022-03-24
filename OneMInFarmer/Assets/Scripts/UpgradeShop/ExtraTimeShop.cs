using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTimeShop : MonoBehaviour
{
    private UpgradeShop upgradeShop;
    [SerializeField] private ExtraTimeShopUI ui;
    private bool isSelectedTargetLevel;
    private int currentChosenLevel; //Level - 1 = button index on ui.

    private ICommand lastestCommand;

    private Status statusToUpgrade;
    public bool isReadied { get; private set; } = false;

    private void Awake()
    {
        upgradeShop = GetComponent<UpgradeShop>();
    }

    private void Start()
    {
        StartCoroutine(InitialSetUp());
    }

    private IEnumerator InitialSetUp()
    {
        yield return new WaitUntil(() => StatusUpgradeManager.Instance);
        statusToUpgrade = StatusUpgradeManager.Instance.extraTimeStatus;
        yield return new WaitUntil(() => Player.Instance);
        StartCoroutine(UpdateUpgradeButtonsInteractable());
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
        ui.SetExtraTimeUpgradeButtonInteractable(oldButtonIndex, true);

        currentChosenLevel = newLevel;
    }

    public void ResetUpgrade()
    {
        lastestCommand?.Undo();
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
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForFixedUpdate();
                cost = statusToUpgrade.GetUpgradeToTargetLevelCost(i + 2);
                if (playerCoin >= cost)
                {
                    ui.SetExtraTimeUpgradeButtonInteractable(i, true);
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
