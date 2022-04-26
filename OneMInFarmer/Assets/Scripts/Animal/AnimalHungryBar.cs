using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalHungryBar : MonoBehaviour
{
    [SerializeField] private Image hungryBar;
    private float maxHungry = 1;
    private float currentValue = 0;

    private Coroutine slideBarCoroutine;
    private Coroutine showBarCoroutine;

    private void Start()
    {
        SetBarFillAmount(0);
    }

    public void UpdateBar(float oldValue, float newValue)
    {
        if (slideBarCoroutine != null)
        {
            SetBarFillAmount(oldValue / maxHungry);
            StopCoroutine(slideBarCoroutine);
        }

        currentValue = oldValue;
        slideBarCoroutine = StartCoroutine(SlideUpdateBar(newValue));
    }

    private IEnumerator SlideUpdateBar(float target)
    {
        float currentBarValue = currentValue;

        while (currentValue != target)
        {
            currentBarValue = Mathf.Lerp(currentBarValue, target, 0.15f);
            SetBarFillAmount(currentBarValue / maxHungry);
            yield return new WaitForSeconds(0.01f);
        }

        slideBarCoroutine = null;
    }

    public void SetBarFillAmount(float newFillAmount)
    {
        hungryBar.fillAmount = newFillAmount;
    }

    public void ShowBar()
    {
        gameObject.SetActive(true);
    }

    public void HideBar()
    {
        if (showBarCoroutine != null) { return; }

        gameObject.SetActive(false);
    }

    public void ShowBarForWhile(float showingTime)
    {
        ShowBar();

        if (showBarCoroutine != null)
        {
            StopCoroutine(showBarCoroutine);
        }

        showBarCoroutine = StartCoroutine(HideBarAtTime(showingTime));
    }

    private IEnumerator HideBarAtTime(float showingTime)
    {
        yield return new WaitForSeconds(showingTime);
        showBarCoroutine = null;
        HideBar();
    }
}
