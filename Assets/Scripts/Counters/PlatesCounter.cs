using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter {
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenobjectSO;
    // 生成盘子计时 
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 2f;
    // 生成盘子计数
    private int spawnCount, spawnCountMax = 4;
    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer >= spawnPlateTimerMax) {
            spawnPlateTimer = 0f;
            if (spawnCount < spawnCountMax) {
                spawnCount++;
                //KitchenObject.SpawnKitchenObject(plateKitchenobjectSO, this);
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject() && spawnCount!=0) {
            spawnCount--;
            KitchenObject.SpawnKitchenObject(plateKitchenobjectSO, player);
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
