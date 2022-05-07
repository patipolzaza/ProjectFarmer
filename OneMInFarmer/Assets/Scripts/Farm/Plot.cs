using UnityEngine;

public class Plot : Interactable
{
    private PlotSaveData _saveData;

    private bool isPlanted = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject plantObject;
    private SpriteRenderer plantSpriteRenderer;
    [SerializeField] private GameObject lockedSign;

    [SerializeField] private Sprite SpriteDry;
    [SerializeField] private Sprite SpriteWet;

    public SeedData seed;
    public int plantStage { get; private set; } = 0;
    public int countHarvest { get; private set; }
    public int agePlant { get; private set; } = 0;
    public int dehydration { get; private set; } = 0;
    bool isDry = true;
    public bool isWither { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();

        plantSpriteRenderer = plantObject.GetComponent<SpriteRenderer>();
        interactEvent.AddListener(PlayerInteract);

        Lock();
    }

    private void OnEnable()
    {
        OnHighlightShowed.AddListener(ShowProductDetail);
        OnHighlightHided.AddListener(HideProductDetail);
    }

    private void OnDisable()
    {
        OnHighlightShowed.RemoveListener(ShowProductDetail);
        OnHighlightHided.RemoveListener(HideProductDetail);
    }

    protected override void Start()
    {
        base.Start();

        _saveData = new PlotSaveData(this);
        UpdateSaveDataOnContainer();
    }

    public void LoadSaveData(PlotSaveData saveData)
    {
        seed = saveData.GetSeed;
        isPlanted = seed == null ? false : true;

        plantStage = saveData.GetPlantStage;
        countHarvest = saveData.GetHarvestCount;
        agePlant = saveData.GetAgePlant;
        dehydration = saveData.GetDehydration;
        isWither = saveData.GetWitherStatus;

        _saveData = saveData;

        UpdatePlant();
    }

    public void PlayerInteract(Player player)
    {
        if (isPlanted)
        {
            if (player.playerHand.holdingObject)
            {
                return;
            }

            if (plantStage >= seed.plantStages.Length - 1)
            {
                Harvest(player);//return Harvest();
            }
            else if (isWither)
            {
                Uproot();
            }
            /*else if (player.holdingObject is WateringPot && player.holdingObject != null)
            {
                Debug.Log("U have WateringPot");
                player.holdingObject.GetComponent<WateringPot>().WateringOnPlot(this);
            }*/
        }
    }

    void Harvest(Player player)
    {
        if (countHarvest > 1)
        {
            countHarvest--;
            plantStage--;
            UpdatePlant();
            player.playerHand.PickUpObject(Instantiate(seed.product, new Vector3(0, 0, 0), Quaternion.identity));
            SoundEffectsController.Instance.PlaySoundEffect("Harvesting");
            return;
        }
        else
        {
            player.playerHand.PickUpObject(Instantiate(seed.product, new Vector3(0, 0, 0), Quaternion.identity));
            isPlanted = false;
            seed = null;
            SoundEffectsController.Instance.PlaySoundEffect("Harvesting");
            plantObject.SetActive(false);

        }
    }

    public bool Plant(SeedData seedData)
    {
        Debug.Log("Plant");
        if (isPlanted)
        {
            return false;
        }

        if (this.seed == null)
        {
            SoundEffectsController.Instance.PlaySoundEffect("Planting");
            this.seed = seedData;
            isPlanted = true;
            plantStage = 0;
            agePlant = 0;
            countHarvest = seed.countHarvest;
            dehydration = 0;
            UpdatePlant();
            plantObject.SetActive(true);
            return true;
        }
        else
        {
            return false;
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
            plantObject.SetActive(true);
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
        else
        {
            plantObject.SetActive(false);
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

    private void ShowProductDetail()
    {

        if (!seed || plantStage < seed.plantStages.Length - 1)
        {
            return;
        }

        seed.product.ShowDetail();
    }

    private void HideProductDetail()
    {
        seed?.product.HideDetail();
    }

    public void UpdateSaveDataOnContainer()
    {
        _saveData.UpdateData(this);
        _saveData.UpdateDataOnContainer();
    }
}
