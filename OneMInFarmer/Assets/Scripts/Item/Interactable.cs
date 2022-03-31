using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInteractable { get; protected set; }
    [SerializeField] protected UnityEvent<Player> interactEvent;
    [SerializeField] protected GameObject interactableObject;
    public Collider2D objectCollider { get; protected set; }
    public SpriteRenderer sr { get; protected set; }

    protected Color defaultColor;
    [SerializeField] protected Color highlightColor;

    protected virtual void Awake()
    {
        sr = interactableObject.GetComponent<SpriteRenderer>();
        objectCollider = GetComponent<Collider2D>();

        highlightColor = new Color32(255, 226, 0, 255);
        defaultColor = sr.color;
        isInteractable = true;
    }

    protected virtual void Start()
    {

    }

    public void SetInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
        objectCollider.enabled = isInteractable;
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

    public virtual void SetObjectSpriteRenderer(bool isRender)
    {
        sr.enabled = isRender;
    }
}
