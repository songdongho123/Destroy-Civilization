using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 사용 방법
 * 1. arrowObject 하나를 Hierarchy에 만들어줍니다 (화살표 Texture)
 * 2. 해당 오브젝트에 Rotation을 80으로, Position을 2로 준 뒤, 각각의 취향에 맞게 조절합니다.
 * 3. 해당 오브젝트에 현 스크립트 파일을 Add한 뒤 arrowObject에 이동시킬 오브젝트를 드래그하여 사용합니다.
 * 현재 코드에서는 플레이어의 사망을 true로 지정하고 0,0,0으로 임의 사용하고 있으나, 
 * 이후 사용하실 분은 deathPosition에 죽은 캐릭터 위치를 포톤 코드를 통해 받아 사용하시면 됨미당
 */
public class Detective : MonoBehaviour
{
    public Transform arrowObject; // 화살표 오브젝트 (Unity 인스펙터에서 할당)
    public Vector3 deathPosition; // 죽은 오브젝트의 위치
    private bool isPlayerDead = true; // 플레이어의 사망 상태

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isPlayerDead)
        {
            // 죽은 오브젝트의 위치가 화면 내에 있는지 확인
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(deathPosition);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            arrowObject.gameObject.SetActive(true);

            if (onScreen)
            {
                spriteRenderer.color = Color.red;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }

    // 플레이어 사망과 관련된 로직을 처리하는 함수
    public void OnPlayerDeath(Vector3 position)
    {
        deathPosition = position;
        isPlayerDead = true;
    }
}
