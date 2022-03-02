using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public bool isPlanted = false;
    public SpriteRenderer plant;

    public Seed seed;
    int plantStage = 0;
    int countHarvest;
    int agePlant = 0;

    private void OnMouseDown()
    {
        Debug.Log("Click");

        if (isPlanted)
        {
            if (plantStage >= seed.plantStages.Length - 1)
            {
                Harvest();
            }
        }
        else
        {
            Plant();
        }
    }

    void Harvest()
    {
        Debug.Log("Harvest");
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
        Debug.Log("Plant");
        isPlanted = true;
        plantStage = 0;
        agePlant = 0;
        countHarvest = seed.countHarvest;
        UpdatePlant();
        plant.gameObject.SetActive(true);
    }
    public void Grow()
    {
        Debug.Log("Grow");
        agePlant++;
        if (plantStage >= seed.plantStages.Length-1)
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

    void UpdatePlant()
    {
        Debug.Log("UpdatePlant");
        plant.sprite = seed.plantStages[plantStage];
    }

    void Wither()
    {
        Debug.Log("Wither");
        seed = null;
        isPlanted = false;
        plant.gameObject.SetActive(false);
        plantStage = 0;
    }
}
