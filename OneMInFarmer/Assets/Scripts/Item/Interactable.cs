using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent<Player> interactEvent;
    private SpriteRenderer sr;

    private Color defaultColor;
    [SerializeField] private Color highlightColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
    }

    public void Interact(Player interactor)
    {
        interactEvent?.Invoke(interactor);
    }

    public void ShowObjectHighlight()
    {
        sr.color = highlightColor;
    }

    public void HideObjectHighlight()
    {
        sr.color = defaultColor;
    }
}
