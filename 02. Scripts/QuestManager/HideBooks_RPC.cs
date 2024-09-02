using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class HideBooks_RPC : MonoBehaviourPunCallbacks
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
                    if (hit.transform.gameObject.CompareTag("Books"))
                    {
                        StartCoroutine(PickUpObject(hit.transform.gameObject));
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0) && isHolding && pickedUpObject != null)
            {
                StartCoroutine(FadeOutObject(pickedUpObject));
            }
        
        
        if(GetComponent<PhotonView>().IsMine) {
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.transform.gameObject.CompareTag("Books"))
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
            Debug.Log(pickedUpObject.GetComponent<PhotonView>().ViewID);
            photonView.RPC("PickUpObjectRPC", RpcTarget.Others, pickedUpObject.GetComponent<PhotonView>().ViewID,this.GetComponent<PhotonView>().ViewID);

            yield return new WaitForSeconds(.5f);
        }


        public IEnumerator FadeOutObject(GameObject holding)
        {
            Renderer renderer = holding.GetComponent<Renderer>();
            Color startColor = renderer.material.color;
            PhotonView pv = holding.GetComponent<PhotonView>();
            PhotonView tv = this.GetComponent<PhotonView>();

            float duration = 0.5f; // 사라지는 시간

            for (float t = 0.0f; t < duration; t += Time.deltaTime)
            {
                Color color = startColor;
                color.a = Mathf.Lerp(1f, 0f, t / duration); // 투명도 조절
                renderer.material.color = color;
                yield return null;
            }

            // 여기서 ViewID를 추출
            int viewID = holding.GetComponent<PhotonView>().ViewID;

            // RPC 호출을 올바른 ViewID와 함께 수행
            photonView.RPC("FadeOutObjectRPC", RpcTarget.Others, viewID);

            // 그 후 객체를 비활성화하고 참조를 제거
            holding.SetActive(false);
            holding.transform.SetParent(null);
            questManager.CompleteQuest(tv.ViewID, nameof(HideBooks_RPC));
            // photonView.RPC("DelayedDestroyAndCreate", RpcTarget.All, viewID);
            Destroy(this);
            // holding = null; // 이제 여기서 holding을 null로 설정하는 것은 필요 없습니다.
        }




        [PunRPC]
        public void FadeOutObjectRPC(int viewID)
        {
            
            Debug.Log(viewID);
            PhotonView pv = PhotonView.Find(viewID);
            if (pv != null && pv.gameObject != null)
            {
                // 객체를 바로 비활성화합니다. 페이드 아웃 효과는 로컬에서 처리해야 합니다.
                pv.gameObject.SetActive(false);
            }
        }


        // [PunRPC]
        // IEnumerator DelayedDestroyAndCreate(int viewID)
        // {
        //     // 오브젝트 파괴 지연
        //     yield return new WaitForSeconds(4f);

        //     PhotonView objectPV = PhotonView.Find(viewID);
        //     if (objectPV != null)
        //     {
        //         // 오브젝트를 파괴하는 로직
        //         if (objectPV.IsMine) // 오브젝트가 로컬 클라이언트에 속한 경우에만 파괴
        //         {
        //             PhotonNetwork.Destroy(objectPV.gameObject);
        //         }

        //         // Quest 완료 로직
        //         PhotonView tv = this.GetComponent<PhotonView>();
        //         questManager.CompleteQuest(tv.ViewID, nameof(ThrowBucket));
        //         Debug.Log("퀘스트 완료");

        //         // 새 오브젝트 생성 로직, 오브젝트 파괴 후 바로 실행됨
        //         if (PhotonNetwork.IsMasterClient) // 마스터 클라이언트에서만 새 오브젝트 생성
        //         {
        //             GameObject cubePrefab = Resources.Load<GameObject>("Cube");
        //             Vector3 position = new Vector3(4f, 5f, 6f);
        //             PhotonNetwork.Instantiate(cubePrefab.name, position, Quaternion.identity);
        //         }
        //     }
        // }


    }

