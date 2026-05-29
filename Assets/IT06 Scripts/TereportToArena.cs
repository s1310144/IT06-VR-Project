using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToArena : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform xrOrigin;

    [Header("Main Camera")]
    [SerializeField] private Transform mainCamera;

    [Header("Teleport destination")]
    [SerializeField] private Transform TeleportPoint;

    // Startボタンから呼び出す関数
    public void Teleport()
    {

        // XR Originをテレポート先へ移動させる
        xrOrigin.position = TeleportPoint.position;

        // テレポート先のZ軸方向を取得する
        Vector3 targetForward = TeleportPoint.forward;

        // Y方向の上下成分を消して、水平方向だけを見る
        targetForward.y = 0f;

        // 長さを1にする
        targetForward.Normalize();

        // 現在のカメラの向きを取得する
        Vector3 cameraForward = mainCamera.forward;

        // カメラの上下成分を消して、水平方向だけを見る
        cameraForward.y = 0f;

        // 長さを1にする
        cameraForward.Normalize();

        // カメラの向きから、テレポート先の向きまで何度回せばいいか計算する
        float angle = Vector3.SignedAngle(cameraForward, targetForward, Vector3.up);

        // XR OriginをY軸方向に回転させる
        xrOrigin.Rotate(0f, angle, 0f, Space.World);
    }
}
