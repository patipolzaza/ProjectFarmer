using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUpgradeShopUI : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Text _statusName;
    [SerializeField] private Text _currentLevelText;
    [SerializeField] private Text _currentStatusValueText;
    [SerializeField] private Text _extraValueText;
    [SerializeField] private Text _upgradeCostText;

    private Wallet _playerWallet;
    private Status _status;

    private void OnEnable()
    {
        if (_playerWallet == null)
        {
            _playerWallet = FindObjectOfType<Player>().transform.Find("Wallet").GetComponent<Wallet>();
        }

        _playerWallet.OnCoinChanged += UpdateUpgradeButtonInteractable;
    }
    private void OnDisable()
    {
        _playerWallet.OnCoinChanged -= UpdateUpgradeButtonInteractable;
    }

    public void SetUpUI(Status status)
    {
        if (_status == null || _status != status)
        {
            _status = status;
        }

        SetStatusNameText(_status.statusName);
        SetExtraValueText("+ " + _status.GetExtraValuePerLevel.ToString());

        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        if (_status.IsReachMaxLevel)
        {
            SetActiveButton(false);
            SetUpgradeCostText("MAX");
            SetCurrentLevelText("MAX");
            SetExtraValueText("");
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
            SetCurrentLevelText(_status.currentLevel.ToString());
            SetCurrentValueText(_status.GetValue.ToString());

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

    private void UpdateUpgradeButtonInteractable(int oldPlayerCoin, int newPlayerCoin)
    {
        if (_status.IsReachMaxLevel)
        {
            SetButtonInteractable(false);
        }
        else
        {
            if (newPlayerCoin >= _status.GetUpgradeCost)
            {
                SetButtonInteractable(true);
            }
            else
            {
                SetButtonInteractable(false);
            }
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

    private void SetCurrentLevelText(string newText)
    {
        _currentLevelText.text = newText;
    }

    private void SetExtraValueText(string newText)
    {
        _extraValueText.text = newText;
    }

    private void SetCurrentValueText(string newText)
    {
        _currentStatusValueText.text = newText;
    }

    private void SetStatusNameText(string newText)
    {
        _statusName.text = newText;
    }
}
