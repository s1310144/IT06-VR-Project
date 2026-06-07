using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseBoard;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float boardDistance = 5.0f;
    [SerializeField] private float boardHeightOffset = 1.1f;
    [SerializeField] private Key debugPauseKey;

    private bool isPaused = false;

    private void Update()
    {
        // debugPauseKeyが押されたらPause状態を切り替える
        if (Keyboard.current[debugPauseKey].wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    // Pause状態を切り替える
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Pauseする
    public void PauseGame()
    {
        Debug.Log("Pause shown");

        isPaused = true;

        // PauseBoardをプレイヤーの目の前に移動する
        MovePauseBoardInFrontOfPlayer();

        // PauseBoardを表示する
        if (pauseBoard != null)
        {
            pauseBoard.SetActive(true);
        }

        // Scene内の時間を止める
        Time.timeScale = 0f;
    }

    // Pauseを解除する
    public void ResumeGame()
    {
        Debug.Log("Pause closed");

        isPaused = false;

        // Scene内の時間を通常に戻す
        Time.timeScale = 1f;

        // PauseBoardを非表示にする
        if (pauseBoard != null)
        {
            pauseBoard.SetActive(false);
        }
    }

    // PauseBoardをプレイヤーの目の前に移動する
    private void MovePauseBoardInFrontOfPlayer()
    {
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

        // PauseBoardの位置を決める
        pauseBoard.transform.position =
            mainCamera.position
            + forward * boardDistance
            + Vector3.up * boardHeightOffset;

        // PauseBoardをプレイヤーの方へ向ける
        pauseBoard.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}