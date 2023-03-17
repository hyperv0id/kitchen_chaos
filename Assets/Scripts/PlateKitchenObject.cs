using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs{
        public KitchenObjectSO kitchenObjectSO;
    }
    // ���ԷŽ��������koso
    [SerializeField]
    private List<KitchenObjectSO> validKOSOs;
    // ������װ��koso
    private List<KitchenObjectSO> koSOInPlate;
    private void Awake() {
        koSOInPlate = new List<KitchenObjectSO>();
    }
    public bool TryAddIngrediant(KitchenObjectSO kitchenObjectSO) {
        // ֻ���⼸������
        if (!validKOSOs.Contains(kitchenObjectSO)) { return false; }
        // �����ظ���
        if (koSOInPlate.Contains(kitchenObjectSO)) { return false; }
        koSOInPlate.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new() {kitchenObjectSO=kitchenObjectSO });
        return true;
    }

}
