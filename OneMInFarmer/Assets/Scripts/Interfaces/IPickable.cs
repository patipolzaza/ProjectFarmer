using UnityEngine;
public interface IPickable
{
    public Transform Pick(Player picker);
    public void Drop();
    public void SetLocalPosition(Vector3 newLocalPosition, bool isIncludeColliderExtendX, bool isIncludeColliderExtendY);
    public void SetParent(Transform newParent);
}
