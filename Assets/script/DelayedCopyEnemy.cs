using UnityEngine;

public class DelayedCopyEnemy : MonoBehaviour
{
    [Header("コピーの遅延時間")]
    public float delay = 0.8f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (PlayerHistoryRecorder.Instance == null)
        {
            return;
        }

        float targetTime =
            Time.time - delay;

        PlayerHistoryRecorder.PlayerRecord record;

        bool found =
            PlayerHistoryRecorder.Instance.GetRecord(
                targetTime,
                out record
            );

        if (!found)
        {
            return;
        }

        // -------------------------
        // 位置コピー
        // -------------------------

        transform.position =
            record.position;

        // -------------------------
        // 向きコピー
        // -------------------------

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX =
                record.flipX;
        }

        // -------------------------
        // アニメーションコピー
        // -------------------------

        if (animator != null)
        {
            animator.SetFloat(
                "Speed",
                record.speed
            );

            animator.SetBool(
                "IsDashing",
                record.isDashing
            );
        }
    }
}