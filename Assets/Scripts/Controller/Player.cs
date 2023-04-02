using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {

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
            selectedCounter.Interact(this);
        }
    }

    void Update() {
        HandleInteractions();
        HandleMovement();
    }
    public bool IsWalking() { return isWalking; }
    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
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
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
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
