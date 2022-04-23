using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeShop : MonoBehaviour
{
    public static UpgradeShop Instance { get; private set; }
    [SerializeField] private UpgradeShopWindowUI ui;
    private int currentPanelIndex = 0;
    public int playerCoinInMemmory { get; private set; }

    public TimeUpgradeShop extraTimeShop { get; private set; }
    public MoveSpeedUpgradeShop moveSpeedUpgradeShop { get; private set; }

    private bool isOpenedShop;

    public UnityEvent OnResetDailyUpgrade;
    public UnityEvent OnResetPermanentUpgrade;

    public bool isReadied { get; private set; } = false;

    private void Awake()
    {
        StartCoroutine(InitialSetUp());
    }

    private void Update()
    {
        if (!isOpenedShop)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangePanel(currentPanelIndex == 0 ? 1 : 0);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentPanelIndex == 0)
            {
                ResetDailyUpgrade();
            }
            else if (currentPanelIndex == 1)
            {
                ResetUpgrade();
            }
        }

        UpdateUI();

        if (isReadied && Player.Instance && playerCoinInMemmory != Player.Instance.wallet.coin)
        {
            playerCoinInMemmory = Player.Instance.wallet.coin;
            extraTimeShop.UpdateShopUpgradeButtons();
            moveSpeedUpgradeShop.UpdateShopButtons();
        }
    }

    private IEnumerator InitialSetUp()
    {
        Instance = this;
        extraTimeShop = GetComponent<TimeUpgradeShop>();
        moveSpeedUpgradeShop = GetComponent<MoveSpeedUpgradeShop>();

        yield return new WaitUntil(() => extraTimeShop.isReadied);
        yield return new WaitUntil(() => moveSpeedUpgradeShop.isReadied);

        ui.HideWindow();
        isReadied = true;
    }

    public void OpenWindow()
    {
        isOpenedShop = true;
        ChangePanel(0);
        ui.ShowWindow();
        UShopButtonInputManager.Instance.UpdateButtonSelection();
        GameManager.Instance.SetTimeScale(0);
    }

    public void CloseWindow()
    {
        isOpenedShop = false;
        ui.HideWindow();
        UShopButtonInputManager.Instance.SetCurrentButtonSelected(null);
        GameManager.Instance.SetTimeScale(1);
        GameManager.Instance.StartDay();
    }

    private void UpdateUI()
    {
        ui.UpdatePlayerCoinText(Player.Instance.wallet.coin);
    }

    public void ChangePanel(int newIndex)
    {
        ui.ChangePanel(currentPanelIndex, newIndex);
        currentPanelIndex = newIndex;
        UShopButtonInputManager.Instance.UpdateButtonSelection();
    }

    public void ResetDailyUpgrade()
    {
        extraTimeShop.ResetUpgrade();
        moveSpeedUpgradeShop.ResetUpgrade();

        OnResetDailyUpgrade?.Invoke();
    }

    public void ResetUpgrade()
    {
        StatusUpgradeManager.Instance.UndoAll();

        OnResetPermanentUpgrade?.Invoke();
    }
}
