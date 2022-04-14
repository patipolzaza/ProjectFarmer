using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : WindowUIBase
{
    [SerializeField] private GameObject _StartGameTextObj;

    public void SetActiveStartGameText(bool isActive)
    {
        _StartGameTextObj.SetActive(isActive);
    }
}
