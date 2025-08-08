using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float maxHP;             // 최대 체력
    public float currentHP;         // 현재 체력
    public float attack;            // 공격력
    public float defense;           // 방어력
    public float speed;             // 이동 속도


    public int maxExp;           // 최대 경험치
    public int currentExp;       // 현재 경험치
    public int gold;             // 골드
    public int gold2;            // 
    public int gold3;            // 
 
    private Vector2 inputVec;
    private Animator animator;
    private Rigidbody2D rb;
    private Scanner scanner;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        scanner = GetComponent<Scanner>();
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = rb.position + inputVec * speed * Time.fixedDeltaTime;

        nextVec.x = Mathf.Clamp(nextVec.x, -9f, 9f);
        nextVec.y = Mathf.Clamp(nextVec.y, -4f, 0f);

        rb.MovePosition(nextVec);      
    }

    private void LateUpdate()
    {
        animator.SetBool("1_Move", inputVec != Vector2.zero);

        // 좌우 반전
        if (Mathf.Abs(inputVec.x) > 0.0001f)
        {
            transform.localScale = inputVec.x > 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        }
    }

    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    public void OnAttackButton()
    {
        animator.SetTrigger("2_Attack");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }
}
