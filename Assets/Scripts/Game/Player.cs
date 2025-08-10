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

    public float stopDistanceToTarget = 0.35f; // 표적에 가까워지면 정지하는 거리
    public Vector2 inputVec;

    private Animator anim;
    private Rigidbody2D rigid;
    private Scanner scanner;

    // 좌우 판정의 떨림 방지용
    private const float FaceEpsilon = 0.0001f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        scanner = GetComponent<Scanner>();
    }

    private void FixedUpdate()
    {
        // 1) 이동 방향 결정: 입력 우선, 없으면 자동 추적
        Vector2 moveDir = inputVec;

        bool hasInput = moveDir.sqrMagnitude > 0.0001f;
        if (!hasInput && scanner != null && scanner.nearestTarget != null)
        {
            Vector2 to = (Vector2)scanner.nearestTarget.position - rigid.position;
            float dist = to.magnitude;

            if (dist > stopDistanceToTarget)
                moveDir = to / Mathf.Max(dist, 0.0001f); // 정규화
            else
                moveDir = Vector2.zero;
        }

        // 2) 실제 이동
        Vector2 nextVec = rigid.position + moveDir * speed * Time.fixedDeltaTime;
        nextVec.x = Mathf.Clamp(nextVec.x, -9f, 9f);
        nextVec.y = Mathf.Clamp(nextVec.y, -4f, 0f);
        rigid.MovePosition(nextVec);
    }

    private void LateUpdate()
    {
        // 1) 입력 방향 우선
        float faceX = 0f;
        bool hasInput = inputVec.sqrMagnitude > FaceEpsilon;
        if (hasInput)
        {
            faceX = inputVec.x;
        }
        else
        {
            // 2) 자동 추적 중이면 타겟의 x 상대 위치로 판정
            Transform targetTf = (scanner != null) ? scanner.nearestTarget : null; // 락온을 쓰면 currentTarget으로 교체
            if (targetTf != null)
            {
                faceX = targetTf.position.x - rigid.position.x;
            }
            else
            {
                // 3) 마지막 보조: 실제 속도 방향
                Vector2 vel = rigid.linearVelocity;
                faceX = vel.x;
            }
        }

        if (Mathf.Abs(faceX) > FaceEpsilon)
        {
            // 스프라이트가 기본 왼쪽을 보고 있다면 이 라인을 반대로 바꾸세요.
            transform.localScale = faceX > 0f
                ? new Vector3(-1f, 1f, 1f)   // 오른쪽 볼 때
                : new Vector3(1f, 1f, 1f);   // 왼쪽 볼 때
        }
    }

    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || inputVec.sqrMagnitude > 0.0001f) 
            return;

        anim.SetTrigger("2_Attack");
    }
}
