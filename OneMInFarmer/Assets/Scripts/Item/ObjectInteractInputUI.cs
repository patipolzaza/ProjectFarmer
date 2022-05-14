using UnityEngine;
using TMPro;

public class ObjectInteractInputUI : MonoBehaviour
{
    [SerializeField] private GameObject uiObject;

    [SerializeField] private TMP_Text _inputKeyText;
    [SerializeField] private string _inputKey;
    [SerializeField] private TMP_Text _inputCommandText;
    [SerializeField] private string _inputCommand;

    private bool _isSetted = false;
    public void ShowUI()
    {
        if (!_isSetted)
        {
            SetInputKeyText(_inputKey);
            SetInputCommandText(_inputCommand);
            _isSetted = true;
        }

        uiObject.SetActive(true);
    }

    public void HideUI()
    {
        uiObject.SetActive(false);
    }

    private void SetInputKeyText(string value)
    {
        _inputKeyText.text = value;
    }

    private void SetInputCommandText(string value)
    {
        _inputCommandText.text = value;
    }
}
