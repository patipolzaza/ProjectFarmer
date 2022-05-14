using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IngameMenuUI : WindowUIBase
{
    public UnityEvent OnUIShowed;
    public UnityEvent OnUIHided;

    private bool _isShowed;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (_isShowed)
            {
                HideWindow();
            }
            else
            {
                if (!EventSystem.current.alreadySelecting)
                {
                    ShowWindow();
                }
            }
        }
    }

    public override void ShowWindow()
    {
        OnUIShowed?.Invoke();
        _isShowed = true;
        base.ShowWindow();
    }

    public override void HideWindow()
    {
        OnUIHided?.Invoke();
        _isShowed = false;
        base.HideWindow();
    }
}
