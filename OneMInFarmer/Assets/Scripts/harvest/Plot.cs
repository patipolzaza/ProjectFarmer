using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : Interactable
{
    private bool isPlanted = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject plantObject;
    private SpriteRenderer plantSpriteRenderer;
    [SerializeField] private GameObject lockedSign;

    [SerializeField] private Sprite SpriteDry;
    [SerializeField] private Sprite SpriteWet;

    public SeedData seed;
    int plantStage = 0;
    int countHarvest;
    int agePlant = 0;
    int dehydration = 0;
    bool isDry = true;
    bool isWither = false;

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();

        plantSpriteRenderer = plantObject.GetComponent<SpriteRenderer>();
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
            if (player.holdingObject)
            {
                Debug.Log("U have WateringPot123123");
                if (player.holdingObject.GetComponent<Item>().GetItemData is SeedData)
                {
                    Debug.Log("U have WateringPotasdasd");
                    Plant(player.holdingObject.GetComponent<Item>());
                    player.UseItem();
                }
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
            player.PickUpItem(Instantiate(seed.product, new Vector3(0, 0, 0), Quaternion.identity));
            return;
        }
        else
        {
            player.PickUpItem(Instantiate(seed.product, new Vector3(0, 0, 0), Quaternion.identity));
            isPlanted = false;
            seed = null;
            plantObject.SetActive(false);

        }
    }

    void Plant(Item seedItem)
    {
        Debug.Log("Plant");
        if (this.seed == null)
        {
            this.seed = (SeedData)seedItem.GetItemData;
            isPlanted = true;
            plantStage = 0;
            agePlant = 0;
            countHarvest = seed.countHarvest;
            dehydration = 0;
            UpdatePlant();
            plantObject.SetActive(true);
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
            plantSpriteRenderer.sprite = seed.plantStages[plantStage];
            if (isWither)
            {
                plantSpriteRenderer.color = new Color32(188, 146, 0, 255);
            }
            else
            {
                plantSpriteRenderer.color = new Color32(255, 255, 255, 255);
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
        plantObject.SetActive(false);
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

    public void Lock()
    {
        lockedSign.SetActive(true);
        spriteRenderer.color = new Color32(200, 200, 200, 255);
        SetInteractable(false);
    }

    public void Unlock()
    {
        lockedSign.SetActive(false);
        spriteRenderer.color = Color.white;
        SetInteractable(true);
    }
}
