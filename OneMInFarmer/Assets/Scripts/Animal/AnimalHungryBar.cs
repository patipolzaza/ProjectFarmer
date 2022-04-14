using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalHungryBar : MonoBehaviour
{
    [SerializeField] private Image hungryBar;
    private int maxHungry = 1;
    private int currentValue = 0;

    private Coroutine slideBarCoroutine;

    public void UpdateBar(int oldValue, int newValue)
    {
        if (slideBarCoroutine != null)
        {
            SetBarFillAmount(oldValue / maxHungry);
            StopCoroutine(slideBarCoroutine);
        }

        currentValue = oldValue;
        slideBarCoroutine = StartCoroutine(SlideUpdateBar(newValue));
    }

    private IEnumerator SlideUpdateBar(int target)
    {
        float currentBarValue = currentValue;

        while (currentValue != target)
        {
            currentBarValue = Mathf.Lerp(currentBarValue, target, 0.1f);
            SetBarFillAmount(currentBarValue / maxHungry);
            yield return new WaitForSeconds(0.01f);
        }

        slideBarCoroutine = null;
    }

    public void SetBarFillAmount(float newFillAmount)
    {
        hungryBar.fillAmount = newFillAmount;
    }
}
