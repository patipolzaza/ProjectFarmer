using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInteractable { get; private set; }
    [SerializeField] protected UnityEvent<Player> interactEvent;
    public Collider2D objectCollider { get; protected set; }
    protected SpriteRenderer sr;

    protected Color defaultColor;
    [SerializeField] protected Color highlightColor;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        objectCollider = GetComponent<Collider2D>();

        defaultColor = sr.color;
        isInteractable = true;
    }

    protected virtual void Start()
    {

    }

    public void SetInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
        //objectCollider.enabled = isInteractable;
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
        else
        {
            sr.color = defaultColor;
        }
    }

    public virtual void HideObjectHighlight()
    {
        sr.color = defaultColor;
    }
}
