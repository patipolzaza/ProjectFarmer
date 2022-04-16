using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationController : MonoBehaviour
{
    public static PlayerAnimationController Instance { get; private set; }
    private Animator anim;
    public bool isFinishedProcess { get; private set; }
    private void OnValidate()
    {
        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    public void wateringAnimation()
    {
        string AnimationName = "Watering";
        anim.SetTrigger(AnimationName);
    }

    public void SetRunningAnimation(Vector2 vector2)
    {
        anim.SetFloat("Horizontal", Mathf.Abs(vector2.x));
        anim.SetFloat("Vertical", Mathf.Abs(vector2.y));
        anim.SetFloat("Magnitude", Mathf.Abs(vector2.magnitude));
    }

    public void pickUpAnimation()
    {
        string AnimationName = "PickUp";
        anim.SetTrigger(AnimationName);
    }

    public void RefillingAnimation()
    {
        anim.SetBool("Refilling", true);
    }


    private void Update()
    {
        isFinishedProcess = false;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp")|| anim.GetCurrentAnimatorStateInfo(0).IsName("Watering") || anim.GetCurrentAnimatorStateInfo(0).IsName("Refilling"))
        {
            isFinishedProcess = true;
        }
    }
}
