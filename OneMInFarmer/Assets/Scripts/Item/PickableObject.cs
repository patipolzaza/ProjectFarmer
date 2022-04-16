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

        if (transform.lossyScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        }

        if (transform.eulerAngles.magnitude != 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
    }

    public virtual void SetLocalPosition(Vector3 newLocalPosition, bool isIncludeColliderExtendX, bool isIncludeColliderExtendY, bool isIncludeOffsetX, bool isIncludeOffsetY)
    {
        float extendX = isIncludeColliderExtendX ? objectCollider.bounds.extents.x : 0;
        float extendY = isIncludeColliderExtendY ? objectCollider.bounds.extents.y : 0;

        float offsetX = isIncludeOffsetX ? objectCollider.offset.x : 0;
        float offsetY = isIncludeOffsetY ? objectCollider.offset.y : 0;

        Vector3 offset = new Vector3(extendX, extendY, 0) - new Vector3(offsetX, offsetY, 0);
        transform.localPosition = newLocalPosition + offset;
    }

    public virtual void SetParent(Transform newParent)
    {
        transform.parent = newParent;
    }
}
