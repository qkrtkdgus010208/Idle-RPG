using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab / Pool")]
    public GameObject enemyPrefab;         // SPUM으로 만든 Enemy 프리팹(UnitRoot)
    public Transform poolParent;           // 선택

    [Header("Respawn Rule")]
    public int maxAlive = 20;              // 최대 마리 수
    public float respawnInterval = 2f;     // 스폰 2초 주기

    [Header("Spawn Rect")]
    public Vector2 minXY = new(-9f, -4f);
    public Vector2 maxXY = new(9f, 0f);

    private MemoryPool pool;
    private int deathsPending;             // 직전 틱 이후 누적된 사망 수

    private void Awake()
    {
        pool = new MemoryPool(enemyPrefab, poolParent);
    }

    private IEnumerator Start()
    {
        // 처음에 최대치만큼 채움
        for (int i = 0; i < maxAlive; i++)
            ActivateOne();

        // 2초 주기 리스폰 루프
        while (true)
        {
            yield return new WaitForSeconds(respawnInterval);

            // 직전 주기 동안 죽은 수만큼만, 동시에 최대치 넘지 않도록 스폰
            int capacity = Mathf.Max(0, maxAlive - pool.ActiveCount);
            int toSpawn = Mathf.Min(deathsPending, capacity);

            for (int i = 0; i < toSpawn; i++)
                ActivateOne();

            deathsPending -= toSpawn;  // 남는 건 다음 틱으로 이월
        }
    }

    private void ActivateOne()
    {
        GameObject item = pool.ActivatePoolItem();
        if (!item) return;

        item.transform.position = GetSpawnPos();

        Enemy enemy = item.GetComponent<Enemy>();
        if (enemy && enemy.target == null && GameManager.Instance && GameManager.Instance.player)
            enemy.target = GameManager.Instance.player.GetComponentInChildren<Rigidbody2D>(true);
    }

    Vector2 GetSpawnPos()
    {
        float x = Random.Range(minXY.x, maxXY.x);
        float y = Random.Range(minXY.y, maxXY.y);
        return new Vector2(x, y);
    }
}
