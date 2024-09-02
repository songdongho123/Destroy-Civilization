using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float pickUpRange = 2.0f; // 플레이어와 객체 사이의 최대 거리
    public float throwForce = 600f; // 물체를 던질 힘
    public Transform player; // 플레이어의 위치
    private bool isHolding = false; // 객체를 들고 있는지 여부


    void OnMouseOver()
    {
        if (player) {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= pickUpRange)
            {
                if (Input.GetMouseButtonDown(0) && !isHolding)
                {
                    StartCoroutine(PickUpObject());
                    // PickUpObject();
                }
                else if (Input.GetMouseButtonDown(0) && isHolding)
                {
                    StartCoroutine(ThrowObject());
                }
            }
        }
    }

    IEnumerator PickUpObject()
    {
        isHolding = true;
        transform.SetParent(player); // 객체를 플레이어의 자식으로 설정
        transform.localPosition = new Vector3(0, 0, 2); // 들고 있는 위치 조정
        GetComponent<Rigidbody>().isKinematic = true; // 물리 연산 비활성화
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator ThrowObject()
    {
        isHolding = false;
        transform.SetParent(null); // 부모 관계 해제

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(player.forward * throwForce); // 오브젝트 던지기
        yield return new WaitForSeconds(.5f);
    }
}
