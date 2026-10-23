using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // ==========================================
    // 移動
    // ==========================================

    [Header("移動")]
    public float moveSpeed = 7f;

    // ==========================================
    // ジャンプ
    // ==========================================

    [Header("ジャンプ")]
    public float jumpPower = 12f;

    // ==========================================
    // 攻撃
    // ==========================================

    [Header("攻撃")]
    public Transform attackPoint;

    // 刀の攻撃範囲
    public float attackRange = 1.2f;

    // クリックしてから実際に判定が出るまでの時間
    public float attackDelay = 0.2f;

    // ==========================================
    // ダッシュ
    // ==========================================

    [Header("ダッシュ")]
    public float dashSpeed = 18f;

    // ダッシュ全体の時間
    public float dashTime = 0.18f;

    // 次のダッシュができるまでの時間
    public float dashCooldown = 0.6f;

    [Header("ダッシュ開始直後の無敵時間")]
    public float dashInvincibleTime = 0.07f;

    [Header("無敵中の透明度")]
    [Range(0f, 1f)]
    public float dashAlpha = 0.4f;

    // ==========================================
    // コンポーネント
    // ==========================================

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // ==========================================
    // 状態
    // ==========================================

    private float moveX;

    private bool isGrounded = false;

    private bool isDashing = false;

    private bool canDash = true;

    // 敵側から
    // 「今プレイヤーは無敵か？」
    // を確認できる
    public bool IsInvincible { get; private set; } = false;

    // Layer番号
    private int playerLayer;
    private int enemyLayer;

    // ==========================================
    // Start
    // ==========================================

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer =
            GetComponent<SpriteRenderer>();

        animator =
            GetComponent<Animator>();

        // Player / Enemy Layerを取得
        playerLayer =
            LayerMask.NameToLayer("Player");

        enemyLayer =
            LayerMask.NameToLayer("Enemy");
    }

    // ==========================================
    // Update
    // ==========================================

    void Update()
    {
        // ------------------------------------------
        // ダッシュ中は通常操作を受け付けない
        // ------------------------------------------

        if (isDashing)
        {
            return;
        }

        // ==========================================
        // 左右移動入力
        // ==========================================

        moveX = 0f;

        if (Keyboard.current != null)
        {
            // Aキー
            if (Keyboard.current.aKey.isPressed)
            {
                moveX = -1f;
            }

            // Dキー
            if (Keyboard.current.dKey.isPressed)
            {
                moveX = 1f;
            }
        }

        // ==========================================
        // キャラクターの左右反転
        // ==========================================

        // 左向き
        if (moveX < 0f)
        {
            spriteRenderer.flipX = true;

            SetAttackPointDirection(-1f);
        }

        // 右向き
        else if (moveX > 0f)
        {
            spriteRenderer.flipX = false;

            SetAttackPointDirection(1f);
        }

        // ==========================================
        // Idle / Run アニメーション
        // ==========================================

        animator.SetFloat(
            "Speed",
            Mathf.Abs(moveX)
        );

        // ==========================================
        // 左クリックで刀攻撃
        // ==========================================

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }

        // ==========================================
        // Spaceでジャンプ
        // ==========================================

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame &&
            isGrounded)
        {
            rb.linearVelocity =
                new Vector2(
                    rb.linearVelocity.x,
                    jumpPower
                );

            isGrounded = false;
        }

        // ==========================================
        // 左Shiftでダッシュ
        // ==========================================

        if (Keyboard.current != null &&
            Keyboard.current.leftShiftKey.wasPressedThisFrame &&
            canDash)
        {
            StartCoroutine(Dash());
        }
    }

    // ==========================================
    // FixedUpdate
    // ==========================================

    void FixedUpdate()
    {
        // ダッシュ中は
        // 通常移動で速度を上書きしない
        if (isDashing)
        {
            return;
        }

        rb.linearVelocity =
            new Vector2(
                moveX * moveSpeed,
                rb.linearVelocity.y
            );
    }

    // ==========================================
    // 攻撃
    // ==========================================

    void Attack()
    {
        // 攻撃アニメーション
        animator.SetTrigger("Attack");

        // 刀のアニメーションに合わせて
        // 少し遅れて判定を出す
        StartCoroutine(
            AttackAfterDelay()
        );
    }

    IEnumerator AttackAfterDelay()
    {
        // 刀を振るまで待つ
        yield return new WaitForSeconds(
            attackDelay
        );

        if (attackPoint == null)
        {
            yield break;
        }

        // 攻撃範囲に入っているColliderを取得
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange
            );

        foreach (Collider2D hit in hits)
        {
            // ======================================
            // ICチップだけ斬って回収
            // ======================================

            Chip chip =
                hit.GetComponent<Chip>();

            if (chip != null)
            {
                chip.Collect();
            }

            // 敵には何もしない
            // なので刀で敵を消すことはできない
        }
    }

    // ==========================================
    // ダッシュ
    // ==========================================

    IEnumerator Dash()
    {
        canDash = false;

        isDashing = true;

        // ------------------------------------------
        // Dashアニメーション開始
        // ------------------------------------------

        animator.SetBool(
            "IsDashing",
            true
        );

        // ==========================================
        // 現在向いている方向を取得
        // ==========================================

        float direction;

        // 左向き
        if (spriteRenderer.flipX)
        {
            direction = -1f;
        }

        // 右向き
        else
        {
            direction = 1f;
        }

        // ==========================================
        // ダッシュ開始
        // ==========================================

        rb.linearVelocity =
            new Vector2(
                direction * dashSpeed,
                0f
            );

        // ==========================================
        // ダッシュ開始直後だけ無敵
        // ==========================================

        IsInvincible = true;

        // ------------------------------------------
        // 半透明にする
        // ------------------------------------------

        Color color =
            spriteRenderer.color;

        color.a = dashAlpha;

        spriteRenderer.color =
            color;

        // ------------------------------------------
        // PlayerとEnemyの物理衝突を無視
        // ------------------------------------------

        if (playerLayer != -1 &&
            enemyLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(
                playerLayer,
                enemyLayer,
                true
            );
        }

        // 無敵時間
        yield return new WaitForSeconds(
            dashInvincibleTime
        );

        // ==========================================
        // 無敵終了
        // ==========================================

        IsInvincible = false;

        // ------------------------------------------
        // 透明度を元に戻す
        // ------------------------------------------

        color =
            spriteRenderer.color;

        color.a = 1f;

        spriteRenderer.color =
            color;

        // ------------------------------------------
        // Enemyとの衝突を復活
        // ------------------------------------------

        if (playerLayer != -1 &&
            enemyLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(
                playerLayer,
                enemyLayer,
                false
            );
        }

        // ==========================================
        // 残りのダッシュ時間
        // ==========================================

        float remainingDashTime =
            dashTime -
            dashInvincibleTime;

        if (remainingDashTime > 0f)
        {
            yield return new WaitForSeconds(
                remainingDashTime
            );
        }

        // ==========================================
        // ダッシュ終了
        // ==========================================

        isDashing = false;

        animator.SetBool(
            "IsDashing",
            false
        );

        // ダッシュの勢いを止める
        rb.linearVelocity =
            new Vector2(
                0f,
                rb.linearVelocity.y
            );

        // ==========================================
        // クールタイム
        // ==========================================

        yield return new WaitForSeconds(
            dashCooldown
        );

        canDash = true;
    }

    // ==========================================
    // AttackPointの左右切り替え
    // ==========================================

    void SetAttackPointDirection(
        float direction)
    {
        if (attackPoint == null)
        {
            return;
        }

        Vector3 position =
            attackPoint.localPosition;

        position.x =
            Mathf.Abs(position.x)
            * direction;

        attackPoint.localPosition =
            position;
    }

    // ==========================================
    // 地面に着地
    // ==========================================

    void OnCollisionEnter2D(
        Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // ==========================================
    // 地面に触れている間
    // ==========================================

    void OnCollisionStay2D(
        Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // ==========================================
    // 地面から離れた
    // ==========================================

    void OnCollisionExit2D(
        Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // ==========================================
    // Scene画面に攻撃範囲を表示
    // ==========================================

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}