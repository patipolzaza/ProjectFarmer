using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInteractable { get; private set; }
    [SerializeField] protected UnityEvent<Player> interactEvent;
    protected SpriteRenderer sr;

    protected Color defaultColor;
    [SerializeField] protected Color highlightColor;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        isInteractable = true;
    }

    protected virtual void Start()
    {

    }

    public void SetInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
    }

    public virtual void Interact(Player interactor)
    {
        if (isInteractable)
        {
            interactEvent?.Invoke(interactor);
        }
    }

    public virtual void ShowObjectHighlight()
    {
        if (isInteractable)
        {
            sr.color = highlightColor;
        }
    }

    public virtual void HideObjectHighlight()
    {
        sr.color = defaultColor;
    }
}
