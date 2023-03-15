using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
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
        return true;
    }

}
