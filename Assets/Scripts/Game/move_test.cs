using UnityEngine;

public class move_test : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveInput = new Vector2(moveX, moveY).normalized;

        rb.linearVelocity = moveInput * 5f;

        animator.SetBool("1_Move", moveInput != Vector2.zero);

        // 좌우 반전
        if (moveX > 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (moveX < 0) transform.localScale = new Vector3(1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("2_Attack");
        }
    }
}
