using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    // Resetボタンから呼び出す関数
    public void Reset()
    {
        // Time.timeScaleを1に戻す
        Time.timeScale = 1f;

        // 現在開いているSceneを取得する
        Scene currentScene = SceneManager.GetActiveScene();

        // 現在のSceneをもう一度読み込む
        SceneManager.LoadScene(currentScene.name);
    }
}
