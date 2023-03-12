using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameInput gameInput;
    [SerializeField]
    private LayerMask counterLayerMask;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteraction;
    }

    private void GameInput_OnInteraction(object sender, EventArgs e) {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero) { lastInteractDir = moveDir; }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask)) {
            raycastHit.transform.TryGetComponent(out ClearCounter clearCounter);
            if (clearCounter != null) { clearCounter.Interact(); }
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
        if(moveDir != Vector3.zero) { lastInteractDir = moveDir; }
        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask)) {
            raycastHit.transform.TryGetComponent(out ClearCounter clearCounter);
            // if(clearCounter != null) { clearCounter.Interact(); }
        }
    }
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime);

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

}
