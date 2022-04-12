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
        SerializedProperty interactableLayerMaskProp;
        SerializedProperty itemHolderTransformProp;
        SerializedProperty itemDropTransformProp;

        SerializedProperty moveSpeedDataProp;

        private void OnEnable()
        {
            interactableDetectorProp = serializedObject.FindProperty("interactableDetector");
            interactableDetectorRangeProp = serializedObject.FindProperty("interactableDetectRange");
            interactableLayerMaskProp = serializedObject.FindProperty("interactableLayerMask");

            itemHolderTransformProp = serializedObject.FindProperty("itemHolderTransform");
            itemDropTransformProp = serializedObject.FindProperty("itemDropTransform");

            moveSpeedDataProp = serializedObject.FindProperty("moveSpeedData");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

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
            EditorGUILayout.PropertyField(interactableLayerMaskProp);
            GUILayout.Space(1.25f);
            GUILayout.Label("Item Pick.");
            EditorGUILayout.PropertyField(itemHolderTransformProp);
            EditorGUILayout.PropertyField(itemDropTransformProp);

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

    [SerializeField] private Transform interactableDetector;
    [SerializeField] private float interactableDetectRange = 0.85f;
    [SerializeField] private LayerMask interactableLayerMask;

    public PickableObject holdingObject { get; private set; }
    public Interactable targetInteractable { get; private set; }
    private bool isDetectInteractable = false;

    [Tooltip("The transform that is a where player hold the hoding object.")]
    [SerializeField] private Transform itemHolderTransform;
    [Tooltip("The transform that is a where player drop the hoding object.")]
    [SerializeField] private Transform itemDropTransform;

    public float facingDirection { get; private set; }

    private Rigidbody2D rb;

    [SerializeField] private UnityEvent OnWatering;

    private Vector2 velocityWorkspace;

    private void Awake()
    {
        facingDirection = transform.localScale.x / Mathf.Abs(transform.localScale.x);

        wallet = transform.Find("Wallet").GetComponent<Wallet>();

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
            if (holdingObject)
            {
                DropItem();
            }

            return;
        }

        CheckMoveInput();

        if (isDetectInteractable && targetInteractable)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (holdingObject)
                {
                    if (holdingObject is IValuable && targetInteractable is ShopForSell)
                    {
                        IValuable valuable = holdingObject as IValuable;
                        ShopForSell shop = targetInteractable as ShopForSell;
                        if (shop.PutItemInContainer(valuable))
                        {
                            holdingObject = null;
                        }
                    }
                    else if (holdingObject is AnimalFood && targetInteractable is Animal)
                    {
                        UseItem();
                    }
                    else if (targetInteractable is PickableObject)
                    {
                        PickUpItem((PickableObject)targetInteractable);
                    }
                    else if (holdingObject is WateringPot && targetInteractable is Plot)
                    {
                        OnWatering?.Invoke();
                        UseItem();
                    }
                    if (holdingObject is WateringPot && targetInteractable is Pool)
                    {
                        OnWatering?.Invoke();
                        UseItem();
                    }
                    else
                    {
                        Interact();
                    }
                }
                else if (targetInteractable is PickableObject)
                {
                    PickUpItem((PickableObject)targetInteractable);
                }
                else
                {
                    Interact();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            DropItem();
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
        if (holdingObject is Item)
        {
            Item useableItem = holdingObject as Item;
            if (useableItem.Use(targetInteractable))
            {
                holdingObject = null;
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

    public void PickUpItem(PickableObject itemToPick)
    {
        if (holdingObject)
        {
            DropItem();
        }

        Transform itemTransform = itemToPick.Pick(this);

        itemToPick.SetParent(itemHolderTransform);
        itemToPick.SetLocalPosition(new Vector3(0, 0, 1), false, true);

        holdingObject = itemTransform.GetComponent<PickableObject>();
    }

    public void DropItem()
    {
        if (holdingObject)
        {
            PickableObject pickable = holdingObject;

            pickable.SetParent(itemDropTransform);
            pickable.SetLocalPosition(new Vector3(0, 0, 1), true, true);
            pickable.Drop();

            pickable.SetInteractable(true);

            holdingObject = null;
        }
    }

    private bool CheckInteractableInRange()
    {
        ChangeTargetInteractable(null);
        Collider2D[] hits = Physics2D.OverlapCircleAll(interactableDetector.position, interactableDetectRange, interactableLayerMask);

        if (hits.Length == 0)
        {
            return false;
        }

        foreach (var hit in hits)
        {
            Interactable interactable = hit.GetComponent<Interactable>();

            if (interactable.Equals(holdingObject) || interactable.Equals(targetInteractable))
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
                    if (holdingObject)
                    {
                        if (holdingObject is IValuable && targetInteractable is ShopForSell)
                        {
                            Debug.Log(targetInteractable);
                            IValuable valuable = holdingObject as IValuable;
                            ShopForSell shop = targetInteractable as ShopForSell;
                            shop.ShowPrice(valuable.GetPrice());

                        }
                    }
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
