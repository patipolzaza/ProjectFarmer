using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationController : MonoBehaviour
{
    
    private Animator anim;
    private void OnValidate()
    {
        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }

    public void OnLanding()
    {

    }

    public void wateringAnimation()
    {
        anim.SetTrigger("WateringTrigger");
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Watering"))
        {
            Debug.Log("End Animation");
        }
    }

    public void runningAnimation()
    {
        anim.SetBool("isPickUp", true);
    }
    public void pickUpAnimation()
    {
        anim.SetTrigger("PickUpTrigger");
        StartCoroutine(pickUpAnimationPros());
    }
    private IEnumerator pickUpAnimationPros()
    {
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"));

        Debug.Log("End Animation");
    }
}
