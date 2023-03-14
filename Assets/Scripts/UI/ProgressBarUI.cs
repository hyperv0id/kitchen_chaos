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
    // Unity ��֧��ֱ��ʹ�ýӿ�ע��
    private IHasProgress progress;
    private void Start() {
        progress = hasProgressObj.GetComponent<IHasProgress>();
        if(progress == null) {
            Debug.LogError("IHasProgress was not implemanted by this gameobject" + hasProgressObj);
        }
        progress.OnProgressChanged += IProgress_OnProgressChanged;
        barImage.fillAmount = 0f;

        // ����֮����Hide(), �����ܶ����¼�
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
