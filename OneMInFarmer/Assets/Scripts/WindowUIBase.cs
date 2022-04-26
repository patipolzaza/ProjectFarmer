using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowUIBase : MonoBehaviour
{
    [SerializeField] protected GameObject windowUIObject;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public virtual void ShowWindow()
    {
        windowUIObject.SetActive(true);
    }

    public virtual void HideWindow()
    {
        windowUIObject.SetActive(false);
    }
}
