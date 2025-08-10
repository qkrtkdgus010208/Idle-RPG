using UnityEngine;
using UnityEngine.Rendering; // SortingGroup

[RequireComponent(typeof(SortingGroup))]
public class YSortOrder : MonoBehaviour
{
    public string sortingLayerName = "Default"; // 둘 다 같은 레이어 사용
    public int orderOffset = 0;                 // 필요시 보정(플레이어만 +5 등)
    public float scale = 100f;                  // 월드Y -> 정수 맵핑 배율

    SortingGroup sg;

    void Awake()
    {
        sg = GetComponent<SortingGroup>();
        if (!string.IsNullOrEmpty(sortingLayerName))
            sg.sortingLayerName = sortingLayerName;
    }

    void LateUpdate()
    {
        // Y가 낮을수록 화면 앞: 부호에 주의(-)
        sg.sortingOrder = Mathf.RoundToInt(-transform.position.y * scale) + orderOffset;
    }
}
