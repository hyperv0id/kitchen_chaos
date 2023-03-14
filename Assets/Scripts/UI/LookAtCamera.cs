using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }
    [SerializeField]
    private Mode mode;
    private void LateUpdate() {
        switch (mode) {
            case Mode.LookAt:
        // �ϵͰ汾��Ҫ�������°汾�Ѿ���unity�����ˣ�������̫�����ܿ���
        transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCam = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCam);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}