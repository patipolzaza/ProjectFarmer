using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent interactEvent;
    private SpriteRenderer sr;

    private Color defaultColor;
    [SerializeField] private Color highlightColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = defaultColor;
    }

    public void Interact()
    {
        interactEvent?.Invoke();
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
