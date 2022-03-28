using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShop : MonoBehaviour
{
    public static UpgradeShop Instance { get; private set; }
    [SerializeField] private UpgradeShopWindowUI ui;
    private int currentPanelIndex = 0;
    public int playerCoinInMemmory { get; private set; }

    public ExtraTimeShop extraTimeShop { get; private set; }
    public MoveSpeedUpgradeShop moveSpeedUpgradeShop { get; private set; }

    public bool isReadied { get; private set; } = false;

    private void Awake()
    {
        StartCoroutine(InitialSetUp());
    }

    private void Update()
    {
        UpdateUI();

        if (isReadied && Player.Instance && playerCoinInMemmory != Player.Instance.wallet.coin)
        {
            playerCoinInMemmory = Player.Instance.wallet.coin;
            extraTimeShop.UpdateShopUpgradeButtons();
            moveSpeedUpgradeShop.UpdateShopButtons();
        }

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

    private IEnumerator InitialSetUp()
    {
        Instance = this;
        extraTimeShop = GetComponent<ExtraTimeShop>();
        moveSpeedUpgradeShop = GetComponent<MoveSpeedUpgradeShop>();
        ChangePanel(0);

        yield return new WaitUntil(() => extraTimeShop.isReadied);
        yield return new WaitUntil(() => moveSpeedUpgradeShop.isReadied);

        isReadied = true;
    }

    public void OpenWindow()
    {
        ui.ShowWindow();
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
        ui.UpdatePlayerCoinText(Player.Instance.wallet.coin);
    }

    public void ChangePanel(int newIndex)
    {
        ui.ChangePanel(currentPanelIndex, newIndex);
        currentPanelIndex = newIndex;
    }

    public void ResetUpgrade()
    {
        StatusUpgradeManager.Instance.UndoAll();
        extraTimeShop.ResetUpgrade();
        moveSpeedUpgradeShop.ResetUpgrade();
    }
}
