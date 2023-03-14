using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
    [SerializeField]
    private BaseCounter baseCounter;
    [SerializeField]
    private GameObject[] visualGameObjectArray;
    // 柜台过多，使用单例模式注入玩家
    private void Start() {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if (e.selectedCounter == baseCounter) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        foreach (GameObject visualGO in visualGameObjectArray) {
            visualGO.SetActive(true);
        }
    }

    private void Hide() {
        foreach (GameObject visualGO in visualGameObjectArray) {
            visualGO.SetActive(false);
        }
    }
}
