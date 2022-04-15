using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Animal : PickableObject, IValuable
{
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public float facingDirection { get; private set; }

    [SerializeField] protected AnimalData animalData;

    [SerializeField] private TextMesh textMesh;
    private Coroutine showTextCoroutine;

    private Vector2 velocityWorkspace = new Vector2();

    public int age { get; protected set; } = 1;
    public AgeSpan currentAgeSpan;
    public int lifePoint { get; protected set; } = 2;

    public bool isHungry { get; private set; } = true;
    private List<AnimalFood> foodsEaten = new List<AnimalFood>();
    public float weight { get; protected set; }
    public UnityEvent<float, float> OnEatenFood;

    public bool isDie { get; private set; } = false;

    #region State Machine

    public StateMachine stateMachine { get; private set; }

    public IdleState idleState { get; private set; }
    [SerializeField] private IdleStateData idleStateData;
    public MoveState moveState { get; private set; }
    [SerializeField] private MoveStateData moveStateData;
    public GrabbedState grabbedState { get; private set; }
    public DieState dieState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        anim = interactableObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        moveState = new MoveState(this, stateMachine, "move", moveStateData);
        idleState = new IdleState(this, stateMachine, "idle", idleStateData);
        grabbedState = new GrabbedState(this, stateMachine, "grabbed");
        dieState = new DieState(this, stateMachine, "die");

        stateMachine.InitializeState(idleState);
        currentAgeSpan = 0;
    }

    protected override void Start()
    {
        base.Start();

        facingDirection = interactableObject.transform.localScale.x / interactableObject.transform.localScale.x;

        float size = GetSize;
        Vector3 newScale = new Vector3(size, size, 1);
        SetScale(newScale);

        weight = animalData.startWeight;
    }

    public void Update()
    {
        stateMachine.currentState.LogicUpdate();

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            IncreaseAge();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            DecreaseLifePoint();
        }
    }

    public void FixedUpdate()
    {
        stateMachine.currentState.PhysicUpdate();
    }

    public string GetAnimalName => animalData.animalName;
    public Sprite GetAnimalShopIcon => animalData.inShopIcon;
    public int GetSellPricePerKilo => animalData.sellPricePerKilo;
    public int GetPurchasePrice => animalData.purchasePrice;

    public void IncreaseAge()
    {
        if (isDie) return;

        if (foodsEaten.Count == 0)
            DecreaseLifePoint();
        else
            DigestFoods();

        age++;
        currentAgeSpan = (AgeSpan)(Mathf.Clamp(Mathf.CeilToInt(age / (animalData.lifespan / 3f) - 1), 0, 2));

        if (age > animalData.lifespan)
            DecreaseLifePoint();

        float size = GetSize;
        Vector3 newScale = new Vector3(size, size, 1);
        SetScale(newScale);
    }

    private void DigestFoods()
    {
        foreach (var food in foodsEaten)
        {
            IncreaseWeight(food.GetWeightGain);
        }

        foodsEaten.Clear();
    }

    private void IncreaseWeight(float amount)
    {
        weight += amount;
    }

    public void DecreaseLifePoint()
    {
        if (isDie)
        {
            return;
        }

        lifePoint--;
        lifePoint = Mathf.Clamp(lifePoint, 0, 2);

        if (lifePoint <= 0)
        {
            stateMachine.ChangeState(dieState);
            isDie = true;
            return;
        }

        Color newColor = defaultColor - new Color32(75, 75, 75, 0);
        SetColor(newColor);
    }

    public override Transform Pick(Player player)
    {
        base.Pick(player);

        if (!isDie)
        {
            stateMachine.ChangeState(grabbedState);
        }

        return transform;
    }

    public override void Drop()
    {
        base.Drop();

        grabbedState.Unleash();
    }

    public virtual bool TakeFood(AnimalFood food)
    {
        if (isDie)
        {
            return false;
        }
        else if (food == null || foodsEaten.Count >= animalData.stomachSize)
        {
            if (showTextCoroutine != null)
            {
                StopMessageCoroutine();
            }

            showTextCoroutine = StartCoroutine(ShowText("I'm NOT HUNGRY."));
            return false;
        }
        else if (!animalData.edibleFoods.Contains(food.GetFoodType))
        {
            if (showTextCoroutine != null)
            {
                StopMessageCoroutine();
            }

            showTextCoroutine = StartCoroutine(ShowText("I Don't like this."));
            return false;
        }

        float oldFoodRatio = foodsEaten.Count / (float)animalData.stomachSize;
        foodsEaten.Add(food);
        showTextCoroutine = StartCoroutine(ShowText("Yummy :)"));
        float newFoodRatio = foodsEaten.Count / (float)animalData.stomachSize;
        OnEatenFood?.Invoke(oldFoodRatio, newFoodRatio);
        return true;
    }

    private void StopMessageCoroutine()
    {
        textMesh.text = "";
        StopCoroutine(showTextCoroutine);
    }

    public IEnumerator ShowText(string textToShow)
    {
        textMesh.text = textToShow;
        yield return new WaitForSeconds(3.5f);
        textMesh.text = "";
    }

    public void SetVelocity(float x, float y)
    {
        if (x > 0 && facingDirection < 0)
        {
            Flip();
        }
        else if (x < 0 && facingDirection > 0)
        {
            Flip();
        }

        velocityWorkspace.Set(x, y);

        rb.velocity = velocityWorkspace;
    }

    public void Flip()
    {
        interactableObject.transform.localScale = new Vector3(-interactableObject.transform.localScale.x, interactableObject.transform.localScale.y, 1);
        facingDirection *= -1;
    }

    public int Sell()
    {
        Wallet playerWallet = Player.Instance.wallet;
        int price = GetPrice();
        //price = Mathf.Clamp(price, 0, animalData.sellPrice);

        playerWallet.EarnCoin(price);
        Destroy(gameObject);

        return price;
    }

    public bool Purchase()
    {
        Wallet playerWallet = Player.Instance.wallet;
        int price = animalData.purchasePrice;

        if (playerWallet.coin >= price)
        {
            playerWallet.LoseCoin(price);
            return true;
        }

        return false;
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

    public int GetPrice()
    {
        int price = 0;

        if (!isDie)
        {
            price = Mathf.FloorToInt(animalData.sellPricePerKilo * weight);
            if (currentAgeSpan == (AgeSpan)1)
                price += animalData.bonusSellPriceForAdult;
            else if (currentAgeSpan == (AgeSpan)2)
                price += animalData.bonusSellPriceForAdult + animalData.bonusSellPriceForElder;
        }

        return price;
    }

    public void PutInShopStash(ShopForSell targetShop)
    {
        targetShop.PutItemInContainer(this);
        SetLocalPosition(Vector3.zero, false, false);
        SetObjectSpriteRenderer(false);
        SetInteractable(false);
    }

    public float GetSize => Mathf.Clamp(0.45f + (((float)age / (float)animalData.lifespan) * 0.55f), 0.45f, 1);
    public void SetScale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }
    public void SetColor(Color newColor)
    {
        defaultColor = newColor;
        sr.color = defaultColor;
    }
}
