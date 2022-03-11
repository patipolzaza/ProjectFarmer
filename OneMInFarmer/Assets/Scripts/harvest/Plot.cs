using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : Interactable
{
    private bool isPlanted = false;
    [SerializeField] private SpriteRenderer plant;

    [SerializeField] Sprite SpriteDry;
    [SerializeField] Sprite SpriteWet;

    public Seed seed;
    int plantStage = 0;
    int countHarvest;
    int agePlant = 0;
    int dehydration = 0;
    bool isDry = true;
    bool isWither = false;

    protected override void Awake()
    {
        base.Awake();

        interactEvent.AddListener(PlayerInteract);
    }

    public void PlayerInteract(Player player)
    {
        if (isPlanted)
        {

            if (plantStage >= seed.plantStages.Length - 1)
            {
                Harvest(player);//return Harvest();
            }
            else if (isWither)
            {
                Uproot();
            }
            else if (player.holdingObject is WateringPot && player.holdingObject != null)
            {
                Debug.Log("U have WateringPot");
                player.holdingObject.GetComponent<WateringPot>().WateringOnPlot(this);
            }
        }
        else
        {
            if (player.holdingObject.GetComponent<ItemStack>().GetItemData() is Seed)
            {
                ItemStack holdingItem = player.holdingObject as ItemStack;
                Debug.Log("U have Seed");
                Plant(holdingItem);
            }
        }


    }

    void Harvest(Player player)
    {
        if (countHarvest > 1)
        {
            countHarvest--;
            plantStage--;
            UpdatePlant();
            player.SetHoldingItem(Instantiate(seed.product, new Vector3(0, 0, 0), Quaternion.identity));
            return;
        }
        else
        {
            player.SetHoldingItem(Instantiate(seed.product, new Vector3(0, 0, 0), Quaternion.identity));
            isPlanted = false;
            seed = null;
            plant.gameObject.SetActive(false);

        }
    }


    void Plant(ItemStack seedItem)
    {
        Debug.Log("Plant");
        if (this.seed == null)
        {
            this.seed = (Seed)seedItem.GetItemData();
            seedItem.UseItemStacks(1);
            isPlanted = true;
            plantStage = 0;
            agePlant = 0;
            countHarvest = seed.countHarvest;
            dehydration = 0;
            UpdatePlant();
            plant.gameObject.SetActive(true);
        }
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
            GetComponent<SpriteRenderer>().sprite = SpriteDry;
        }
        else
        {
            Debug.Log("Update not Dry");
            GetComponent<SpriteRenderer>().sprite = SpriteWet;
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
    public void ResetPlotStatus()
    {
        if (isPlanted)
        {
            Grow();
            Dring();
        }
    }
}
