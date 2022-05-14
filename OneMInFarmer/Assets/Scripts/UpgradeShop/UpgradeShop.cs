using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeShop : MonoBehaviour
{
    public static UpgradeShop Instance { get; private set; }
    [SerializeField] private UpgradeShopWindowUI ui;
    private int currentPanelIndex = 0;
    public int playerCoinInMemory { get; private set; }

    public TimeUpgradeShop extraTimeShop { get; private set; }
    public MoveSpeedUpgradeShop moveSpeedUpgradeShop { get; private set; }

    public bool isOpenedShop { get; private set; }

    public UnityEvent OnResetDailyUpgrade;
    public UnityEvent OnResetPermanentUpgrade;
    public UnityEvent OnPanelChanged;

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

        playerCoinInMemory = Player.Instance.wallet.coin;
        extraTimeShop.UpdateShopUpgradeButtons();
        moveSpeedUpgradeShop.UpdateShopButtons();
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
        ui.ShowWindow();
        ChangePanel(0);
        UShopButtonInputManager.Instance.UpdateButtonSelection();
    }

    public void CloseWindow()
    {
        isOpenedShop = false;
        ui.HideWindow();
        UShopButtonInputManager.Instance.DeselectCurrentButton();
    }

    private void UpdateUI()
    {
        ui.UpdatePlayerCoinText(Player.Instance.wallet.coin);
    }

    public void ChangePanel(int newIndex)
    {
        ui.ChangePanel(currentPanelIndex, newIndex);
        currentPanelIndex = newIndex;

        OnPanelChanged?.Invoke();
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
