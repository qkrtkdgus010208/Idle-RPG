using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab / Pool")]
    public GameObject enemyPrefab;
    public Transform poolParent;

    [Header("Respawn Rule")]
    public int maxAlive = 20;
    public float respawnInterval = 3f;

    [Header("Spawn Rect")]
    public Vector2 minXY = new Vector2(-9f, -4f);
    public Vector2 maxXY = new Vector2(9f, 0f);

    private MemoryPool pool;
    private Rigidbody2D cachedPlayerRb;

    public void SetPlayerTarget(Rigidbody2D rb) { cachedPlayerRb = rb; }

    private void Awake()
    {
        pool = new MemoryPool(enemyPrefab, poolParent);
    }

    private IEnumerator Start()
    {
        // 초기 채우기
        for (int i = 0; i < maxAlive; i++)
            ActivateOne();

        WaitForSeconds wait = new WaitForSeconds(respawnInterval);

        while (true)
        {
            yield return wait;

            // 꺼져있는 수만큼 보충
            int capacity = Mathf.Max(0, maxAlive - pool.ActiveCount);
            for (int i = 0; i < capacity; i++)
                ActivateOne();
        }
    }

    private void ActivateOne()
    {
        GameObject item = pool.ActivatePoolItem();

        if (item == null) 
            return;

        Vector2 spawnPos = GetSpawnPos();
        item.transform.GetChild(0).position = spawnPos;
        
        // 타깃/풀 주입
        Enemy enemy = item.GetComponentInChildren<Enemy>(true);
        if (enemy != null)
        {
            enemy.Init(pool, item); // ★ 훅 없이 풀/루트만 주입
            if (cachedPlayerRb != null)
                enemy.target = cachedPlayerRb; // 필요시 덮어쓰기
        }
    }

    private Vector2 GetSpawnPos()
    {
        float x = Random.Range(minXY.x, maxXY.x);
        float y = Random.Range(minXY.y, maxXY.y);
        return new Vector2(x, y);
    }
}
