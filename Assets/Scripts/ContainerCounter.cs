using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent {
    public event EventHandler onPlayerGrabbedObj;
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            kitchenObjectTransform.GetComponent<KitchenObjects>().SetKitchenObjectParent(player);
            onPlayerGrabbedObj?.Invoke(this, EventArgs.Empty);
        }
    }
}
