using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent {
    public Transform GetKitchenObjectFollowTransform();
    public void SetKitchenObject(KitchenObjects kitchenObject);
    public KitchenObjects GetKitchenObjects();
    public void ClearKitchenObject();
    public bool HasKitchenObject();
}
