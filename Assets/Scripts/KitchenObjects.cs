using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjects : MonoBehaviour
{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    private ClearCounter clearCounter;

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }
    public void SetClearCounter(ClearCounter counter) {
        if(this.clearCounter != null) {
            this.clearCounter.ClearKitchenObject();
        }
        this.clearCounter = counter;
        if(counter.HasKitchenObject()) {
            Debug.LogError("Counter already has a kitchen object!!!");
        }
        this.clearCounter.SetKitchenObject(this);
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public ClearCounter GetClearCounter() { return this.clearCounter; }
}
