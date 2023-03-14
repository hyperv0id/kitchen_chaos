using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual :MonoBehaviour
{
    [SerializeField] PlatesCounter platesCounter;
    [SerializeField]
    private Transform topPoint;
    [SerializeField]
    private Transform plateVisualPrefab;

    private List<GameObject> plateVisualList;
    private void Start() {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
        plateVisualList = new();
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e) {
        int cnt = plateVisualList.Count;
        GameObject plateVisual = plateVisualList[cnt - 1];
        plateVisualList.RemoveAt(cnt-1);
        Destroy(plateVisual);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e) {
        Transform transform = Instantiate(plateVisualPrefab, topPoint);
        float plateOffsetY = .1f;
        transform.localPosition = new Vector3(0, plateOffsetY * plateVisualList.Count, 0);
        plateVisualList.Add(transform.gameObject);
    }
}
