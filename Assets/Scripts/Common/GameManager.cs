using BackEnd;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Refs")]
    public GameObject player;     // Player 최상위(프리팹 인스턴스)
    public Rigidbody2D playerRb;  // Player의 Rigidbody2D (UnitRoot에 있어도 OK)

    [Header("Runtime")]
    public int targetFps = 60;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject);
            return; 
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = targetFps;
    }

    public void BindPlayer(GameObject playerRoot)
    {
        player = playerRoot;
        playerRb = player ? player.GetComponentInChildren<Rigidbody2D>(true) : null;
    }

    // 뒤끝 자동 로그인 유무 판단(원하는 로직으로 교체)
    public bool TryAutoLogin()
    {
        var bro = Backend.BMember.LoginWithTheBackendToken();

        if (bro.IsSuccess())
        {
            Debug.Log("[Backend] 토큰 자동 로그인 성공");
            return true;
        }
        else
        {
            Debug.Log("[Backend] 토큰 자동 로그인 실패");
            return false;
        }
    }
}
