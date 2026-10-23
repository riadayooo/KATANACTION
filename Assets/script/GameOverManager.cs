using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    // ==========================================
    // GAME OVER UI
    // ==========================================

    private GameObject gameOverPanel;

    private TMP_Text finalScoreText;

    private Button retryButton;

    // ==========================================
    // プレイ中のUI
    // ==========================================

    private GameObject scoreTextObject;

    private GameObject hpPanel;

    // ==========================================
    // Awake
    // ==========================================

    void Awake()
    {
        Instance = this;

        // Retry後などのために時間を元に戻す
        Time.timeScale = 1f;

        // Canvas取得
        Canvas canvas =
            GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError(
                "Canvasが見つかりません！"
            );

            return;
        }

        // OFFになっているオブジェクトも含めて検索
        Transform[] allObjects =
            canvas.GetComponentsInChildren<Transform>(true);

        foreach (Transform obj in allObjects)
        {
            // ==================================
            // GameOverPanel
            // ==================================

            if (obj.name == "GameOverPanel")
            {
                gameOverPanel =
                    obj.gameObject;
            }

            // ==================================
            // 最終スコア数字
            // ==================================

            if (obj.name == "FinalScoreText")
            {
                finalScoreText =
                    obj.GetComponent<TMP_Text>();
            }

            // ==================================
            // Retryボタン
            // ==================================

            if (obj.name == "RetryButton")
            {
                retryButton =
                    obj.GetComponent<Button>();
            }

            // ==================================
            // プレイ中のスコア表示
            // ==================================

            if (obj.name == "ScoreText")
            {
                scoreTextObject =
                    obj.gameObject;
            }

            // ==================================
            // ハートUI
            // ==================================

            if (obj.name == "HPPanel")
            {
                hpPanel =
                    obj.gameObject;
            }
        }

        // ==========================================
        // Retryボタンを自動接続
        // ==========================================

        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();

            retryButton.onClick.AddListener(
                RetryGame
            );
        }

        // ==========================================
        // 最初はGAME OVERを非表示
        // ==========================================

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogError(
                "GameOverPanelが見つかりません！"
            );
        }

        // プレイ中UIは表示
        if (scoreTextObject != null)
        {
            scoreTextObject.SetActive(true);
        }

        if (hpPanel != null)
        {
            hpPanel.SetActive(true);
        }
    }

    // ==========================================
    // GAME OVER
    // ==========================================

    public void ShowGameOver()
    {
        Debug.Log(
            "GAME OVER画面を表示！"
        );

        // ==========================================
        // スコア取得
        // ==========================================

        int score = 0;

        if (ScoreManager.Instance != null)
        {
            score =
                ScoreManager.Instance.Score;
        }

        // ==========================================
        // 数字だけ表示
        // ==========================================

        if (finalScoreText != null)
        {
            finalScoreText.text =
                score.ToString();
        }

        // ==========================================
        // プレイ中UIを消す
        // ==========================================

        if (scoreTextObject != null)
        {
            scoreTextObject.SetActive(false);
        }

        if (hpPanel != null)
        {
            hpPanel.SetActive(false);
        }

        // ==========================================
        // GAME OVER UI表示
        // ==========================================

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // 一番手前に表示
            gameOverPanel.transform.SetAsLastSibling();
        }

        Canvas.ForceUpdateCanvases();

        // ==========================================
        // ゲーム停止
        // ==========================================

        Time.timeScale = 0f;
    }

    // ==========================================
    // RETRY
    // ==========================================

    public void RetryGame()
    {
        // 時間を戻す
        Time.timeScale = 1f;

        // 現在のGameSceneを最初から読み直す
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}