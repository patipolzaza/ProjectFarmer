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
}
