using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PermanentUpgradeShop : MonoBehaviour
{
    private Status _statusToUpgrade;

    [SerializeField] private GameObject _targetObjectContainStatus;
    private IContainStatus _componentContainStatus;

    public UnityEvent<Status> OnShopSetup;
    public UnityEvent OnUpgradedStatus;

    private void Awake()
    {
        _componentContainStatus = _targetObjectContainStatus.GetComponent<IContainStatus>();
        _statusToUpgrade = _componentContainStatus.GetStatus;

        OnShopSetup?.Invoke(_statusToUpgrade);
    }

    public void UpgradeStatus()
    {
        ICommand upgradeCommand = GetUpgradeCommand;

        if (StatusUpgradeManager.Instance.ExecuteCommand(upgradeCommand))
        {
            OnUpgradedStatus?.Invoke();
        }
    }

    protected virtual ICommand GetUpgradeCommand
    {
        get
        {
            return new UpgradeStatusCommand(_statusToUpgrade);
        }
    }
}
