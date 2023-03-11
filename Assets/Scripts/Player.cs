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
        isWalking = moveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime);
        transform.position += speed * moveDir * Time.deltaTime;
    }
    public bool IsWalking() { return isWalking; }

}
