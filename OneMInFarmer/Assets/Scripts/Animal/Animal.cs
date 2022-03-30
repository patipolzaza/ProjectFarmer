using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : PickableObject
{
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public float facingDirection { get; private set; }

    [SerializeField] protected AnimalData animalData;

    [SerializeField] private TextMesh textMesh;
    private Coroutine showTextCoroutine;

    private Vector2 velocityWorkspace = new Vector2();

    public int currentAge { get; protected set; } = 1;

    public bool isHungry { get; private set; } = true;

    #region State Machine

    public StateMachine stateMachine { get; private set; }

    public IdleState idleState { get; private set; }
    [SerializeField] private IdleStateData idleStateData;
    public MoveState moveState { get; private set; }
    [SerializeField] private MoveStateData moveStateData;
    public GrabbedState grabbedState { get; private set; }
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
        //interactEvent.AddListener();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.InitializeState(idleState);

        facingDirection = interactableObject.transform.localScale.x / interactableObject.transform.localScale.x;
    }

    public void Update()
    {
        stateMachine.currentState.LogicUpdate();
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
        currentAge++;
    }

    public override Transform Pick(Player player)
    {
        base.Pick(player);
        stateMachine.ChangeState(grabbedState);

        return transform;
    }

    public override void Drop()
    {
        base.Drop();

        grabbedState.Unleash();
    }

    public virtual bool TakeFood(AnimalFood food)
    {
        if (food == null || !isHungry)
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

    public void ResetAnimalStatus()
    {
        if (!isHungry)
        {
            IncreaseAge();
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

    public void Sell()
    {
        Destroy(gameObject);

        Wallet playerWallet = Player.Instance.wallet;
        int price = Mathf.FloorToInt(animalData.sellPrice * (currentAge / animalData.lifespan));

        playerWallet.EarnCoin(price);
    }

    public void Buy()
    {
        Wallet playerWallet = Player.Instance.wallet;
        int price = animalData.buyPrice;

        playerWallet.LoseCoin(price);
    }
}
