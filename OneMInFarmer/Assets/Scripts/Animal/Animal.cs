using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : PickableObject, IValuable
{
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public float facingDirection { get; private set; }

    [SerializeField] protected AnimalData animalData;

    [SerializeField] private TextMesh textMesh;
    private Coroutine showTextCoroutine;

    private Vector2 velocityWorkspace = new Vector2();

    public int currentAge { get; protected set; } = 1;
    public int lifePoint { get; protected set; } = 2;

    public bool isHungry { get; private set; } = true;
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
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.InitializeState(idleState);

        facingDirection = interactableObject.transform.localScale.x / interactableObject.transform.localScale.x;

        float size = GetSize;
        Vector3 newScale = new Vector3(size, size, 1);
        SetScale(newScale);
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

    public void SetHungry(bool isHungry)
    {
        this.isHungry = isHungry;
    }

    public void IncreaseAge()
    {
        if (isDie)
        {
            return;
        }

        currentAge++;

        if (currentAge > animalData.lifespan)
        {
            DecreaseLifePoint();
        }

        currentAge = Mathf.Clamp(currentAge, 0, animalData.lifespan);

        float size = GetSize;
        Vector3 newScale = new Vector3(size, size, 1);
        SetScale(newScale);
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
        else if (food == null || !isHungry)
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

        showTextCoroutine = StartCoroutine(ShowText("Yummy :)"));
        isHungry = false;
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

    public void ResetAnimalHungryStatus()
    {
        if (!isHungry)
        {
            IncreaseAge();
        }
        else
        {
            DecreaseLifePoint();
        }

        isHungry = true;
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
        price = Mathf.Clamp(price, 0, animalData.sellPrice);

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

    public int GetPrice() => isDie ? 0 : Mathf.FloorToInt((float)animalData.sellPrice * ((float)currentAge / (float)animalData.lifespan));

    public void PutInShopStash(ShopForSell targetShop)
    {
        targetShop.PutItemInContainer(this);
        SetLocalPosition(Vector3.zero, false, false);
        SetObjectSpriteRenderer(false);
        SetInteractable(false);
    }

    public float GetSize => Mathf.Clamp(0.45f + (((float)currentAge / (float)animalData.lifespan) * 0.55f), 0.45f, 1);
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
