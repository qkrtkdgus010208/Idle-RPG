using UnityEngine;
using UnityEngine.InputSystem;

public class move_test : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public Vector2 inputVec;
    public float speed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);      
    }

    private void LateUpdate()
    {
        animator.SetBool("1_Move", inputVec != Vector2.zero);

        // 좌우 반전
        if (inputVec.x != 0)
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
}
