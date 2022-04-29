using UnityEngine;
using UnityEngine.Events;

public class Hand : MonoBehaviour
{
    private PlayerAnimationController playerAC;
    public PickableObject holdingObject { get; private set; } = null;

    [SerializeField] private Transform handTransform;
    [Tooltip("The transform that is a where to drop the hoding object.")]
    [SerializeField] private Transform transformToDropObject;

    public UnityEvent<PickableObject> OnItemPickEvent;
    public UnityEvent OnItemDropEvent;

    private void OnEnable()
    {
        OnItemPickEvent.AddListener(ItemInHandUI.Instance.ShowUI);
    }

    private void OnDisable()
    {
        OnItemPickEvent.RemoveListener(ItemInHandUI.Instance.ShowUI);
    }

    private void Awake()
    {
        playerAC = transform.root.Find("PlayerCharacter").GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        playerAC.SetIsHoldItemBoolean(holdingObject != null);
    }

    public void PickUpObject(PickableObject pickableObject)
    {
        if (holdingObject)
        {
            DropObject();
        }

        pickableObject.Pick();

        pickableObject.SetParent(handTransform);
        pickableObject.SetLocalPosition(new Vector3(0, 0, 1), false, false, false, false);
        pickableObject.SetInteractable(false);
        pickableObject.HideObjectHighlight();

        holdingObject = pickableObject;

        OnItemPickEvent?.Invoke(pickableObject);
    }

    public void DropObject()
    {
        if (holdingObject)
        {
            holdingObject.SetParent(transformToDropObject);
            holdingObject.SetLocalPosition(new Vector3(0, 0, 1), false, true, true, true);
            holdingObject.Drop();

            holdingObject.SetInteractable(true);

            holdingObject = null;

            OnItemDropEvent?.Invoke();
        }
    }

    public void SetInHandItemToNull()
    {
        if (holdingObject)
        {
            DropObject();
        }

        holdingObject = null;
    }
}
