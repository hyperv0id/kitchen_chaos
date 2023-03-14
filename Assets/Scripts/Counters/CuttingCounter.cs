using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs>  OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField]
    private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;


    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                // 只有在料理中的才会被放进来，否则不会被放进来
                CuttingRecipeSO recipeSO = GetRecipeFormInput(player.GetKitchenObject().GetKitchenObjectSO());
                if(recipeSO!= null) {
                    cuttingProgress = 0;
                    OnProgressChanged?.Invoke(this, new() { progressNormalized = (float)cuttingProgress / recipeSO.maxCuttingProgress }) ;
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
        // Cut Action
        if (HasKitchenObject()) {
            CuttingRecipeSO recipeSO = GetRecipeFormInput(GetKitchenObject().GetKitchenObjectSO());
            // 不能切的料理
            if (recipeSO == null) { return; }
            cuttingProgress++;
            OnProgressChanged?.Invoke(this, new() { progressNormalized = (float)cuttingProgress / recipeSO.maxCuttingProgress });

            OnCut?.Invoke(this, EventArgs.Empty);

            if (cuttingProgress >= recipeSO.maxCuttingProgress) {
                // 破环原物品，生成切片后的物品
                GetKitchenObject().DestorySelf();
                KitchenObject.SpawnKitchenObject(recipeSO.output, this);
            }
        }
        // do nothing
    }
    private bool HasRecipewithInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO recipe = GetRecipeFormInput(inputKitchenObjectSO);
        return recipe ? true : false;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO recipe = GetRecipeFormInput(inputKitchenObjectSO);
        return recipe?recipe.output:null;
    }
    private CuttingRecipeSO GetRecipeFormInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var item in cuttingRecipeSOArray) {
            if (item.input == inputKitchenObjectSO) {
                return item;
            }
        }
        return null;
    }

}
