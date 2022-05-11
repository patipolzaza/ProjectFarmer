using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToggler : MonoBehaviour
{
    [SerializeField] private GameObject[] pairObjects;

    private void OnEnable()
    {
        HidePairObjects();
    }

    private void OnDisable()
    {
        ShowPairObjects();
    }

    private void ShowPairObjects()
    {
        foreach (var pair in pairObjects)
        {
            pair.gameObject.SetActive(true);
        }
    }
    private void HidePairObjects()
    {
        foreach (var pair in pairObjects)
        {
            pair.gameObject.SetActive(false);
        }
    }
}
