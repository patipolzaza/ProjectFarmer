using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUI : WindowUIBase
{
    public static InputUI Instance { get; private set; }
    [SerializeField] private GameObject _dropInputUI;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDropInputUI()
    {
        _dropInputUI.SetActive(true);
    }

    public void HideDropInputUI()
    {
        _dropInputUI.SetActive(false);
    }
}
