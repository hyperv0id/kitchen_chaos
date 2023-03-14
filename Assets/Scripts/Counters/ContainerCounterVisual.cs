using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "_OpenClose";
    [SerializeField]
    private ContainerCounter containerCounter;

    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Start() {
        containerCounter.onPlayerGrabbedObj += ContainerCounter_onPlayerGrabbedObj;
    }

    private void ContainerCounter_onPlayerGrabbedObj(object sender, System.EventArgs e) {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
