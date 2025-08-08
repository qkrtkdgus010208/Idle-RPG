using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP;             // 최대 체력
    public float currentHP;         // 현재 체력
    public float attack;            // 공격력
    public float defense;           // 방어력

    public int expReward;           // 경험치 보상
    public int goldReward;          // 골드 보상
    //public ItemDrop[] dropTable;    // 드랍 아이템 목록
    //public float dropRate;          // 드랍 확률

    public float speed;         // 이동 속도
    public Rigidbody2D target;

    private Vector2 moveDir;
    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }

    private void LateUpdate()
    {
        animator.SetBool("1_Move", true);

        transform.localScale = target.position.x > rb.position.x ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    private void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
