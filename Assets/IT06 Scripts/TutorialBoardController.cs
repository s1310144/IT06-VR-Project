using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialBoardController : MonoBehaviour
{

    [Header("Tutorial Text")]
    [SerializeField] private TMP_Text tutorialText;     // Tutorial_Board内の説明テキスト

    [Header("Tutorial Buttons")]
    [SerializeField] private GameObject backButton;     // 前のページへ戻るボタン
    [SerializeField] private GameObject nextButton;     // 次のページへ進むボタン
    [SerializeField] private GameObject startButton;    // ゲーム開始ボタン

    [Header("Tutorial Pages")]
    [TextArea(3, 8)]
    [SerializeField] private string[] tutorialMessages; // 説明文をページごとに入れる配列

    private int pageIndex = 0; // 現在表示しているページ番号

    private void Start()
    {
        
    }

    void OnEnable()
    {
        pageIndex = 0;
        UpdateTutorialPage();
    }

    public void NextPage()
    {
        // 最後のページでなければ、次のページへ進む
        if (pageIndex < tutorialMessages.Length - 1)
        {
            pageIndex++;
            UpdateTutorialPage();
        }
    }

    public void BackPage()
    {
        // 1ページ目でなければ、前のページへ戻る
        if (pageIndex > 0)
        {
            pageIndex--;
            UpdateTutorialPage();
        }
    }

    private void UpdateTutorialPage()
    {
        // 説明文が1つもない場合は何もしない
        if (tutorialMessages == null || tutorialMessages.Length == 0)
        {
            return;
        }

        // 現在のページ番号に対応した説明文を表示する
        if (tutorialText != null)
        {
            tutorialText.text = tutorialMessages[pageIndex];
        }

        // 1ページ目ではBackを消す
        if (backButton != null)
        {
            backButton.SetActive(pageIndex > 0);
        }

        // 最後のページではNextを消す
        if (nextButton != null)
        {
            nextButton.SetActive(pageIndex < tutorialMessages.Length - 1);
        }

    }
}
