using Photon.Realtime;
using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    public Transform playerTransform; // 플레이어의 Transform을 할당
    public float height = 20.0f; // 카메라가 플레이어 위에서 유지할 높이

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 newPosition = playerTransform.position;
            newPosition.y += height;
            transform.position = newPosition;

            // 카메라를 항상 플레이어를 바라보도록 하려면 아래 줄을 주석 해제하세요.
            // transform.LookAt(playerTransform);
        }
    }
}
