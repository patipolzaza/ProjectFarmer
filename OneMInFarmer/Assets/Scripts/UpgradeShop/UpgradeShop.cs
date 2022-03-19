using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    public static UpgradeShop Instance;
    private UpgradeShopWindowUI ui;

    [Header("ExtraTime Upgrade")]
    [SerializeField] private int baseTimeCostPerSecond = 1;
    [SerializeField] private int minExtraTime = 0;
    [SerializeField] private int maxExtraTime = 30;
    private int currentExtraTime;
    private bool isPurchasedExtraTime = false;

    [Header("MoveSpeed Upgrade")]
    [SerializeField] private float buffMoveSpeedMultiplier = 0.25f;
    [SerializeField] private float moveSpeedBuffCost = 5;

    private void Awake()
    {
        Instance = this;
        ui = FindObjectOfType<UpgradeShopWindowUI>();
    }

    private void Start()
    {
        CloseWindow();
    }

    private void Update()
    {
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (ui.gameObject.activeSelf)
            {
                CloseWindow();
            }
            else
            {
                OpenWindow();
            }
        }
    }

    public void OpenWindow()
    {
        ui.ShowWindow();
        isPurchasedExtraTime = false;
        ui.SetTimeUpgradeButtonsInteractable(true);
        GameManager.Instance.SetTimeScale(0);
    }

    public void CloseWindow()
    {
        ui.HideWindow();
        GameManager.Instance.SetTimeScale(1);
        GameManager.Instance.StartDay();
    }

    private void UpdateUI()
    {
        ui.UpdatePlayerCoinText(GameManager.Instance.player.wallet.coin);
        ui.UpdateCurrentExtraTimeText(currentExtraTime);
        if (isPurchasedExtraTime)
        {
            ui.UpdateCurrentCostText("Purchased");
        }
        else
        {
            ui.UpdateCurrentCostText($"Cost: {GetPrice(currentExtraTime)}");
        }

        ui.UpdateTimeForNextDayText(GameManager.Instance.GetTimeForNextDayString);
    }

    private bool UpgradeTime(int extraTime)
    {
        var playerWallet = GameManager.Instance.player.wallet;

        if (playerWallet.coin >= GetPrice(extraTime))
        {
            int price = GetPrice(extraTime);
            playerWallet.LoseCoin(price);
            isPurchasedExtraTime = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BuyExtraTime()
    {
        if (UpgradeTime(currentExtraTime))
        {
            ui.SetTimeUpgradeButtonsInteractable(false);
        }
    }

    public void SetExtraTime(int extraTime)
    {
        int playerCoin = GameManager.Instance.player.wallet.coin;
        currentExtraTime = Mathf.Clamp(extraTime, minExtraTime, maxExtraTime);

        if (extraTime <= minExtraTime)
        {
            ui.SetDecreaseButtonInteractable(false);
        }
        else if (extraTime >= maxExtraTime || extraTime >= playerCoin)
        {
            ui.SetIncreaseButtonInteractable(false);
        }
        else
        {
            ui.SetDecreaseButtonInteractable(true);
            ui.SetIncreaseButtonInteractable(true);
        }

    }

    public void ResetUpgrade()
    {
        StatusUpgradeManager.Instance.UndoAll();
    }

    public int GetPrice(int extraTime)
    {
        int currentPrice = Mathf.CeilToInt(baseTimeCostPerSecond * (GameManager.Instance.currentDay * 0.35f)) * extraTime;
        return currentPrice;
    }
}
