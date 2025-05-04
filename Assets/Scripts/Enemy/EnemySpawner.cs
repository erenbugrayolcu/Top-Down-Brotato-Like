using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // farklı düşman türleri
    public float spawnRadius = 20f;
    public int maxEnemies = 10;
    public Transform player;

    private int currentEnemyCount = 0;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, 5f);
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;

        Vector3 spawnPos = GetRandomSpawnPosition();

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);
        
        EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript != null)
            enemyScript.SetTarget(player);

        currentEnemyCount++;
    }

    Vector3 GetRandomSpawnPosition()
{
    Vector3 spawnPosition;
    int maxAttempts = 10; // Rastgele pozisyon bulmak için maksimum deneme sayısı
    int attempts = 0;

    do
    {
        Vector2 randomPos = Random.insideUnitCircle.normalized * spawnRadius;
        spawnPosition = player.position + new Vector3(randomPos.x, randomPos.y, 0);
        spawnPosition.z = 0f;

        attempts++;
    }
    while (!IsPositionOnGround(spawnPosition) && attempts < maxAttempts);

    return spawnPosition;
}

bool IsPositionOnGround(Vector3 position)
{
    // "Ground" layer'ını kontrol etmek için bir 2D çember çarpışma testi
    Collider2D hit = Physics2D.OverlapPoint(position, LayerMask.GetMask("Ground"));
    return hit != null;
}
}
