using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmearPaper : MonoBehaviour
{
    public bool isCompleted;
    private string objectTag; // 물체의 태그
    public float detectionRadius; // 감지 반경
    public Color highlightColor = Color.yellow; // 감지 반경 내에 있는 물체의 색상

    private Renderer objectRenderer; // 물체의 랜더러
    private Color originalColor; // 물체의 원래 색상

    private GameObject holding = null;
    public float clickCount; // 클릭한 횟수
    private GameObject playerObject; // 플레이어 오브젝트
    private void Start()
    {
        isCompleted = false;
        detectionRadius = 2f; // 감지 반경
        clickCount = 0f; // 클릭한 횟수
        objectRenderer = GetComponent<Renderer>(); // 물체의 랜더러 가져오기

        // 물체의 초기 색상 저장
        originalColor = objectRenderer.material.color;

        // 플레이어 오브젝트와 관련 컴포넌트 초기화
        playerObject = GameObject.FindGameObjectWithTag("Player");

    }

    private void FixedUpdate()
    {
        if (!isCompleted)
        {
            SetMission();
        }
    }
    

    // 미션 코드
    private void SmearObjectMission()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            if (holding == null)
            {
                // 오브젝트 선택 및 들기 로직
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 2f) && hit.collider.CompareTag("Paper"))
                {
                    PickUpObject(hit.collider.gameObject);
                }

            }
            else
            {
                clickCount += 1f; // 클릭한 횟수
                
                if (clickCount <= 8f) {
                    // 오브젝트 오염시키기 로직
                    SmearObject();
                }

                else if (clickCount > 8f)
                {
                    objectRenderer.material.color = Color.black;
                    holding.GetComponent<Rigidbody>().isKinematic = false;
                    holding.transform.SetParent(null); // 부모 관계 해제
                    holding = null;

                    isCompleted = true;
                }
            }
        
        }
    }

    // 미션 코드 - 잡기 로직
    private void PickUpObject(GameObject pickUpObject)
    {
        holding = pickUpObject;
        holding.transform.SetParent(playerObject.transform); // 플레이어의 자식으로 설정
        holding.transform.localPosition = new Vector3(0, 1, 2); // 들고 있는 위치 조정
        holding.GetComponent<Rigidbody>().isKinematic = true; // 물리 연산 비활성화
    }

    // 미션 코드 - 오염시키기 로직
    private void SmearObject()
    {
        float t = Mathf.Clamp01(clickCount / 10f); // 클릭 횟수에 비례한 보간값 계산
        Color startColor = objectRenderer.material.color;
        Color targetColor = Color.Lerp(startColor, Color.black, t);
        objectRenderer.material.color = targetColor;
    }
    
    // 미션 초기 코드
    private void SetMission() {
        if (playerObject != null)
        {
            // 플레이어와 이 객체 간의 거리를 계산
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);

            // 거리가 감지 반경보다 짧으면 플레이어가 인식 반경 안에 있음
            if (distanceToPlayer <= detectionRadius)
            {
                Debug.Log("Player detected!"); // 플레이어가 감지됨을 로그로 출력

                // 인식 반경 내에 있는 물체를 하이라이트하기
                HighlightObject();

                // 미션 코드 추가
                SmearObjectMission();
            }
            else
            {
                // 인식 반경 내에 없는 경우 원래 색상으로 되돌리기
                ResetHighlight();
            }
        }
    }

    // 미션 초기 코드 + 인식 반경 내에 있는 물체를 하이라이트하는 함수
    private void HighlightObject()
    {
        // 물체의 랜더러가 있을 경우에만 실행
        if (objectRenderer != null && holding == null)
        {
            objectRenderer.material.color = highlightColor; // 물체의 색상 변경
        }
    }

    // 미션 초기 코드 + 물체의 원래 색상으로 되돌리는 함수
    private void ResetHighlight()
    {
        // 물체의 랜더러가 있을 경우에만 실행
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor; // 물체의 색상을 원래 색상으로 변경
        }
    }
}
