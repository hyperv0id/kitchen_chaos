using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField]
    private Transform counterTopPoint;
    private KitchenObject kitchenObject;
     public virtual void Interact(Player player) {
        // Do nothing
        Debug.LogError("BaseCounter.Interact() was Not Implemented");
    }
   public virtual void InteractAlt(Player player) {
        // Do nothing
        Debug.LogError("BaseCounter.InteractAlt() was Not Implemented");
    }
    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
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
