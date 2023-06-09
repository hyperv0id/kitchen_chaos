using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs{
        public KitchenObjectSO kitchenObjectSO;
    }
    // 可以放进盘子里的koso
    [SerializeField]
    private List<KitchenObjectSO> validKOSOs;
    // 盘子里装的koso
    private List<KitchenObjectSO> koSOInPlate;
    private void Awake() {
        koSOInPlate = new List<KitchenObjectSO>();
    }
    public bool TryAddIngrediant(KitchenObjectSO kitchenObjectSO) {
        // 只有这几种能拿
        if (!validKOSOs.Contains(kitchenObjectSO)) { return false; }
        // 不能重复拿
        if (koSOInPlate.Contains(kitchenObjectSO)) { return false; }
        koSOInPlate.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new() {kitchenObjectSO=kitchenObjectSO });
        return true;
    }

}
