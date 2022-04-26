using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDayUI : MonoBehaviour
{
    public static EndDayUI Instance;

    [SerializeField] private GameObject uiObject;

    public bool isFinishedAnimation { get; private set; }

    private Animator animator;

    private void Awake()
    {
        Instance = this;
        animator = uiObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.isActiveAndEnabled && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Finish();
        }
    }

    public void Show()
    {
        isFinishedAnimation = false;
        uiObject.SetActive(true);
    }

    public void Finish()
    {
        isFinishedAnimation = true;
    }

    public void Hide()
    {
        isFinishedAnimation = false;
        uiObject.SetActive(false);
    }
}
