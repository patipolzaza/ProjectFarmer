using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : Interactable
{

    protected override void Awake()
    {
        base.Awake();
    }

    public virtual Transform Pick()
    {
        return transform;
    }

    public virtual void Drop()
    {
        transform.parent = null;
    }

    public virtual void SetLocalPosition(Vector3 newLocalPosition, bool isIncludeColliderExtendX, bool isIncludeColliderExtendY)
    {
        float extendX = isIncludeColliderExtendX ? objectCollider.bounds.extents.x : 0;
        float extendY = isIncludeColliderExtendY ? objectCollider.bounds.extents.y : 0;

        float offsetX = objectCollider.offset.x;
        float offsetY = objectCollider.offset.y;

        Vector3 offset = new Vector3(extendX, extendY, 0) - new Vector3(offsetX, offsetY, 0);
        transform.localPosition = newLocalPosition + offset;
    }

    public virtual void SetParent(Transform newParent)
    {
        transform.parent = newParent;
    }
}
