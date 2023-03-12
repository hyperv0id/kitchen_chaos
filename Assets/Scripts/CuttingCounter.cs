using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField]
    private KitchenObjectSO cutKitchenObjectSO;
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else {
            if (!player.HasKitchenObject()) {
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    public override void InteractAlt(Player player) {
        if (HasKitchenObject()) {
            // �ƻ�ԭ��Ʒ��������Ƭ�����Ʒ
            GetKitchenObject().DestorySelf();
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}
