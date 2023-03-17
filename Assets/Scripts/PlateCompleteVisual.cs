using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    [SerializeField]
    private PlateKitchenObject plateKO;
    [SerializeField]
    private List<KitchenObjectSO_GameObject> kosoGameObjArray;
    private void Start() {
        plateKO.OnIngredientAdded += PlateKO_OnIngredientAdded;
        foreach (var item in kosoGameObjArray) {
            item.gameObject.SetActive(false);
        }
    }

    private void PlateKO_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        Debug.Log(e.ToString());
        foreach (var item in kosoGameObjArray)
        {
            if(item.kitchenObjectSO == e.kitchenObjectSO) {
                item.gameObject.SetActive(true);
            }
        }
    }
}