using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class ThrowBucket : MonoBehaviourPunCallbacks
    {
        public float pickUpRange = 4.0f; // 플레이어와 객체 사이의 최대 거리
        public float throwForce = 600f; // 물체를 던질 힘
        private GameObject objectToPickUp; // 플레이어가 집어 올릴 객체
        private GameObject pickedUpObject; // 현재 들고 있는 객체
        private bool isHolding = false; // 객체를 들고 있는지 여부

        private Renderer objectRenderer; // 물체의 랜더러


        private Material originalMaterial;
        private Renderer targetRenderer;

        public Material highlightMaterial; 
        public float tiltForce = 1500f;

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
                    if (hit.transform.gameObject.CompareTag("Bucket"))
                    {
                        // 클릭한 오브젝트에 PhotonView 컴포넌트가 있는지 확인
                        PhotonView pv = hit.transform.GetComponent<PhotonView>();
                        if (pv != null)
                        {
                            
                            Debug.Log(pv);
                            // SmearObject_RPC 메소드를 모든 클라이언트에서 호출
                            // photonView.RPC("TiltAndReleaseFish", RpcTarget.Others, pv.ViewID);
                            TiltAndReleaseFish(pv.ViewID);
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
                if (hit.transform.gameObject.CompareTag("Bucket"))
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
        public void TiltAndReleaseFish_RPC(int viewID)
        {
            // Debug.Log("Tlqkf");
            PhotonView objectPV = PhotonView.Find(viewID);
            if (objectPV != null)
            {
                
                Rigidbody rb = objectPV.GetComponent<Rigidbody>();

                rb.AddTorque(Vector3.forward * tiltForce);

                
            }
        }

        public void TiltAndReleaseFish(int viewID)
        {
            PhotonView objectPV = PhotonView.Find(viewID);
            if (objectPV != null)
            {
                
                
                Debug.Log("Tlqkf");
                // Quest 완료 로직
                PhotonView tv = this.GetComponent<PhotonView>();

                Rigidbody rb = objectPV.GetComponent<Rigidbody>();

                rb.AddTorque(Vector3.forward * tiltForce);
                questManager.CompleteQuest(tv.ViewID, nameof(ThrowBucket));
                Destroy(this);
                Debug.Log("퀘스트 완료");
                photonView.RPC("TiltAndReleaseFish_RPC", RpcTarget.Others, viewID);
                // StartCoroutine(DelayedDestroyAndCreate(viewID));

               


                
            }
        }


        // IEnumerator DelayedDestroyAndCreate(int viewID)
        // {
        //     // 오브젝트 파괴 지연

        //     Debug.Log("TiltAndReleaseFish called");
        //     Debug.Log("진입"); 
        //     PhotonView objectPV = PhotonView.Find(viewID);
        //     Debug.Log(viewID);
        //     Debug.Log(objectPV.ViewID);

        //     yield return new WaitForSeconds(2.0f);
        //     Debug.Log("퀘DDD");
            
        //     if (objectPV != null)
        //     {
                
        //         Destroy(objectPV.gameObject);
        //         Debug.Log("퀘스트 완료");

        //         GameObject cubePrefab = Resources.Load<GameObject>("Cube (1)");
        //         Vector3 position = new Vector3(6f, 5f, 19f);
        //         PhotonNetwork.Instantiate(cubePrefab.name, position, Quaternion.identity);
            
        //     }
        //     else
        //     {
        //         Debug.Log("ttqq");
        //     }

        //     yield return new WaitForSeconds(3f);
        // }



    



    }

