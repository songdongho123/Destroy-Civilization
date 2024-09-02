using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilt : MonoBehaviour
{
    public Transform player; // 플레이어의 위치
    public float tiltForce = 500f;


    public float pickUpRange = 2.0f; // 플레이어와 객체 사이의 최대 거리
    public float throwForce = 600f; // 물체를 던질 힘
    // private bool isHolding = false; // 객체를 들고 있는지 여부




    void OnMouseOver()
    {
        if (player) {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= pickUpRange)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TiltBucket();
                }
            }
        }
    }



    private void TiltBucket()
    {
        // 양동이 엎기 로직
        Rigidbody bucketRigidbody = gameObject.GetComponent<Rigidbody>();
        if (bucketRigidbody != null)
        {
            // 회전력을 추가하여 천천히 엎을 수도 있습니다.
            bucketRigidbody.AddTorque(Vector3.forward * tiltForce); // 전방축을 기준으로 회전력 추가
            // StartCoroutine(ReleaseFish(gameObject));

             // Mesh Collider에도 Rigidbody를 추가하여 물리 시뮬레이션에 참여하도록 합니다.
            MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
            if (meshCollider != null)
            {
                meshCollider.enabled = true; // Mesh Collider 활성화
                Rigidbody meshRigidbody = gameObject.GetComponent<Rigidbody>();
                if (meshRigidbody != null)
                {
                    meshRigidbody.isKinematic = false; // 물리 시뮬레이션에 영향을 받도록 isKinematic 비활성화
                }
            }
        }
    }


    private IEnumerator ReleaseFish(GameObject bucket)
    {
        // 엎어진 후 잠시 기다렸다가 물고기를 떨어뜨리는 것을 시뮬레이션
        yield return new WaitForSeconds(1); // 1초 대기

        // 양동이의 모든 물고기에 접근
        foreach (Transform fish in bucket.transform)
        {
            // 물고기 오브젝트에 Rigidbody가 있다면
            Rigidbody fishRigidbody = fish.GetComponent<Rigidbody>();
            if (fishRigidbody != null)
            {
                fishRigidbody.isKinematic = false; // 물리 연산 활성화
                fishRigidbody.AddForce(Vector3.up * 5, ForceMode.VelocityChange); // 위로 약간 힘을 가해 물고기가 튀어오르게 함
            }
        }
    }
}
