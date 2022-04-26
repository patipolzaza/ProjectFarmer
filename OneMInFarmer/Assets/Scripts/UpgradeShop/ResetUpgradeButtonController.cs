using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetUpgradeButtonController : MonoBehaviour
{
    [SerializeField] private Button _resetButton;

    private void Start()
    {
        HideButton();
    }

    public void ShowButton()
    {
        _resetButton.gameObject.SetActive(true);
    }

    public void HideButton()
    {
        _resetButton.gameObject.SetActive(false);
    }
}
