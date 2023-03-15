using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter{
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
           if (player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else {
            // counter has KO

            if (!player.HasKitchenObject()) {
                // 柜子有玩家没有
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
            else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKO)) {
                // 玩家拿的是一个盘子，尝试将柜子里的东西放到盘子里
                if (plateKO.TryAddIngrediant(GetKitchenObject().GetKitchenObjectSO())) {
                    GetKitchenObject().DestorySelf();
                }
            }else if(GetKitchenObject().TryGetPlate(out plateKO)) {
                // 自己有一个盘子，尝试将玩家的东西放到柜子上的盘子里
                if (plateKO.TryAddIngrediant(player.GetKitchenObject().GetKitchenObjectSO())){
                    player.GetKitchenObject().DestorySelf();
                }
            }
            
        }
    }


}
