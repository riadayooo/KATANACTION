using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // ==========================================
    // スコア表示
    // ==========================================

    [Header("スコアの数字表示")]
    public TMP_Text scoreText;

    // ==========================================
    // 敵出現設定
    // ==========================================

    [Header("何個チップを取るごとに敵を出すか")]
    public int chipsPerEnemy = 5;

    // ==========================================
    // 内部データ
    // ==========================================

    private int score = 0;

    private int spawnedEnemyCount = 0;

    // 他のスクリプトからスコアを取得するとき用
    public int Score
    {
        get
        {
            return score;
        }
    }

    // ==========================================
    // Awake
    // ==========================================

    void Awake()
    {
        Instance = this;
    }

    // ==========================================
    // Start
    // ==========================================

    void Start()
    {
        // 最初は0
        score = 0;

        UpdateScoreUI();
    }

    // ==========================================
    // チップを取ったとき
    // ==========================================

    public void AddScore(int amount)
    {
        // スコア加算
        score += amount;

        // UI更新
        UpdateScoreUI();

        Debug.Log(
            "現在のチップ数：" + score
        );

        // ======================================
        // 何体の敵が必要か計算
        // ======================================

        int targetEnemyCount =
            score / chipsPerEnemy;

        // ======================================
        // 必要な数まで敵を追加
        // ======================================

        while (
            spawnedEnemyCount <
            targetEnemyCount
        )
        {
            if (EnemySpawner.Instance != null)
            {
                EnemySpawner.Instance
                    .SpawnEnemy();

                spawnedEnemyCount++;

                Debug.Log(
                    "敵追加！ 合計：" +
                    spawnedEnemyCount +
                    "体"
                );
            }
            else
            {
                Debug.LogWarning(
                    "EnemySpawnerが見つかりません！"
                );

                break;
            }
        }
    }

    // ==========================================
    // スコアUI更新
    // ==========================================

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // 「SCORE : 」は表示しない
            // 数字だけ表示
            scoreText.text =
                score.ToString();
        }
    }
}