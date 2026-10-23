using UnityEngine;
using System.Collections;

public class ChipSpawner : MonoBehaviour
{
    [Header("チップPrefab")]
    public GameObject chipPrefab;

    [Header("チップ出現候補地点")]
    public Transform[] spawnPoints;

    [Header("何秒ごとに出現するか")]
    public float spawnInterval = 3f;

    [Header("同時に存在できる最大チップ数")]
    public int maxChips = 3;

    [Header("同じ場所に重ならないための判定範囲")]
    public float checkRadius = 0.5f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        // 最初に少し待つ
        yield return new WaitForSeconds(1f);

        while (true)
        {
            SpawnChip();

            yield return new WaitForSeconds(
                spawnInterval
            );
        }
    }

    void SpawnChip()
    {
        if (chipPrefab == null)
        {
            Debug.LogWarning(
                "Chip Prefabが設定されていません！"
            );

            return;
        }

        if (spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogWarning(
                "ChipのSpawn Pointがありません！"
            );

            return;
        }

        // 現在Sceneに存在しているChip数
        Chip[] currentChips =
            FindObjectsByType<Chip>(
                FindObjectsSortMode.None
            );

        // 最大数に達していたら生成しない
        if (currentChips.Length >= maxChips)
        {
            return;
        }

        // 最大10回までランダム地点を探す
        for (int i = 0; i < 10; i++)
        {
            int randomIndex =
                Random.Range(
                    0,
                    spawnPoints.Length
                );

            Transform point =
                spawnPoints[randomIndex];

            if (!HasChipNearPoint(point.position))
            {
                Instantiate(
                    chipPrefab,
                    point.position,
                    Quaternion.identity
                );

                return;
            }
        }
    }

    bool HasChipNearPoint(Vector2 position)
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                position,
                checkRadius
            );

        foreach (Collider2D hit in hits)
        {
            Chip chip =
                hit.GetComponent<Chip>();

            if (chip != null)
            {
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (spawnPoints == null)
        {
            return;
        }

        foreach (Transform point in spawnPoints)
        {
            if (point == null)
            {
                continue;
            }

            Gizmos.DrawWireSphere(
                point.position,
                checkRadius
            );
        }
    }
}