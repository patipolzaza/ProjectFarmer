using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : PickableObject
{
    [SerializeField] public Rigidbody2D rb { get; private set; }
    [SerializeField] public Animator anim { get; private set; }

    [SerializeField] protected AnimalData animalData;

    [SerializeField] private TextMesh textMesh;
    private Coroutine showTextCoroutine;

    private Vector2 velocityWorkspace = new Vector2();

    public int currentAge { get; protected set; }

    public bool isHungry { get; private set; } = true;

    #region State Machine
    public StateMachine stateMachine { get; private set; }

    public IdleState idleState { get; private set; }
    [SerializeField] private IdleStateData idleStateData;
    public MoveState moveState { get; private set; }
    [SerializeField] private MoveStateData moveStateData;

    #endregion
    protected override void Awake()
    {
        base.Awake();

        anim = interactableObject.GetComponent<Animator>();
        rb = interactableObject.GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        moveState = new MoveState(this, stateMachine, "move", moveStateData);
        idleState = new IdleState(this, stateMachine, "idle", idleStateData);
        //interactEvent.AddListener();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.InitializeState(idleState);
    }

    public void Update()
    {
        stateMachine.currentState.LogicUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(moveState);
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
        currentAge++;
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

    public void SetVelocity(float x, float y)
    {
        velocityWorkspace.Set(x, y);

        rb.velocity = velocityWorkspace;
    }
}
