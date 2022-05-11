using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowUIBase : MonoBehaviour
{
    [SerializeField] protected GameObject windowUIObject;
    [SerializeField] private bool isHaveOwnEventSystem;
    private EventSystem _mainEventSystem;
    public virtual void ShowWindow()
    {
        if (isHaveOwnEventSystem)
        {
            if (!_mainEventSystem)
            {
                _mainEventSystem = EventSystem.current;
            }

            _mainEventSystem.gameObject.SetActive(false);
        }

        windowUIObject.SetActive(true);
    }

    public virtual void HideWindow()
    {
        if (_mainEventSystem)
        {
            _mainEventSystem.gameObject.SetActive(true);
        }

        windowUIObject.SetActive(false);
    }
}
