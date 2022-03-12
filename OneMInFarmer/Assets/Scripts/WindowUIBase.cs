using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowUIBase : MonoBehaviour
{
    [SerializeField] protected GameObject windowUIObject;

    public void ShowWindow()
    {
        windowUIObject.SetActive(true);
    }

    public void CloseWindow()
    {
        windowUIObject.SetActive(false);
    }
}
