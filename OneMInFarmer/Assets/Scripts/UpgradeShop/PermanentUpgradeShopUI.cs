using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUpgradeShopUI : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Text _upgradeCostText;
    private Wallet _playerWallet;
    private Status _status;

    private void OnEnable()
    {
        if (_playerWallet == null)
        {
            _playerWallet = FindObjectOfType<Player>().wallet;
        }

        _playerWallet.OnCoinChanged += UpdateUpgradeButtonInteractable;
    }
    private void OnDisable()
    {
        if (_playerWallet == null)
        {
            _playerWallet = FindObjectOfType<Player>().wallet;
        }

        _playerWallet.OnCoinChanged -= UpdateUpgradeButtonInteractable;
    }

    public void SetUpUI(Status status)
    {
        if (_status == status)
        {
            return;
        }

        _status = status;
        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        if (_status.IsReachMaxLevel)
        {
            SetActiveButton(false);
            SetUpgradeCostText("MAX");
        }
        else
        {
            Player player = Player.Instance;

            if (player == null)
            {
                player = FindObjectOfType<Player>();
            }

            int upgradeCost = _status.GetUpgradeCost;

            SetUpgradeCostText(upgradeCost.ToString());

            if (player.wallet.coin >= upgradeCost)
            {
                SetButtonInteractable(true);
            }
            else
            {
                SetButtonInteractable(false);
            }
        }
    }

    private void UpdateUpgradeButtonInteractable(int oldValue, int newValue)
    {
        if (_status.IsReachMaxLevel)
        {
            SetButtonInteractable(false);
            return;
        }

        if (newValue >= _status.GetUpgradeCost)
        {
            SetButtonInteractable(true);
        }
        else
        {
            SetButtonInteractable(false);
        }
    }

    private void SetButtonInteractable(bool isInteractable)
    {
        _upgradeButton.interactable = isInteractable;
    }

    private void SetActiveButton(bool isActive)
    {
        _upgradeButton.gameObject.SetActive(isActive);
    }

    private void SetUpgradeCostText(string message)
    {
        _upgradeCostText.text = message;
    }
}
