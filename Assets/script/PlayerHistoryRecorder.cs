using UnityEngine;
using System.Collections.Generic;

public class PlayerHistoryRecorder : MonoBehaviour
{
    public static PlayerHistoryRecorder Instance;

    public class PlayerRecord
    {
        public float time;
        public Vector2 position;
        public bool flipX;

        public float speed;
        public bool isDashing;
    }

    private List<PlayerRecord> history = new List<PlayerRecord>();

    [Header("‰½•b•ª•Û‘¶‚·‚é‚©")]
    public float recordTime = 10f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerRecord record = new PlayerRecord();

        record.time = Time.time;
        record.position = transform.position;

        if (spriteRenderer != null)
        {
            record.flipX = spriteRenderer.flipX;
        }

        if (animator != null)
        {
            record.speed = animator.GetFloat("Speed");
            record.isDashing = animator.GetBool("IsDashing");
        }

        history.Add(record);

        // ŒÃ‚¢‹L˜^‚ðíœ
        while (history.Count > 0 &&
               Time.time - history[0].time > recordTime)
        {
            history.RemoveAt(0);
        }
    }

    public bool GetRecord(
        float targetTime,
        out PlayerRecord record)
    {
        record = null;

        if (history.Count == 0)
        {
            return false;
        }

        for (int i = history.Count - 1; i >= 0; i--)
        {
            if (history[i].time <= targetTime)
            {
                record = history[i];
                return true;
            }
        }

        return false;
    }
}