using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class DayFloatingText : MonoBehaviour
{
    private Animator anim;
    private bool isFinishedPlayAnimation;
    private Text text;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
        text = GetComponent<Text>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        isFinishedPlayAnimation = false;
        anim.SetTrigger("floatText");
    }

    public void OnEndAnimation()
    {
        isFinishedPlayAnimation = true;
        anim.SetBool("isFinished", isFinishedPlayAnimation);
        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
