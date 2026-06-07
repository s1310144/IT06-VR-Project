using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartManager : MonoBehaviour
{
    [SerializeField] private GameObject board;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float boardDistance = 3.0f;
    [SerializeField] private float boardHeightOffset = 1.1f;

    // Enableになったら動く
    public void OnEnable()
    {
        Debug.Log("Clear/GameOver Board shown");

        // カメラの正面方向を取得する
        Vector3 forward = mainCamera.forward;

        // 上下方向の傾きを消す
        // これをしないと、下を向いてPauseした時に板が地面側に出る
        forward.y = 0f;

        // 長さを1にする
        forward.Normalize();

        // もしforwardがほぼ0なら、カメラの前方向をそのまま使う
        if (forward.sqrMagnitude < 0.001f)
        {
            forward = mainCamera.forward;
        }

        // Boardの位置を決める
        board.transform.position =
            mainCamera.position
            + forward * boardDistance
            + Vector3.up * boardHeightOffset;

        // Boardをプレイヤーの方へ向ける
        board.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);

        // Boardを表示する
        board.SetActive(true);

        // Scene内の時間を止める
        Time.timeScale = 0f;
    }
}