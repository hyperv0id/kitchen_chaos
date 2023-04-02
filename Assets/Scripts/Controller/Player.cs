using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct InputConstants
{
    public const string MouseXInput = "Mouse X";
    public const string MouseYInput = "Mouse Y";
    public const string MouseScrollInput = "Mouse ScrollWheel";
    public const string HorizontalInput = "Horizontal";
    public const string VerticalInput = "Vertical";
}

public class Player : MonoBehaviour, IKitchenObjectParent {

    [SerializeField]
    private KinematicCharacterController.Examples.ExampleCharacterCamera OrbitCamera; // 主相机
    [SerializeField]
    private Transform CameraFollowPoint; // 相机看向哪里
    [SerializeField]
    private PlayerKCC kccCharacter; // 使用kcc组件控制角色运动

    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameInput gameInput;
    [SerializeField]
    private LayerMask counterLayerMask;

    [SerializeField]
    private Transform kitchenObjectPoint;


    private bool isWalking;
    private Vector3 lastInteractDir;

    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    private void Awake() {
        if (Instance != null)
            Debug.LogError("This should not happen in this single-player game");
        Instance = this;
    }
    private void Start() {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            OrbitCamera.SetFollowTransform(CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            OrbitCamera.IgnoredColliders.Clear();
            OrbitCamera.IgnoredColliders.AddRange(kccCharacter.GetComponentsInChildren<Collider>());
        gameInput.OnInteractAction += GameInput_OnInteraction;
        gameInput.OnInteractAltAction += GameInput_OnInteractAltAction;
    }

    private void GameInput_OnInteractAltAction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.InteractAlt(this);
        }
    }

    private void GameInput_OnInteraction(object sender, EventArgs e) {
        if(selectedCounter != null) {
            Debug.Log(selectedCounter + "should interact");
            selectedCounter.Interact(this);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        HandlePlayerInputs();
        HandleInteractions();
        // HandleMovement();
    }

    private void HandlePlayerInputs(){
        KCCInput characterInputs = new();

        // Build the CharacterInputs struct
        characterInputs.moveInput = gameInput.GetMoveV3();

        isWalking = characterInputs.moveInput != Vector3.zero;

        characterInputs.CameraRotation = OrbitCamera.Transform.rotation;

        // Apply inputs to character
        kccCharacter.SetInput(ref characterInputs);
    }

    // 更新UI or 视觉
    private void LateUpdate()
    {
        HandleCameraInput();
    }

    // 处理相机
    private void HandleCameraInput()
    {
        // Create the look input vector for the camera
        float mouseLookAxisUp = Input.GetAxisRaw(InputConstants.MouseYInput);
        float mouseLookAxisRight = Input.GetAxisRaw(InputConstants.MouseXInput);
        Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        // Prevent moving the camera while the cursor isn't locked
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            lookInputVector = Vector3.zero;
        }

        // Input for zooming the camera (disabled in WebGL because it can cause problems)
        float scrollInput = -Input.GetAxis(InputConstants.MouseScrollInput);

        // Apply inputs to the camera
        OrbitCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

        // Handle toggling zoom level
        if (Input.GetMouseButtonDown(1))
        {
            OrbitCamera.TargetDistance = (OrbitCamera.TargetDistance == 0f) ? OrbitCamera.DefaultDistance : 0f;
        }
    }
    public bool IsWalking() { return isWalking; }
    private void HandleInteractions() {
        Vector3 moveDir = gameInput.GetMoveV3();
        if (moveDir != Vector3.zero) { lastInteractDir = moveDir; }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask)) {
            raycastHit.transform.TryGetComponent(out BaseCounter baseCounter);
            if (baseCounter != null) {
                if (baseCounter != selectedCounter) { SelectCounter( baseCounter); }
            }
            else {
                SelectCounter(null);
            }
        }
        else {
            SelectCounter(null);
        }
        //SetselectedCounter(selectedCounter);
    }
    private void HandleMovement() {
        Vector3 moveDir = gameInput.GetMoveV3();
        float rotateSpeed = 7f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        float playerRadius = .7f;
        float playerHeight = 2f;
        float moevDistance = speed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moevDistance);
        if (!canMove) {
            moveDir = Vector3.zero;
            // 如果是rust的话可以原来的变量应该好一些
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moevDistance);
            // can only move on X
            if (canMove) { moveDir = moveDirX; }
            else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moevDistance);
                // can only move on Z
                if (canMove) { moveDir = moveDirZ; }
                else { /* cannot move at any dir*/}
            }
        }

        transform.position += speed * moveDir * Time.deltaTime;
        isWalking = moveDir != Vector3.zero;
    }
    private void SelectCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        if(selectedCounter)
            Debug.Log(selectedCounter + "is Selected");
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }
    public KitchenObject GetKitchenObject() {
        return this.kitchenObject;
    }
    public void ClearKitchenObject() {
        this.kitchenObject = null;
    }
    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
