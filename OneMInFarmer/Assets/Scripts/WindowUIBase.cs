using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowUIBase : MonoBehaviour
{
    [SerializeField] protected GameObject windowUIObject;

    public virtual void ShowWindow()
    {
        windowUIObject.SetActive(true);
    }

    public virtual void HideWindow()
    {
        windowUIObject.SetActive(false);
    }
}
