using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP, currentHP, attack, defense;
    public int expReward, goldReward;
    //public ItemDrop[] dropTable;    // 드랍 아이템 목록
    //public float dropRate;          // 드랍 확률

    public float speed = 2.5f;
    public float stopDistance = 0.4f;
    public float slowRadius = 0.8f;
    public Rigidbody2D target;
    public bool isLive;

    private Rigidbody2D rigid;
    private Collider2D coll;
    private Animator anim;

    // ★ 풀 반납을 위한 주입 필드
    private MemoryPool pool;
    private GameObject root;   // 풀에서 관리하는 루트(= Enemy 오브젝트)
    private bool returned;     // 중복 반납 방지

    public void Init(MemoryPool p, GameObject rootGo)
    {
        pool = p;
        root = rootGo;
        returned = false;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        target = GameManager.Instance.playerRb;
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        anim.SetBool("4_Death", false);
        currentHP = maxHP;
        returned = false; // 재활성화 시 플래그 리셋
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("3_Damage"))
            return;

        Vector2 to = target.position - rigid.position;
        float dist = to.magnitude;

        if (dist <= stopDistance)
        {
            rigid.linearVelocity = Vector2.zero;
            return;
        }

        float v = speed * (dist < slowRadius ? dist / slowRadius : 1f);
        rigid.linearVelocity = to * (v / Mathf.Max(dist, 0.0001f));
    }

    private void LateUpdate()
    {
        if (!isLive)
            return;

        Vector2 vel = rigid.linearVelocity;

        if (Mathf.Abs(vel.x) > 0.0001f)
            transform.localScale = vel.x > 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !isLive)
            return;

        currentHP -= 100f * Time.deltaTime;

        if (currentHP > 0f)
        {
            anim.SetTrigger("3_Damaged");
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            anim.SetBool("4_Death", true);
        }
    }

    // 애니메이션 이벤트에서 호출
    private void Dead()
    {
        // ❌ SetActive(false) 직접 호출 금지 (자식/부모 비활성화 금지)
        // ✅ 풀에 루트를 반납
        if (returned) return;
        returned = true;

        if (pool != null && root != null)
        {
            pool.DeactivatePoolItem(root);
        }
    }
}
