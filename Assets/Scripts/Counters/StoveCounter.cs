using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter,IHasProgress {

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }

    public enum State {
        Idle, Frying, Fried, Burned,
    }
    [SerializeField]
    private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField]
    private BurningRecipeSO[] burningRecipeSOArray;

    private State state;

    private float fryingTimer;
    private float burningTimer;

    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
    }
    private void Update() {

        if (!HasKitchenObject()) {
            OnProgressChanged?.Invoke(this, new() { progressNormalized = 0f });
            return;
        }

        switch (state) {
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new() { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
                if (fryingTimer >= fryingRecipeSO.fryingTimerMax) {
                    // fried
                    GetKitchenObject().DestorySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                    state = State.Fried;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    burningTimer = 0f;
                    burningRecipeSO = GetBurningRecipeFormInput(GetKitchenObject().GetKitchenObjectSO());
                }
                break;
            case State.Fried:
                burningTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new() { progressNormalized = burningTimer / burningRecipeSO.burningTimerMax });
                if (burningTimer >= burningRecipeSO.burningTimerMax) {
                    // fried
                    GetKitchenObject().DestorySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                    state = State.Burned;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    OnProgressChanged?.Invoke(this, new() { progressNormalized = 0f });
                }
                break;
            case State.Burned:
                break;
        }

    }
    public override void Interact(Player player) {
        // TODO
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                // 只有在料理中的才会被放进来，否则不会被放进来
                fryingRecipeSO = GetFryingRecipeFormInput(player.GetKitchenObject().GetKitchenObjectSO());
                if (fryingRecipeSO != null) {
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    fryingTimer = 0f;
                    OnProgressChanged?.Invoke(this, new() { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
                    if (fryingRecipeSO != null) {
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                    }
                }
            }
        }
        else {
            // 被玩家取走
            if (!player.HasKitchenObject()) {
                this.GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                fryingTimer = 0f;
                OnProgressChanged?.Invoke(this, new() { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            }
            else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKO)) {
                // 玩家拿的是一个盘子
                if (plateKO.TryAddIngrediant(GetKitchenObject().GetKitchenObjectSO())) {
                    GetKitchenObject().DestorySelf();
                    // 重置状态机
                    state = State.Idle;
                    fryingTimer = 0f;
                    OnProgressChanged?.Invoke(this, new() { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                }
            }
        }

    }
    private bool HasRecipewithInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO recipe = GetFryingRecipeFormInput(inputKitchenObjectSO);
        return recipe ? true : false;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO recipe = GetFryingRecipeFormInput(inputKitchenObjectSO);
        return recipe ? recipe.output : null;
    }
    private FryingRecipeSO GetFryingRecipeFormInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var item in fryingRecipeSOArray) {
            if (item.input == inputKitchenObjectSO) {
                return item;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeFormInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var item in burningRecipeSOArray) {
            if (item.input == inputKitchenObjectSO) {
                return item;
            }
        }
        return null;
    }

}
