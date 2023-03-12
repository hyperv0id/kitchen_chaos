using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameInput gameInput;
    private bool isWalking;

    void Update() {
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
    public bool IsWalking() { return isWalking; }

}
