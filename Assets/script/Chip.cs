using UnityEngine;
using UnityEngine.SceneManagement;

public class Chip : MonoBehaviour
{
    private bool collected = false;

    public void Collect()
    {
        if (collected)
        {
            return;
        }

        collected = true;

        // スコアを1増やす
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1);
        }

        Debug.Log("ICチップを回収した！");

        // チップを消す
        Destroy(gameObject);
    }
}