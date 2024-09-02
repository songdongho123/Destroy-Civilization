/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ThrowOfficeSupplies : MonoBehaviour
{
    public bool isCompleted;
    private string objectTag; // 물체의 태그
    public float detectionRadius; // 감지 반경
    public Color highlightColor = Color.yellow; // 감지 반경 내에 있는 물체의 색상

    private Renderer objectRenderer; // 물체의 랜더러
    private Color originalColor; // 물체의 원래 색상

    private GameObject holding = null;
    public float throwForce; // 오브젝트를 던질 때 적용할 힘

    private GameObject playerObject; // 플레이어 오브젝트

    private void Start()
    {
        isCompleted = false;
        detectionRadius = 2f; // 감지 반경
        throwForce = 1500; // 오브젝트를 던질 때 적용할 힘
        objectRenderer = GetComponent<Renderer>(); // 물체의 랜더러 가져오기

        // 물체의 초기 색상 저장
        originalColor = objectRenderer.material.color;

        // 플레이어를 찾음
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!isCompleted)
        {
            SetMission();
        }
    }
    

    // 미션 코드
    private void ThrowOfficeSuppliesMission()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            if (holding == null)
            {
                // 오브젝트 선택 및 들기 로직
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 2f) && hit.collider.CompareTag("OfficeSupply"))
                {
                    PickUpObject(hit.collider.gameObject);
                }
            }
            else
            {
                // 오브젝트 던지기 로직
                ThrowObject();
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

    // 미션 코드 - 던지기 로직
    private void ThrowObject()
    {
        holding.GetComponent<Rigidbody>().isKinematic = false;
        holding.transform.SetParent(null); // 부모 관계 해제
        holding.GetComponent<Rigidbody>().AddForce(playerObject.transform.forward * throwForce); // 오브젝트 던지기
        holding = null; // 참조 초기화
        isCompleted = true;
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
                ThrowOfficeSuppliesMission();
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
*/


using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class ThrowOfficeSupplies : MonoBehaviourPunCallbacks
    {
        public float pickUpRange = 2.0f; // 플레이어와 객체 사이의 최대 거리
        public float throwForce = 600f; // 물체를 던질 힘
        private GameObject objectToPickUp; // 플레이어가 집어 올릴 객체
        private GameObject pickedUpObject; // 현재 들고 있는 객체
        private bool isHolding = false; // 객체를 들고 있는지 여부


        private Material originalMaterial;
        private Renderer targetRenderer;

        public Material highlightMaterial; 

        QuestManager questManager;

        void Start()
        {
            // QuestManager 인스턴스를 찾아서 할당합니다.
            questManager = FindObjectOfType<QuestManager>();
            highlightMaterial = Resources.Load<Material>("HighlightMaterial"); 
        }


        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isHolding)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, pickUpRange))
                {
                    if (hit.transform.gameObject.CompareTag("Office"))
                    {
                        StartCoroutine(PickUpObject(hit.transform.gameObject));
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0) && isHolding && pickedUpObject != null)
            {
                StartCoroutine(ThrowObject(pickedUpObject));
            }
        
        
        if(GetComponent<PhotonView>().IsMine) {
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.transform.gameObject.CompareTag("Office"))
                {
                    // 하이라이트할 오브젝트의 렌더러 컴포넌트를 가져옵니다.
                    Renderer hitRenderer = hit.transform.gameObject.GetComponent<Renderer>();
                    if (targetRenderer != hitRenderer) // 이전 프레임과 다른 오브젝트라면
                    {
                        // 이전에 하이라이트된 오브젝트가 있으면 원래 머터리얼로 복원합니다.
                        if (targetRenderer != null)
                        {
                            targetRenderer.material = originalMaterial;
                        }

                        // 새로운 오브젝트를 하이라이트합니다.
                        targetRenderer = hitRenderer;
                        originalMaterial = targetRenderer.material; // 원본 머터리얼 저장
                        targetRenderer.material = highlightMaterial; // 하이라이트 머터리얼로 변경
                    }
                }
                else if (targetRenderer != null) // 오브젝트에서 마우스가 벗어났다면
                {
                    // 원래 머터리얼로 복원하고 참조를 초기화합니다.
                    targetRenderer.material = originalMaterial;
                    targetRenderer = null;
                }
            }
            else if (targetRenderer != null) // 오브젝트에서 마우스가 벗어났다면
            {
                // 원래 머터리얼로 복원하고 참조를 초기화합니다.
                targetRenderer.material = originalMaterial;
                targetRenderer = null;
            }

            // 기존의 코드...
        }
    }
            
   
        
}


        




        IEnumerator PickUpObject(GameObject objectToPick)
        {
            isHolding = true;
            pickedUpObject = objectToPick;

            // 객체를 플레이어의 자식으로 설정
            pickedUpObject.transform.SetParent(this.transform);
            pickedUpObject.transform.localPosition = new Vector3(0, 0, 2); // 들고 있는 위치 조정
            pickedUpObject.GetComponent<Rigidbody>().isKinematic = true; // 물리 연산 비활성화
            Debug.Log(photonView.ViewID);
            photonView.RPC("PickUpObjectRPC", RpcTarget.Others, pickedUpObject.GetComponent<PhotonView>().ViewID,this.GetComponent<PhotonView>().ViewID);

            yield return new WaitForSeconds(.5f);
        }


        IEnumerator ThrowObject(GameObject objectToThrow)
        {
            isHolding = false;

            // 부모 관계 해제
            objectToThrow.transform.SetParent(null);

            Rigidbody rb = objectToThrow.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(this.transform.forward * throwForce); // 오브젝트 던지기

            PhotonView pv = objectToThrow.GetComponent<PhotonView>();
            PhotonView tv = this.GetComponent<PhotonView>();
        


            if (questManager != null)
            {
                
                questManager.CompleteQuest(tv.ViewID, nameof(Pick));
                Debug.Log("퀘스트 완료");
                Destroy(this);
                // StartCoroutine(DelayedDestroyAndCreate(objectToThrow.GetComponent<PhotonView>().ViewID));
            }
            else
            {
                Debug.LogError("QuestManager not found!");
            }

            pickedUpObject = null; // 참조 초기화

            Debug.Log("확인2");
            photonView.RPC("ThrowObjectRPC", RpcTarget.Others, objectToThrow.GetComponent<PhotonView>().ViewID);
        

            yield return new WaitForSeconds(.5f);
        }


        [PunRPC]
        public void PickUpObjectRPC(int viewID, int viewID_user)
        {
            Debug.Log("던지기");
            PhotonView objectPV = PhotonView.Find(viewID);
            PhotonView userPV = PhotonView.Find(viewID_user);
            Debug.Log(objectPV.ViewID);
            Debug.Log(userPV.ViewID);
            if (objectPV != null)
            {
                // GameObject 참조를 PhotonView를 통해 얻습니다.
                GameObject objectToPick = objectPV.gameObject;

                isHolding = true;
                pickedUpObject = objectToPick;

                // 객체를 플레이어의 자식으로 설정
                pickedUpObject.transform.SetParent(userPV.transform);
                pickedUpObject.transform.localPosition = new Vector3(0, 0, 2); // 들고 있는 위치 조정
                pickedUpObject.GetComponent<Rigidbody>().isKinematic = true; // 물리 연산 비활성화
            }
        }

        [PunRPC]
        public void ThrowObjectRPC(int viewID)
        {
            
            Debug.Log("RPC");
            PhotonView objectPV = PhotonView.Find(viewID);
            if (objectPV != null)
            {
                GameObject objectToThrow = objectPV.gameObject;
                objectToThrow.transform.SetParent(null); // 부모 관계 해제

                Rigidbody rb = objectToThrow.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.AddForce(objectPV.transform.forward * throwForce); // 오브젝트 던지기
                }

                // pickedUpObject를 null로 설정하는 것은 로컬 플레이어에만 적용되어야 합니다.
                // RPC 메소드 내에서는 이를 조작하지 않는 것이 좋습니다.
            }
        }


        // IEnumerator DelayedDestroyAndCreate(int viewID)
        // {
        //     // 오브젝트 파괴 지연
        //     yield return new WaitForSeconds(4f);

        //     PhotonView objectPV = PhotonView.Find(viewID);
        //     if (objectPV != null)
        //     {
               
        //         PhotonNetwork.Destroy(objectPV.gameObject);
        //         Debug.Log("큐브 생성");

        //         GameObject cubePrefab = Resources.Load<GameObject>("Cube");
        //         Vector3 position = new Vector3(6f, 5f, 13f);
        //         PhotonNetwork.Instantiate(cubePrefab.name, position, Quaternion.identity);
            
        //     }
        // }


    }

