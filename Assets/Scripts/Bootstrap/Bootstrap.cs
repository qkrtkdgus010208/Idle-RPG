using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject gameManagerPrefab;  // GameManager 프리팹(없으면 씬에 배치해도 됨)
    public GameObject playerPrefab;       // SPUM Player 프리팹(최상위, UnitRoot 포함)

    [Header("Flow")]
    public string initSceneName = "Init";
    public string gameSceneName = "Game";

    private void Awake()
    {
        // GameManager 생성/유지
        if (GameManager.Instance == null)
        {
            if (gameManagerPrefab != null)
                InstantiateAndKeep(gameManagerPrefab);
            else
                Debug.LogError("[Bootstrap] GameManagerPrefab이 비었습니다.");
        }

        // Player 인스턴스 생성/등록
        if (GameManager.Instance.player == null)
        {
            if (playerPrefab != null)
            {
                var player = InstantiateAndKeep(playerPrefab);
                GameManager.Instance.BindPlayer(player);
            }
            else
            {
                Debug.LogError("[Bootstrap] PlayerPrefab이 비었습니다.");
            }
        }
    }

    private GameObject InstantiateAndKeep(GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        DontDestroyOnLoad(go);
        return go;
    }

    private void Start()
    {
        bool auto = GameManager.Instance.TryAutoLogin();
        SceneManager.LoadScene(auto ? gameSceneName : initSceneName);
    }
}
