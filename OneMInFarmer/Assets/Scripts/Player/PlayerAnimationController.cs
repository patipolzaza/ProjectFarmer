using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public bool isPlayingAnimation { get; private set; }

    public void InteractAnimation()
    {
        string AnimationName = "Interact";
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

    public void SetRefillingAnimation(bool value)
    {
        string AnimationName = "Refilling";
        anim.SetBool(AnimationName, value);
    }  


    public void SetIsHoldItemBoolean(bool value)
    {
        anim.SetBool("IsHoldItem", value);
    }

    private void Update()
    {
        isPlayingAnimation = false;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp") || anim.GetCurrentAnimatorStateInfo(0).IsName("Interact") || anim.GetCurrentAnimatorStateInfo(0).IsName("Refilling"))
        {
            isPlayingAnimation = true;
        }
    }
}
