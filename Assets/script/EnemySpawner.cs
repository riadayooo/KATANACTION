using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("“GPrefab")]
    public GameObject enemyPrefab;

    [Header("“G‚ÌoŒ»êŠ")]
    public Transform[] spawnPoints;

    [Header("’x‰„ƒRƒs[İ’è")]
    public float firstEnemyDelay = 0.8f;
    public float delayIncrease = 0.6f;

    private int spawnedEnemyCount = 0;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy Prefab‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñI");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Spawn Point‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñI");
            return;
        }

        // ƒ‰ƒ“ƒ_ƒ€‚ÈoŒ»êŠ
        int randomIndex =
            Random.Range(0, spawnPoints.Length);

        Transform spawnPoint =
            spawnPoints[randomIndex];

        // “G¶¬
        GameObject newEnemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        // “G‚²‚Æ‚É’x‰„ŠÔ‚ğ•Ï‚¦‚é
        DelayedCopyEnemy copyEnemy =
            newEnemy.GetComponent<DelayedCopyEnemy>();

        if (copyEnemy != null)
        {
            copyEnemy.delay =
                firstEnemyDelay +
                (spawnedEnemyCount * delayIncrease);
        }

        spawnedEnemyCount++;

        Debug.Log(
            "“GoŒ»I Œ»İ‚Ì“G”F" +
            spawnedEnemyCount +
            " / DelayF" +
            copyEnemy.delay
        );
    }
}