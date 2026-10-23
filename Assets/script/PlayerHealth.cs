using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("ライフ")]
    public int maxHealth = 3;

    [Header("被弾後の無敵時間")]
    public float damageInvincibleTime = 1f;

    [Header("ハートUI")]
    public Image[] hearts;

    [Header("ハート画像")]
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private int currentHealth;

    private bool damageInvincible = false;
    private bool isDead = false;

    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;

        playerMovement =
            GetComponent<PlayerMovement>();

        spriteRenderer =
            GetComponent<SpriteRenderer>();

        UpdateHearts();
    }

    public void TakeDamage()
    {
        if (isDead)
        {
            return;
        }

        if (damageInvincible)
        {
            return;
        }

        // ダッシュ無敵
        if (playerMovement != null &&
            playerMovement.IsInvincible)
        {
            return;
        }

        currentHealth--;

        Debug.Log(
            "ダメージ！ 残りHP：" +
            currentHealth
        );

        UpdateHearts();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(
            DamageInvincible()
        );
    }

    IEnumerator DamageInvincible()
    {
        damageInvincible = true;

        for (int i = 0; i < 5; i++)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }

            yield return new WaitForSeconds(0.1f);

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }

            yield return new WaitForSeconds(0.1f);
        }

        damageInvincible = false;
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
            {
                continue;
            }

            if (i < currentHealth)
            {
                hearts[i].sprite =
                    fullHeart;
            }
            else
            {
                hearts[i].sprite =
                    emptyHeart;
            }
        }
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("GAME OVER!");

        // 点滅中に死んでも表示を戻す
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        // GAME OVER画面を出す
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.ShowGameOver();
        }
        else
        {
            Debug.LogError(
                "GameOverManagerがSceneにありません！"
            );
        }
    }

    void OnTriggerEnter2D(
        Collider2D other)
    {
        if (other.gameObject.layer ==
            LayerMask.NameToLayer("Enemy"))
        {
            TakeDamage();
        }
    }
}