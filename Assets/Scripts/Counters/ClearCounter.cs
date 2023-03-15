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
                // ���������û��
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
            else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKO)) {
                // ����õ���һ�����ӣ����Խ�������Ķ����ŵ�������
                if (plateKO.TryAddIngrediant(GetKitchenObject().GetKitchenObjectSO())) {
                    GetKitchenObject().DestorySelf();
                }
            }else if(GetKitchenObject().TryGetPlate(out plateKO)) {
                // �Լ���һ�����ӣ����Խ���ҵĶ����ŵ������ϵ�������
                if (plateKO.TryAddIngrediant(player.GetKitchenObject().GetKitchenObjectSO())){
                    player.GetKitchenObject().DestorySelf();
                }
            }
            
        }
    }


}
