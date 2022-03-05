using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected UnityEvent<Player> interactEvent;
    protected SpriteRenderer sr;

    protected Color defaultColor;
    [SerializeField] protected Color highlightColor;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
    }

    protected virtual void Start()
    {

    }

    public virtual void Interact(Player interactor)
    {
        interactEvent?.Invoke(interactor);
    }

    public virtual void ShowObjectHighlight()
    {
        sr.color = highlightColor;
    }

    public virtual void HideObjectHighlight()
    {
        sr.color = defaultColor;
    }
}
