using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour {
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;
    [SerializeField]
    private Transform counterTopPoint;
    private KitchenObjects kitchenObject;
    public void Interact() {
        if (kitchenObject == null) {
            Debug.Log("Interact!");
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObjects>().SetClearCounter(this);
            kitchenObjectTransform.localPosition = Vector3.zero;
            kitchenObject = kitchenObjectTransform.GetComponent<KitchenObjects>();
            kitchenObject.SetClearCounter(this);
        }
        else {
            Debug.Log(kitchenObject.GetClearCounter());
        }
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
