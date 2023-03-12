using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField]
    private Transform counterTopPoint;
    private KitchenObjects kitchenObject;
    public virtual void Interact(Player player) {
        // Do nothing
        Debug.LogError("BaseCounter.Interact() was Not Implemented");
    }
    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }
    public void SetKitchenObject(KitchenObjects kitchenObject) {
        this.kitchenObject = kitchenObject;
    }
    public KitchenObjects GetKitchenObjects() {
        return this.kitchenObject;
    }
    public void ClearKitchenObject() {
        this.kitchenObject = null;
    }
    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
