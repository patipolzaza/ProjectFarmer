using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PermanentUpgradeShop : MonoBehaviour
{
    protected Status _statusToUpgrade;

    [SerializeField] protected GameObject _targetObjectContainStatus;
    protected IContainStatus _componentContainStatus;

    public UnityEvent<Status> OnShopSetup;
    public UnityEvent OnUpgradedStatus;

    protected void OnValidate()
    {
        if (_targetObjectContainStatus && _targetObjectContainStatus.GetComponent<IContainStatus>() == null)
        {
            _targetObjectContainStatus = null;
        }
    }

    private void OnEnable()
    {
        OnShopSetup?.Invoke(_statusToUpgrade);
    }

    protected void Awake()
    {
        _componentContainStatus = _targetObjectContainStatus.GetComponent<IContainStatus>();
        _statusToUpgrade = _componentContainStatus.GetStatus;
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
