using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UShopButtonInputNavigator : MonoBehaviour, ISelectHandler
{
    private Button button;
    private bool isInteractableInMem;
    [SerializeField] private bool isStartButton = false;
    [Space(15f)]
    public UnityAction OnInteractableOn;
    public UnityAction OnInteractableOff;
    public Button GetButton => button;
    public bool IsStartButton => isStartButton;

    private void Awake()
    {
        button = GetComponent<Button>();

        isInteractableInMem = button.interactable;
    }

    private void OnEnable()
    {
        OnInteractableOff += ChangeSelectTarget;
    }

    private void OnDisable()
    {
        OnInteractableOff -= ChangeSelectTarget;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (button.interactable)
        {
            UShopButtonInputManager.Instance.ChangeSelectedButton(button);
        }
        else
        {
            Selectable target = null;
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                target = FindInteractableUp();
                if (!target)
                {
                    target = FindInteractableLeft();
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                target = FindInteractableDown();
                if (!target)
                {
                    target = FindInteractableLeft();
                }
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                target = FindInteractableRight();
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                target = FindInteractableLeft();
            }

            UShopButtonInputManager.Instance.ChangeSelectedButton(target);
        }
    }

    private void LateUpdate()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (isInteractableInMem == false && button.interactable == true)
        {
            OnInteractableOn?.Invoke();
        }
        else if (isInteractableInMem == true && button.interactable == false)
        {
            OnInteractableOff?.Invoke();
        }

        isInteractableInMem = button.interactable;
    }

    private void ChangeSelectTarget()
    {
        if (!UShopButtonInputManager.Instance.currentSelected || !UShopButtonInputManager.Instance.currentSelected.Equals(button))
        {
            return;
        }

        if (button is DailyUpgradeShopButton)
        {
            Selectable target = FindInteractableLeft();

            if (!target)
            {
                if (FindInteractableRight())
                {
                    target = FindInteractableRight();
                }
                else if (FindInteractableUp())
                {
                    target = FindInteractableUp();
                }
                else if (FindInteractableDown())
                {
                    target = FindInteractableDown();
                }

                if (!target)
                {
                    UShopButtonInputManager.Instance.ChangeSelectedButtonToStartButton();
                }
                else
                {
                    UShopButtonInputManager.Instance.ChangeSelectedButton(target);
                }
            }
            else
            {
                UShopButtonInputManager.Instance.ChangeSelectedButton(target);
            }
        }
        else
        {
            if (FindInteractableUp())
            {
                UShopButtonInputManager.Instance.ChangeSelectedButton(FindInteractableUp());
            }
            else if (FindInteractableDown())
            {
                UShopButtonInputManager.Instance.ChangeSelectedButton(FindInteractableDown());
            }
            else
            {
                UShopButtonInputManager.Instance.ChangeSelectedButtonToStartButton();
            }
        }
    }

    private Selectable FindInteractableUp()
    {
        Selectable selectable = button;

        if (selectable.interactable)
        {
            return selectable;
        }
        else
        {
            Navigation navigation = button.navigation;
            int index = 0;
            while ((selectable && !selectable.interactable) && index <= 10)
            {
                navigation = selectable.navigation;

                if (selectable && !selectable.interactable)
                {
                    Selectable selectable2 = null;
                    Navigation navigation2 = navigation;
                    while (navigation2.selectOnLeft && (selectable && !selectable.interactable))
                    {
                        selectable2 = navigation2.selectOnLeft;

                        if (selectable2 && selectable2.interactable)
                        {
                            selectable = selectable2;
                        }
                        else
                        {
                            navigation2 = selectable2.navigation;
                        }
                    }

                    navigation2 = navigation;
                    while (navigation2.selectOnRight && (selectable && !selectable.interactable))
                    {
                        selectable2 = navigation2.selectOnRight;

                        if (selectable2 && selectable2.interactable)
                        {
                            selectable = selectable2;
                        }
                        else
                        {
                            navigation2 = selectable2.navigation;
                        }
                    }
                }

                if (selectable && !selectable.interactable)
                {
                    selectable = navigation.selectOnUp;
                }

                index++;
            }

            return selectable && selectable.interactable ? selectable : null;
        }
    }
    private Selectable FindInteractableDown()
    {
        Selectable selectable = button;

        if (selectable.interactable)
        {
            return selectable;
        }
        else
        {
            Navigation navigation = button.navigation;
            int index = 0;
            while ((selectable && !selectable.interactable) && index <= 10)
            {
                navigation = selectable.navigation;

                if (selectable && !selectable.interactable)
                {
                    Selectable selectable2 = null;
                    Navigation navigation2 = navigation;
                    while (navigation2.selectOnLeft && (selectable && !selectable.interactable))
                    {
                        selectable2 = navigation2.selectOnLeft;

                        if (selectable2 && selectable2.interactable)
                        {
                            selectable = selectable2;
                        }
                        else
                        {
                            navigation2 = selectable2.navigation;
                        }
                    }

                    navigation2 = navigation;
                    while ((selectable && !selectable.interactable) && navigation2.selectOnRight)
                    {
                        selectable2 = navigation2.selectOnRight;

                        if (selectable2 && selectable2.interactable)
                        {
                            selectable = selectable2;
                        }
                        else
                        {
                            navigation2 = selectable2.navigation;
                        }
                    }
                }

                if (selectable && !selectable.interactable)
                {
                    selectable = navigation.selectOnDown;
                }

                index++;
            }

            return selectable && selectable.interactable ? selectable : null;
        }
    }
    private Selectable FindInteractableLeft()
    {
        Selectable selectable = button;

        if (selectable.interactable)
        {
            return selectable;
        }
        else
        {
            Navigation navigation = button.navigation;
            while (navigation.selectOnLeft && (selectable && !selectable.interactable))
            {
                selectable = navigation.selectOnLeft;
                navigation = selectable.navigation;
            }

            return selectable && selectable.interactable ? selectable : null;
        }
    }
    private Selectable FindInteractableRight()
    {
        Selectable selectable = button;

        if (selectable.interactable)
        {
            return selectable;
        }
        else
        {
            Navigation navigation = button.navigation;
            while (navigation.selectOnRight && (selectable && !selectable.interactable))
            {
                selectable = navigation.selectOnRight;
                navigation = selectable.navigation;
            }

            return selectable && selectable.interactable ? selectable : null;
        }
    }
}
