using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
  public static Player Instance { get; private set; }

  public PercentStatus moveSpeedStatus { get; private set; }
  [SerializeField] private PercentStatusData moveSpeedData;
  private Vector2 moveInput;
  private bool canMove = true;
  private bool canInteract = false;

  [SerializeField] private GameObject characterObject;

  public Wallet wallet { get; private set; }
  public PlayerAnimationController PlayerAnimation { get; private set; }

  [SerializeField] private Transform interactableDetector;
  [SerializeField] private float interactableDetectRange = 0.85f;

  public Interactable targetInteractable { get; private set; }
  private bool isDetectInteractable = false;

  public float facingDirection { get; private set; }

  public Hand playerHand { get; private set; }

  private Rigidbody2D rb;
  [SerializeField] private UnityEvent OnInteractEvent;
  [SerializeField] private UnityEvent OnPickingEvent;
  [SerializeField] private UnityEvent OnDropingEvent;
  [SerializeField] private UnityEvent OnRefillingEvent;
  private Vector2 velocityWorkspace;

  private void Awake()
  {
    playerHand = GetComponent<Hand>();
    moveSpeedStatus = new PercentStatus(moveSpeedData.statusName, moveSpeedData);

    facingDirection = transform.localScale.x / Mathf.Abs(transform.localScale.x);

    wallet = transform.Find("Wallet").GetComponent<Wallet>();
    PlayerAnimation = transform.Find("PlayerCharacter").GetComponent<PlayerAnimationController>();
    Instance = this;
  }

  private void Start()
  {
    DisableMove();
  }

  private void OnValidate()
  {
    if (!rb && GetComponent<Rigidbody2D>())
    {
      rb = GetComponent<Rigidbody2D>();
      rb.isKinematic = false;
      rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
  }

  private void Update()
  {

    isDetectInteractable = CheckInteractableInRange();

    if (!canMove)
    {
      return;
    }

    if (PlayerAnimation.isPlayingAnimation)
    {
      if (isDetectInteractable && targetInteractable)
      {
        if (Input.GetKey(KeyCode.J))
        {
          if (playerHand.holdingObject)
          {
            if (playerHand.holdingObject is WateringPot && targetInteractable is Pool)
            {
              PlayerAnimation.SetRefillingAnimation(true);
              SoundEffectsController.Instance.PlaySoundEffect("Refilling");
              UseItem();
            }
          }
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
          PlayerAnimation.SetRefillingAnimation(false);
          SoundEffectsController.Instance.StopSoundEffect("Refilling");
        }
      }
      moveInput.Set(0, 0);
      return;
    }

    CheckMoveInput();

    if (Input.GetKeyDown(KeyCode.J) && isDetectInteractable && targetInteractable)
    {
      if (playerHand.holdingObject)
      {
        if (playerHand.holdingObject is ISellable && targetInteractable is ShopForSell)
        {
          ISellable sellable = playerHand.holdingObject as ISellable;
          ShopForSell shop = targetInteractable as ShopForSell;
          if (shop.PutItemInContainer(sellable))
          {
            playerHand.SetInHandItemToNull();
          }
        }
        else if (playerHand.holdingObject is IUsable && ItemUseMatcher.isMatch((IUsable)playerHand.holdingObject, targetInteractable))
        {
          UseItem();
          if (playerHand.holdingObject is WateringPot && targetInteractable is Pool)
          {
            PlayerAnimation.SetRefillingAnimation(true);
            return;
          }
          else
            OnInteractEvent?.Invoke();

        }
        else if (playerHand.holdingObject is IAnimalEdible && targetInteractable is Animal)
        {
          IAnimalEdible animalEdible = (IAnimalEdible)playerHand.holdingObject;
          Animal animal = targetInteractable as Animal;

          if (animalEdible.Feed(animal))
          {
            playerHand.SetInHandItemToNull();
          }
        }
        else if (targetInteractable is PickableObject)
        {
          playerHand.PickUpObject((PickableObject)targetInteractable);
          OnPickingEvent.Invoke();
        }
        else
        {
          Interact();
        }
      }
      else if (targetInteractable is PickableObject)
      {
        playerHand.PickUpObject((PickableObject)targetInteractable);
        OnPickingEvent.Invoke();
      }
      else
      {
        Interact();
      }
    }

    if (Input.GetKeyDown(KeyCode.K))
    {
      if (playerHand.holdingObject)
      {
        OnDropingEvent.Invoke();
      }

    }

    if (Input.GetKeyDown(KeyCode.L))
    {
      wallet.EarnCoin(5);
    }

    if (Input.GetButtonUp("Submit"))
    {
      canInteract = true;
    }
  }

  public void FixedUpdate()
  {
    Move();
  }

  private void CheckMoveInput()
  {
    if (!canMove)
    {
      moveInput.Set(0, 0);
      return;
    }

    float inputX = Input.GetAxisRaw("Horizontal");
    float inputY = Input.GetAxisRaw("Vertical");

    if (moveInput.magnitude != 0)
    {
      if (facingDirection > 0 && inputX < 0)
      {
        Flip();
      }
      else if (facingDirection < 0 && inputX > 0)
      {
        Flip();
      }
    }

    moveInput.Set(inputX, inputY);
  }

  public void UseItem()
  {
    if (playerHand.holdingObject is IUsable)
    {
      IUsable useableItem = playerHand.holdingObject as IUsable;

      if (useableItem.Use(targetInteractable))
      {
        playerHand.SetInHandItemToNull();
      }
    }
  }

  private void Move()
  {
    if (canMove)
    {
      int moveSpeed = moveSpeedStatus.GetValue;

      float velocityX = moveInput.x * moveSpeed * Time.fixedDeltaTime;
      float velocityY = moveInput.y * moveSpeed * Time.fixedDeltaTime;

      velocityWorkspace.Set(velocityX, velocityY);
      PlayerAnimation.SetRunningAnimation(velocityWorkspace);
      rb.velocity = velocityWorkspace;
      if (Mathf.Abs(velocityWorkspace.magnitude) > 0.1)
      {
        SoundEffectsController.Instance.PlaySoundEffect("Walk");
      }
      else
        SoundEffectsController.Instance.StopSoundEffect("Walk");
    }
  }

  private void Flip()
  {
    facingDirection *= -1;
    characterObject.transform.localScale = new Vector2(-characterObject.transform.localScale.x, characterObject.transform.localScale.y);
  }

  private void Interact()
  {
    if (isDetectInteractable && targetInteractable && canInteract)
    {
      targetInteractable.Interact(this);
    }
  }

  private bool CheckInteractableInRange()
  {
    ChangeTargetInteractable(null);
    Collider2D[] hits = Physics2D.OverlapCircleAll(interactableDetector.position, interactableDetectRange);

    if (hits.Length == 0)
    {
      return false;
    }

    foreach (var hit in hits)
    {
      if (hit.GetComponent<Interactable>() == null)
      {
        continue;
      }

      Interactable interactable = hit.GetComponent<Interactable>();

      if (interactable.Equals(playerHand.holdingObject) || interactable.Equals(targetInteractable))
      {
        continue;
      }

      if (interactable && interactable.isInteractable)
      {
        if (targetInteractable)
        {
          float distanceBetweenOldTarget = Vector2.Distance(transform.position, targetInteractable.objectCollider.bounds.center);
          float distanceBetweenNewTarget = Vector2.Distance(transform.position, interactable.objectCollider.bounds.center);

          if (distanceBetweenNewTarget < distanceBetweenOldTarget)
          {
            ChangeTargetInteractable(interactable);
          }
        }
        else
        {
          ChangeTargetInteractable(interactable);
        }
      }

    }

    if (targetInteractable)
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  private void ChangeTargetInteractable(Interactable newInteractable)
  {
    targetInteractable?.HideObjectHighlight();
    targetInteractable = newInteractable;
    targetInteractable?.ShowObjectHighlight();
  }

  public void EnableMove()
  {
    canMove = true;
  }

  public void DisableMove()
  {
    canMove = false;
    moveInput = Vector3.zero;
    rb.velocity = Vector3.zero;
    PlayerAnimation.SetRunningAnimation(Vector2.zero);

    canInteract = false;
  }

  private void OnDrawGizmos()
  {
    if (interactableDetector)
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(interactableDetector.position, interactableDetectRange);
    }
  }
}
