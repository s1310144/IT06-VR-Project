using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseBoard;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float boardDistance = 1.5f;
    [SerializeField] private float boardHeightOffset = -0.1f;
    [SerializeField] private Behaviour[] scriptsToDisable;
    [SerializeField] private Key debugPauseKey;

    private bool isPaused = false;

    // ポーズ前に各スクリプトがONだったかを記録する配列
    private bool[] originalEnabledStates;

    private void Start()
    {
        // 登録したスクリプトの数だけ、ON/OFF状態を保存する配列を作る
        originalEnabledStates = new bool[scriptsToDisable.Length];
    }

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
        isPaused = true;

        // PauseBoardをプレイヤーの目の前に移動する
        MovePauseBoardInFrontOfPlayer();

        // PauseBoardを表示する
        if (pauseBoard != null)
        {
            pauseBoard.SetActive(true);
        }

        // 登録されたスクリプトを停止する
        for (int i = 0; i < scriptsToDisable.Length; i++)
        {
            // 登録欄が空なら飛ばす
            if (scriptsToDisable[i] == null)
            {
                continue;
            }

            // ポーズ前のON/OFF状態を保存する
            originalEnabledStates[i] = scriptsToDisable[i].enabled;

            // スクリプトをOFFにする
            scriptsToDisable[i].enabled = false;
        }

        // Scene内の時間を止める
        Time.timeScale = 0f;
    }

    // Pauseを解除する
    public void ResumeGame()
    {
        isPaused = false;

        // Scene内の時間を通常に戻す
        Time.timeScale = 1f;

        // 停止させたスクリプトを元の状態に戻す
        for (int i = 0; i < scriptsToDisable.Length; i++)
        {
            // 登録欄が空なら飛ばす
            if (scriptsToDisable[i] == null)
            {
                continue;
            }

            // ポーズ前にONだったものはON、OFFだったものはOFFに戻す
            scriptsToDisable[i].enabled = originalEnabledStates[i];
        }

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