using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjects : MonoBehaviour
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        if(this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        if(kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("Counter already has a kitchen object!!!");
        }
        this.kitchenObjectParent.SetKitchenObject(this);
        transform.parent = this.kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IKitchenObjectParent GetKitchenObjectParent() { return this.kitchenObjectParent; }
}
