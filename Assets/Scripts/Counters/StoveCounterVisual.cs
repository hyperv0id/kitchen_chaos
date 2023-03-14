using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField]
    private StoveCounter stoveCounter;
    [SerializeField]
    private GameObject stoveOnGameobject;
    [SerializeField]
    private GameObject particlesGameobject;

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool showVisual = stoveCounter.HasKitchenObject();
        stoveOnGameobject.SetActive(showVisual);
        particlesGameobject.SetActive(showVisual);
    }
}
