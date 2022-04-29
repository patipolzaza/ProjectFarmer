using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UShopButtonInputManager : MonoBehaviour
{
    public static UShopButtonInputManager Instance { get; private set; }
    public Selectable currentSelected { get; private set; }

    [SerializeField] private List<Selectable> _startDayButtons = new List<Selectable>();
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!UpgradeShop.Instance || !UpgradeShop.Instance.isOpenedShop)
        {
            return;
        }

        CheckButtonInput();
    }

    private void CheckButtonInput()
    {
        if ((Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")))
        {
            UpdateButtonSelection();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            currentSelected?.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void UpdateButtonSelection()
    {
        if (currentSelected)
        {
            currentSelected.Select();
        }
        else
        {
            var buttonNavigators = FindObjectsOfType<UShopButtonInputNavigator>();
            int index = 0;
            foreach (var bn in buttonNavigators)
            {
                if (bn.IsStartButton && bn.GetButton.interactable)
                {
                    ChangeSelectedButton(bn.GetButton);
                    break;
                }
                else if (index == buttonNavigators.Length - 1 && !currentSelected)
                {
                    ChangeSelectedButtonToStartButton();
                }

                index++;
            }
        }
    }

    public void ChangeSelectedButton(Selectable selectable)
    {
        if (!selectable || !selectable.interactable)
        {
            return;
        }

        selectable.Select();
        SetCurrentButtonSelected(selectable);
    }

    public void DeselectCurrentButton()
    {
        currentSelected = null;

        var currentSelectedObject = EventSystem.current.currentSelectedGameObject;
        if (currentSelectedObject)
        {
            currentSelectedObject = null;
        }
    }

    public void ChangeSelectedButtonToStartButton()
    {
        Selectable target = null;
        foreach (var btn in _startDayButtons)
        {
            if (btn.transform.parent.gameObject.activeSelf && btn.interactable)
            {
                Debug.Log(btn.transform.parent.gameObject.activeSelf);
                Debug.Log(btn.interactable);

                target = btn;
                break;
            }
        }

        Debug.Log($"Change select button to start button({target})");
        ChangeSelectedButton(target);
    }

    public void SetCurrentButtonSelected(Selectable selectable)
    {
        currentSelected = selectable;
    }
}
