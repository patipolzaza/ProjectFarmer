using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public bool isPlanted = false;
    public SpriteRenderer plant;

    [SerializeField] WateringPot pot;

    public Seed seed;
    int plantStage = 0;
    int countHarvest;
    int agePlant = 0;
    int dehydration = 0;
    bool isDry = true;
    bool isWither = false;

    private void OnMouseDown()
    {
        if (isPlanted)
        {
            if (plantStage >= seed.plantStages.Length - 1)
            {
                Harvest();
            }
            else if (isWither)
            {
                Uproot();
            }
        }
        else
        {
            Plant();
        }
        pot.WateringOnPlot(this);
    }

    void Harvest()
    {
        if (countHarvest > 1)
        {
            countHarvest--;
            plantStage--;
            UpdatePlant();
            return;
        }
        else
        {
            isPlanted = false;
            seed = null;
            plant.gameObject.SetActive(false);

        }
    }

    void Plant()
    {
        isPlanted = true;
        plantStage = 0;
        agePlant = 0;
        countHarvest = seed.countHarvest;
        dehydration = 0;
        UpdatePlant();
        plant.gameObject.SetActive(true);
    }

    public void Watering()
    {
        isDry = false;
        UpdatePlant();
    }
    public void Dring()
    {
        isDry = true;
        UpdatePlant();
    }

    public void Grow()
    {
        if (!isDry)
        {
            dehydration = 0;
            agePlant++;
            if (plantStage >= seed.plantStages.Length - 1)
            {
                if (agePlant >= 1 + seed.plantStages.Length + seed.countHarvest)
                {
                    Wither();
                }
                return;
            }
            plantStage++;
            UpdatePlant();
        }
        else
        {
            dehydration++;

            if (dehydration >= 2)
            {
                Wither();
            }
        }


    }

    void UpdatePlant()
    {
        if (isDry)
        {
            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color32(102, 102, 102, 255);
        }
        if (isPlanted)
        {
            plant.sprite = seed.plantStages[plantStage];
            if (isWither)
            {
                plant.GetComponent<SpriteRenderer>().color = new Color32(188, 146, 0, 255);
            }
            else
            {
                plant.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            }
        }
    }

    void Wither()
    {
        isWither = true;
        UpdatePlant();
    }

    void Uproot()
    {
        seed = null;
        isPlanted = false;
        isWither = false;
        plant.gameObject.SetActive(false);
        plantStage = 0;
    }
}
