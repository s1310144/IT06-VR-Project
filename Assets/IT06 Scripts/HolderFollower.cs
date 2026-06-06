using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderFollower : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;

    [SerializeField] private Transform xrOrigin;

    private void Update()
    {
        // 必要な参照が設定されていない場合は何もしない
        if (mainCamera == null || xrOrigin == null)
        {
            return;
        }

        // 身体の位置をHMDの位置へ追従させる
        transform.position = mainCamera.position;

        // 身体の向きはXR Originの水平方向に合わせる
        Vector3 forward = xrOrigin.forward;
        forward.y = 0f;

        // 正しい向きが取得できた場合だけ回転する
        if (forward.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }
}