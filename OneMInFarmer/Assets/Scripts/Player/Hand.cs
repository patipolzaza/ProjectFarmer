using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public PickableObject holdingObject { get; private set; } = null;

    [SerializeField] private Transform handTransform;
    [Tooltip("The transform that is a where to drop the hoding object.")]
    [SerializeField] private Transform transformToDropObject;

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
        }
    }
}
