using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class SmearPaper_RPC : MonoBehaviourPunCallbacks
    {
        public float pickUpRange = 2.0f; // 플레이어와 객체 사이의 최대 거리
        public float throwForce = 600f; // 물체를 던질 힘
        private GameObject objectToPickUp; // 플레이어가 집어 올릴 객체
        private GameObject pickedUpObject; // 현재 들고 있는 객체
        // private bool isHolding = false; // 객체를 들고 있는지 여부

        private Renderer objectRenderer; // 물체의 랜더러


        private Material originalMaterial;
        private Renderer targetRenderer;

        public Material highlightMaterial; 

        public float clickCount; // 클릭한 횟수

        QuestManager questManager;

        void Start()
        {
            // QuestManager 인스턴스를 찾아서 할당합니다.
            questManager = FindObjectOfType<QuestManager>();
            highlightMaterial = Resources.Load<Material>("HighlightMaterial"); 
            clickCount = 0f; // 클릭한 횟수
        }


        void Update()
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, pickUpRange))
                {
                    if (hit.transform.gameObject.CompareTag("Paper"))
                    {
                        // 클릭한 오브젝트에 PhotonView 컴포넌트가 있는지 확인
                        PhotonView pv = hit.transform.GetComponent<PhotonView>();
                        if (pv != null)
                        {
                            
                            Debug.Log(pv);
                            // SmearObject_RPC 메소드를 모든 클라이언트에서 호출
                            
                            SmearObject(pv.ViewID);
                        }
                    }
                }
            }
        
        
        if(GetComponent<PhotonView>().IsMine) {
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.transform.gameObject.CompareTag("Paper"))
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


        [PunRPC]
        public void SmearObject_RPC(int viewID)
        {
            PhotonView objectPV = PhotonView.Find(viewID);
            PhotonView tv = this.GetComponent<PhotonView>();
            Debug.Log(objectPV.ViewID);
            Debug.Log(viewID);
            if (objectPV != null && objectPV.gameObject != null)
            {
                Renderer renderer = objectPV.gameObject.GetComponent<Renderer>();
                // 클릭 카운트에 따라 색상 변경 로직...
                // 예: renderer.material.color = Color.Lerp(startColor, Color.black, t);
                // 여기서는 단순화를 위해 직접 Color.black으로 설정
                renderer.material.color = Color.black;
              

            }
        }

        public void SmearObject(int viewID)
        {
            PhotonView objectPV = PhotonView.Find(viewID);
            PhotonView tv = this.GetComponent<PhotonView>();
          
            Debug.Log(objectPV.ViewID);
            Debug.Log(viewID);
            if (objectPV != null && objectPV.gameObject != null)
            {
                Renderer renderer = objectPV.gameObject.GetComponent<Renderer>();
                
                // 클릭 카운트에 따라 색상 변경 로직...
                // 예: renderer.material.color = Color.Lerp(startColor, Color.black, t);
                // 여기서는 단순화를 위해 직접 Color.black으로 설정
                renderer.material.color = Color.black;
                questManager.CompleteQuest(tv.ViewID, nameof(SmearPaper_RPC));
                photonView.RPC("SmearObject_RPC", RpcTarget.Others, viewID);

                Debug.Log("퀘스트 완료");
                Destroy(this);

            }
        }

    }

