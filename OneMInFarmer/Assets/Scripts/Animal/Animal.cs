using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : PickableObject
{
    [SerializeField] private AnimalData animalData;

    [SerializeField] private TextMesh textMesh;
    private Coroutine showTextCoroutine;

    public int currentAge { get; protected set; }

    public bool isHungry { get; private set; } = true;
    protected override void Awake()
    {
        base.Awake();

        //interactEvent.AddListener();
    }

    public void SetHungry(bool isHungry)
    {
        this.isHungry = isHungry;
    }

    public virtual bool TakeFood(AnimalFood food)
    {
        if (food == null || !isHungry)
        {
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
            }

            showTextCoroutine = StartCoroutine(ShowText("I'm NOT HUNGRY."));
            return false;
        }
        else if (!animalData.edibleFoods.Contains(food.GetFoodType))
        {
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
            }

            showTextCoroutine = StartCoroutine(ShowText("I Don't like this."));
            return false;
        }

        showTextCoroutine = StartCoroutine(ShowText("Yummy :)"));
        isHungry = false;
        return true;
    }

    public IEnumerator ShowText(string textToShow)
    {
        textMesh.text = textToShow;
        yield return new WaitForSeconds(3.5f);
        textMesh.text = "";
    }

    public void ResetAnimalStatus()
    {
        isHungry = true;
    }
}
