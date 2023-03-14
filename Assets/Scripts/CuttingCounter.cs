using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {
    [SerializeField]
    private CuttingRecipeSO[] cuttingRecipeSOArray;
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                // 只有在料理中的才会被放进来，否则不会被放进来
                if (HasRecipewithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
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
            KitchenObjectSO outputSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            // 不能切的料理
            if (outputSO == null) { return; }
            // 破环原物品，生成切片后的物品
            GetKitchenObject().DestorySelf();
            KitchenObject.SpawnKitchenObject(outputSO, this);
        }
        // do nothing
    }
    private bool HasRecipewithInput(KitchenObjectSO inputkitchenobjectso) {
        foreach (var cuttingRecipeso in cuttingRecipeSOArray) {
            if (cuttingRecipeso.input == inputkitchenobjectso) {
                return true;
            }
        }
        return false;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var item in cuttingRecipeSOArray) {
            if (item.input == inputKitchenObjectSO) {
                return item.output;
            }
        }
        return null;
    }

}
