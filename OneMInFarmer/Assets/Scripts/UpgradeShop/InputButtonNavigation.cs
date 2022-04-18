using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputButtonNavigation : MonoBehaviour
{
    private Button button;
    private bool isInteractableInMem;
    public UnityEvent OnInteractableOn;
    public UnityEvent OnInteractableOff;

    [SerializeField] private Button aboveButton;
    [SerializeField] private Button belowButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private void Awake()
    {
        button = GetComponent<Button>();

        isInteractableInMem = button.interactable;
    }

    private void Update()
    {
        if (!isInteractableInMem && button.interactable)
        {
            OnInteractableOn?.Invoke();
        }
        else if (isInteractableInMem && !button.interactable)
        {
            OnInteractableOff?.Invoke();
        }

        isInteractableInMem = button.interactable;
    }

    private void ChangeNavigationTop(Selectable selectable)
    {

    }
}
