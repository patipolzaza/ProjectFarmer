using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UShopButtonInputManager : MonoBehaviour
{
    public static UShopButtonInputManager Instance { get; private set; }
    public Selectable currentSelected { get; private set; }

    [SerializeField] private Selectable startDayButton;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CheckButtonInput();
    }

    private void CheckButtonInput()
    {
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
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
            var selectables = FindObjectsOfType<Selectable>();
            int index = 0;
            foreach (var sb in selectables)
            {
                if (sb.GetComponent<UShopButtonInputNavigator>())
                {
                    UShopButtonInputNavigator buttonNavigator = sb.GetComponent<UShopButtonInputNavigator>();
                    if (buttonNavigator.IsStartButton)
                    {
                        currentSelected = sb;
                        sb.Select();
                        break;
                    }
                    else if (index == selectables.Length - 1 && !currentSelected)
                    {
                        currentSelected = sb;
                        sb.Select();
                    }
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

    public void ChangeSelectedButtonToStartButton()
    {
        ChangeSelectedButton(startDayButton);
    }

    public void SetCurrentButtonSelected(Selectable selectable)
    {
        currentSelected = selectable;
    }
}
