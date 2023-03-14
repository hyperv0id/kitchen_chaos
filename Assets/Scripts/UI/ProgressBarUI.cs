using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField]
    private Image barImage;
    [SerializeField]
    private GameObject hasProgressObj;
    // Unity 不支持直接使用接口注入
    private IHasProgress progress;
    private void Start() {
        progress = hasProgressObj.GetComponent<IHasProgress>();
        if(progress == null) {
            Debug.LogError("IHasProgress was not implemanted by this gameobject" + hasProgressObj);
        }
        progress.OnProgressChanged += IProgress_OnProgressChanged;
        barImage.fillAmount = 0f;

        // 订阅之后再Hide(), 否则不能订阅事件
        Hide();
    }

    private void IProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;
        if (barImage.fillAmount == 0f || barImage.fillAmount == 1f) { Hide(); }
        else { Show(); }
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
    private void Show() {
        gameObject.SetActive(true);
    }
}
