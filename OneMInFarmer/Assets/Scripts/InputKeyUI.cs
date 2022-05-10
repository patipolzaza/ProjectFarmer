using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputKeyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private string _inputKeyName;

    private void OnEnable()
    {
        SetKeyText(Input.GetButton(_inputKeyName).ToString().ToUpper());
    }

    private void SetKeyText(string inputKey)
    {
        keyText.text = inputKey;
    }
}
