using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using TMPro;

public class Animal : PickableObject, IBuyable, ISellable
{
  private AnimalSaveData _saveData;
  public string prefabPath { get; private set; } = "";

  public Rigidbody2D rb { get; private set; }
  public Animator anim { get; private set; }

  public float facingDirection { get; private set; }

  [SerializeField] protected AnimalData animalData;

  [SerializeField] private TMP_Text messageText;
  private Coroutine showTextCoroutine;

  private Vector2 velocityWorkspace = new Vector2();

  public int age { get; protected set; } = 1;
  public AgeSpan currentAgeSpan { get; protected set; }
  public int lifePoint { get; protected set; } = 2;
  public bool isHungry { get; private set; } = true;
  private List<Food> foodsEaten = new List<Food>();
  public float weight { get; protected set; }

  public UnityEvent<float, float> OnEatenFood;
  public UnityEvent OnClearEatenFoods;
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

  public string GetAnimalName => animalData.animalName;
  public Sprite GetIcon => animalData.inShopIcon;
  public int GetSellPricePerKilo => animalData.sellPricePerKilo;
  public int GetBuyPrice => animalData.purchasePrice;
  public float GetSize => Mathf.Clamp(0.45f + (((float)age / (float)animalData.lifespan) * 0.4f), 0.6f, 1);
  public int GetSellPrice
  {
    get
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
  }
  public int GetLifespan => animalData.lifespan;
  public List<FoodType> GetEdibleFoods => animalData.edibleFoods;
  public string GetPrefabPath
  {
    get
    {
      return animalData.prefabPath;
    }
  }

  public void LoadAnimalData(AnimalSaveData saveData)
  {
    interactableObject.transform.localScale = saveData.GetAnimalScale;
    age = saveData.GetAge;
    weight = saveData.GetWeight;
    lifePoint = saveData.GetLifePoint;
    currentAgeSpan = (AgeSpan)saveData.GetAgeSpan;
    prefabPath = saveData.GetAnimalPrefabPath;

    //_saveData = new AnimalSaveData(saveData);
    _saveData = saveData;
  }

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

  private void OnEnable()
  {
    OnHighlightShowed.AddListener(ShowDetail);
    OnHighlightHided.AddListener(HideDetail);
  }

  private void OnDisable()
  {
    OnHighlightShowed.RemoveListener(ShowDetail);
    OnHighlightHided.RemoveListener(HideDetail);
  }

  protected override void Start()
  {
    base.Start();

    facingDirection = interactableObject.transform.localScale.x > 0 ? 1 : -1;

    if (_saveData == null)
    {
      float size = GetSize;
      Vector3 newScale = new Vector3(size, size, 1);
      SetScale(newScale);

      weight = animalData.startWeight;

      prefabPath = GetPrefabPath;

      _saveData = new AnimalSaveData(this);
    }

    UpdateDataOnContainer();
  }

  protected override void Update()
  {
    base.Update();
    stateMachine.currentState.LogicUpdate();
  }

  public void FixedUpdate()
  {
    stateMachine.currentState.PhysicUpdate();
  }


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

    UpdateDataOnContainer();
  }

  private void DigestFoods()
  {
    foreach (var food in foodsEaten)
    {
      IncreaseWeight(food.GetWeightGain);
    }

    OnClearEatenFoods?.Invoke();
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

  public void ClearEatenFoods()
  {
    foodsEaten.Clear();
  }

  public override Transform Pick()
  {
    base.Pick();

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

  public void SetWeight(int value)
  {
    weight = Mathf.Clamp(value, animalData.startWeight, float.PositiveInfinity);
  }

  public virtual bool TakeFood(Food food)
  {
    if (food == null || isDie)
    {
      return false;
    }
    else if (foodsEaten.Count >= animalData.stomachSize)
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
    SoundEffectsController.Instance.PlaySoundEffect("AnimalEating");
    float newFoodRatio = foodsEaten.Count / (float)animalData.stomachSize;
    OnEatenFood?.Invoke(oldFoodRatio, newFoodRatio);
    return true;
  }

  private void StopMessageCoroutine()
  {
    messageText.text = "";
    StopCoroutine(showTextCoroutine);
  }

  public IEnumerator ShowText(string textToShow)
  {
    messageText.text = textToShow;
    yield return new WaitForSeconds(2f);
    messageText.text = "";
  }

  public void SetVelocity(float x, float y)
  {
    FacingToDirection(x);

    velocityWorkspace.Set(x, y);
    rb.velocity = velocityWorkspace;
  }

  private void FacingToDirection(float velocityX)
  {
    int direction = (int)(velocityX > 0 ? 1 : velocityX < 0 ? -1 : facingDirection);
    Vector3 newScale = interactableObject.transform.localScale;

    if ((direction > 0 && interactableObject.transform.localScale.x < 0) || (direction < 0 && interactableObject.transform.localScale.x > 0))
    {
      newScale.x *= -1;
    }

    interactableObject.transform.localScale = newScale;
    facingDirection = direction;
  }

  public void Flip()
  {
    interactableObject.transform.localScale = new Vector3(-interactableObject.transform.localScale.x, interactableObject.transform.localScale.y, 1);
    facingDirection *= -1;
  }

  public int Sell()
  {
    Wallet playerWallet = Player.Instance.wallet;
    int price = GetSellPrice;

    playerWallet.EarnCoin(price);

    AnimalFarmManager.Instance.RemoveAnimal(this);
    Destroy(gameObject);

    return price;
  }

  public bool Buy(Player player)
  {
    if (AnimalFarmManager.Instance.AddAnimal(this))
    {
      Wallet playerWallet = player.wallet;
      int price = animalData.purchasePrice;

      if (playerWallet.coin >= price)
      {
        playerWallet.LoseCoin(price);
        return true;
      }
      else
      {
        return false;
      }
    }
    else
    {
      return false;
    }
  }

  public GameObject GetObject()
  {
    return gameObject;
  }

  private void ShowDetail()
  {
    List<FoodType> edibleFoods = animalData.edibleFoods;

    bool isEatMeat = edibleFoods.Contains(FoodType.Meat);
    bool isEatPlant = edibleFoods.Contains(FoodType.Plant);

    AnimalDetailDisplayer.Instance.SetDetails(this, isEatMeat, isEatPlant);
    AnimalDetailDisplayer.Instance.ShowWindow();
  }

  private void HideDetail()
  {
    AnimalDetailDisplayer.Instance.HideWindow();
  }

  private void UpdateDataOnContainer()
  {
    _saveData.UpdateData(this);
  }
}
