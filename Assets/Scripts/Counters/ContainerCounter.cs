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
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            onPlayerGrabbedObj?.Invoke(this, EventArgs.Empty);
        }
    }
}
