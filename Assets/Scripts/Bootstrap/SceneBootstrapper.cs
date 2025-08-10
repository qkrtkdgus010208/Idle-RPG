using UnityEngine;
using Unity.Cinemachine;   // CM3 API

public class SceneBootstrapper : MonoBehaviour
{
    private System.Collections.IEnumerator Start()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || gm.player == null) yield break;

        // 스폰: 위치만 적용 (회전 X)
        gm.player.transform.position = transform.position;

        // 한 프레임 대기(카메라 초기화 이후에 바인딩)
        yield return null;

        // CM3: 카메라의 CameraTarget 구조체로 TrackingTarget 설정
        CinemachineCamera cmCam = Object.FindFirstObjectByType<CinemachineCamera>();
        if (cmCam != null)
        {
            CameraTarget target = cmCam.Target; // struct
            target.TrackingTarget = gm.playerRb.transform;          // 리지드바디(=UnitRoot)의 Transform을 추적
            cmCam.Target = target;                                   // 구조체라 다시 대입해야 반영됨

            // Confiner2D 캐시 무효화(바운딩 셰이프는 인스펙터에서 지정)
            CinemachineConfiner2D conf2D = cmCam.GetComponent<CinemachineConfiner2D>();
            if (conf2D != null)
                conf2D.InvalidateBoundingShapeCache();
        }

        // EnemySpawner들에 타깃 주입
        EnemySpawner[] spawners = Object.FindObjectsByType<EnemySpawner>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (EnemySpawner sp in spawners)
            sp.SetPlayerTarget(gm.playerRb);
    }
}
