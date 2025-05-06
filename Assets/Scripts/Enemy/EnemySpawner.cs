using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int maxEnemies = 10;
    public Transform player;
    public BoxCollider2D Spawnabl; // doğma alanı

    private int currentEnemyCount = 0;

    private void Start()
    {
        if (Spawnabl == null)
        {
            Debug.LogError("Spawnabl (BoxCollider2D) atanmadı.");
            return;
        }

        InvokeRepeating(nameof(SpawnEnemy), 2f, 5f);
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;

        Vector3 spawnPos = GetRandomPointInArea();

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);

        EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript != null)
            enemyScript.SetTarget(player);

        currentEnemyCount++;
    }

    Vector3 GetRandomPointInArea()
    {
        Bounds bounds = Spawnabl.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, 0f);
    }

    // sahnede kutuyu gösterelim
    private void OnDrawGizmos()
    {
        if (Spawnabl != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Spawnabl.bounds.center, Spawnabl.bounds.size);
        }
    }
}
