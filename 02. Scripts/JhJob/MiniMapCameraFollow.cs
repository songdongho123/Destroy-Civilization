using Photon.Realtime;
using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    public Transform playerTransform; // �÷��̾��� Transform�� �Ҵ�
    public float height = 20.0f; // ī�޶� �÷��̾� ������ ������ ����

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 newPosition = playerTransform.position;
            newPosition.y += height;
            transform.position = newPosition;

            // ī�޶� �׻� �÷��̾ �ٶ󺸵��� �Ϸ��� �Ʒ� ���� �ּ� �����ϼ���.
            // transform.LookAt(playerTransform);
        }
    }
}
