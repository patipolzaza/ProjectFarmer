using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmQuitUI : MonoBehaviour
{
    [SerializeField] private GameObject _warningText;

    private void OnEnable()
    {
        if (GameManager.Instance.isSaveLoaded && !GameManager.Instance.isHaveMoreProgress)
        {
            SetActiveWarningText(true);
        }
        else
        {
            SetActiveWarningText(false);
        }
    }

    private void SetActiveWarningText(bool value)
    {
        _warningText.SetActive(value);
    }
}
