using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInteractable { get; protected set; }
    [SerializeField] protected UnityEvent<Player> interactEvent;
    [SerializeField] protected GameObject interactableObject;
    protected float objectDefaultScale;
    public Collider2D objectCollider { get; protected set; }
    public SpriteRenderer[] spriteRenderers { get; protected set; }

    protected Color defaultColor;
    protected Color highlightColor;

    [Header("On Highlight Events")]
    public UnityEvent OnHighlightShowed;
    public UnityEvent OnHighlightHided;


    protected virtual void Awake()
    {
        if (interactableObject.GetComponent<SpriteRenderer>())
        {
            SpriteRenderer sr = interactableObject.GetComponent<SpriteRenderer>();
            spriteRenderers = new SpriteRenderer[1];
            spriteRenderers[0] = sr;
            defaultColor = sr.color;
        }
        else if (interactableObject.GetComponent<SpriteRenderers>())
        {
            SpriteRenderers srs = interactableObject.GetComponent<SpriteRenderers>();
            spriteRenderers = new SpriteRenderer[srs.GetSpriteRenderers.Length];
            spriteRenderers = srs.GetSpriteRenderers;
            defaultColor = srs.GetDefaultColor;
        }

        objectCollider = GetComponent<Collider2D>();

        highlightColor = new Color32(255, 226, 0, 255);
        isInteractable = true;

        objectDefaultScale = Mathf.Abs(interactableObject.transform.localScale.x);
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
        defaultColor = spriteRenderers[0].color;

        if (isInteractable)
        {
            foreach (var sr in spriteRenderers)
            {
                sr.color = highlightColor;
            }
        }
        else
        {
            foreach (var sr in spriteRenderers)
            {
                sr.color = defaultColor;
            }
        }
        OnHighlightShowed?.Invoke();
    }

    public virtual void SetScale(Vector3 newScale)
    {
        Vector3 scale = new Vector3(objectDefaultScale * newScale.x, objectDefaultScale * newScale.y, 1);
        interactableObject.transform.localScale = scale;
    }

    public virtual void SetColor(Color newColor)
    {
        defaultColor = newColor;

        foreach (var sr in spriteRenderers)
        {
            sr.color = defaultColor;
        }
    }

    public virtual void HideObjectHighlight()
    {
        foreach (var sr in spriteRenderers)
        {
            sr.color = defaultColor;
        }

        OnHighlightHided?.Invoke();
    }

    public virtual void SetObjectSpriteRenderer(bool isRender)
    {
        foreach (var sr in spriteRenderers)
        {
            sr.enabled = isRender;
        }
    }
}
