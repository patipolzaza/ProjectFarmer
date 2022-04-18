using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    #region Custom Inepector
    [CustomEditor(typeof(Player))]
    [CanEditMultipleObjects]
    class CustomInspector : Editor
    {
        SerializedProperty interactableDetectorProp;
        SerializedProperty interactableDetectorRangeProp;
        SerializedProperty characterObjectProp;

        SerializedProperty moveSpeedDataProp;

        SerializedProperty OnWateringEventProp;
        SerializedProperty OnPickingEventProp;

        private void OnEnable()
        {
            interactableDetectorProp = serializedObject.FindProperty("interactableDetector");
            interactableDetectorRangeProp = serializedObject.FindProperty("interactableDetectRange");
            characterObjectProp = serializedObject.FindProperty("characterObject");

            moveSpeedDataProp = serializedObject.FindProperty("moveSpeedData");

            OnWateringEventProp = serializedObject.FindProperty("OnWateringEvent");
            OnPickingEventProp = serializedObject.FindProperty("OnPickingEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Label("Character Object.");
            EditorGUILayout.PropertyField(characterObjectProp);
            GUILayout.Space(2f);

            GUILayout.Label("Status Data.");
            EditorGUILayout.PropertyField(moveSpeedDataProp);
            if (GUILayout.Button("Generate data file"))
            {
                OpenSaveFilePanel();
            }
            GUILayout.Space(1.25f);
            GUILayout.Label("Interactable Detection.");
            EditorGUILayout.PropertyField(interactableDetectorProp);
            EditorGUILayout.PropertyField(interactableDetectorRangeProp);
            GUILayout.Space(1.25f);

            GUILayout.Label("Unity Envent.");
            EditorGUILayout.PropertyField(OnWateringEventProp);
            EditorGUILayout.PropertyField(OnPickingEventProp);


            serializedObject.ApplyModifiedProperties();
        }

        private void OpenSaveFilePanel()
        {
            string startPath = Application.dataPath;
            string filePath = EditorUtility.SaveFilePanel("Choose path to place file.", startPath, "Data_MoveSpeedStatus", "asset");

            if (filePath.Length > 0)
            {
                GenerateMoveSpeedData(filePath);
            }
        }

        private void GenerateMoveSpeedData(string filePath)
        {
            string[] paths = filePath.Split('/');

            string realPath = "";
            bool isFoundAssetsAtPath = false;
            int index = 0;
            string appendingPath;
            foreach (var splittedPath in paths)
            {
                if (splittedPath.Equals("Assets"))
                {
                    isFoundAssetsAtPath = true;
                }

                if (isFoundAssetsAtPath)
                {
                    appendingPath = splittedPath;

                    realPath += appendingPath;

                    if (index < paths.Length - 1)
                    {
                        realPath += "/";
                    }
                }

                index++;
            }

            PercentStatusData moveSpeedData = CreateInstance<PercentStatusData>();
            moveSpeedData.Init("Move Speed", 4, 250, 30, 10, 5);

            AssetDatabase.CreateAsset(moveSpeedData, realPath);
            AssetDatabase.SaveAssets();

            SetMoveSpeedStatusData(moveSpeedData);
        }

        private void SetMoveSpeedStatusData(PercentStatusData newData)
        {
            moveSpeedDataProp.objectReferenceValue = newData;
        }
    }
    #endregion
    public static Player Instance { get; private set; }

    public PercentStatus moveSpeedStatus { get; private set; }
    [SerializeField] private PercentStatusData moveSpeedData;
    private Vector2 moveInput;
    private bool canMove = true;

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
    [SerializeField] private UnityEvent OnWateringEvent;
    [SerializeField] private UnityEvent OnPickingEvent;

    private Vector2 velocityWorkspace;

    private void Awake()
    {
        playerHand = GetComponent<Hand>();

        facingDirection = transform.localScale.x / Mathf.Abs(transform.localScale.x);

        wallet = transform.Find("Wallet").GetComponent<Wallet>();
        PlayerAnimation = transform.Find("PlayerCharacter").GetComponent<PlayerAnimationController>();
        Instance = this;
    }

    private void Start()
    {
        moveSpeedStatus = new PercentStatus(moveSpeedData.statusName, moveSpeedData);
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
            if (playerHand.holdingObject)
            {
                playerHand.DropObject();
            }

            return;
        }

        if (PlayerAnimation.isPlayingAnimation)
        {
            moveInput.Set(0, 0);
            return;
        }

        CheckMoveInput();

        if (isDetectInteractable && targetInteractable)
        {
            if (Input.GetKeyDown(KeyCode.J))
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
                        OnWateringEvent.Invoke();
                        UseItem();
                        if (playerHand.holdingObject is WateringPot && targetInteractable is Plot)
                        {
                            OnWateringEvent.Invoke();
                        }



                    }
                    else if (playerHand.holdingObject is AnimalFood && targetInteractable is Animal)
                    {
                        UseItem();
                    }

                    else if (targetInteractable is PickableObject)
                    {
                        playerHand.PickUpObject((PickableObject)targetInteractable);
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
            if (Input.GetKey(KeyCode.J))
            {
                if (playerHand.holdingObject)
                {
                    if (playerHand.holdingObject is WateringPot && targetInteractable is Pool)
                    {

                        UseItem();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            OnPickingEvent.Invoke();
            playerHand.DropObject();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            wallet.EarnCoin(5);
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
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        characterObject.transform.localScale = new Vector2(-characterObject.transform.localScale.x, characterObject.transform.localScale.y);
    }

    private void Interact()
    {
        if (isDetectInteractable && targetInteractable)
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
        if (targetInteractable)
        {
            targetInteractable.HideObjectHighlight();
        }

        targetInteractable = newInteractable;

        if (targetInteractable)
        {
            targetInteractable.ShowObjectHighlight();
        }
    }

    public void EnableMove()
    {
        canMove = true;
    }

    public void DisableMove()
    {
        canMove = false;
        rb.velocity = Vector3.zero;
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
