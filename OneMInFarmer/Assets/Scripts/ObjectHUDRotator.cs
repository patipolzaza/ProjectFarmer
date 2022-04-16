using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHUDRotator : MonoBehaviour
{
    private Transform root;

    private void Awake()
    {
        root = transform.root;
    }

    private void Update()
    {
        if (transform.lossyScale.x < 0)
        {
            FlipHorizontal();
        }

        if (transform.lossyScale.y < 0)
        {
            FlipVertical();
        }

        if (transform.eulerAngles.magnitude != 0)
        {
            RotateToZero();
        }
    }

    private void FlipHorizontal()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
    }
    private void FlipVertical()
    {
        Debug.Log("Flip Vertical");
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, 1);
    }
    private void RotateToZero()
    {
        transform.eulerAngles = Vector3.zero;
    }
}
