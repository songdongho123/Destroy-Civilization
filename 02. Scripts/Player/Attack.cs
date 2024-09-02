using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attack : MonoBehaviourPunCallbacks
{
    // private PlayerController playerController;
    private PlayerController otherPlayerController;
    public int attackedPlayerViewID = -1;   // (참고. RPC 안하는 변수)
    public float attackDistance = 3.5f;     // 공격 거리
    private Camera characterCamera;         // 캐릭터에 붙은 카메라
    private float lastAttackTime;           // 마지막 공격 시간
    public float attackCooldown = 30f;      // 공격 쿨다운 시간
    private GameObject attackUI;

    // JobManager
    public GameObject jobManagerObject;
    public JobManager jobManager;
    
    // Report
    private Report report;

    void Start()
    {
        // playerController = gameObject.GetComponent<PlayerController>();
        
        // 캐릭터에 붙은 카메라 찾기
        characterCamera = GetComponentInChildren<Camera>();

        attackDistance = 3.5f;
        
        GameObject UI = GameObject.Find("Game UI");
        if (UI != null)
        {
            GameObject skillUI = UI.transform.Find("KillUI").gameObject;
            attackUI = skillUI.transform.Find("Image").gameObject;
        }
        
        // JobManager 불러오기
        jobManagerObject = GameObject.Find("JobManager");
        jobManager = jobManagerObject.GetComponent<JobManager>(); 
        // report 스크립트 불러오기
        report = gameObject.GetComponent<Report>();
    }
    

    // 공격 활성화 하는 함수 (카메라 레이 -> 부딪힌 대상이 범위 내인지 확인 -> 상대방인지 확인)
    public void ActivateAttack()
    {
        RaycastHit hit;

        // 카메라에서 마우스 위치로 레이를 쏘기
        Ray ray = characterCamera.ScreenPointToRay(Input.mousePosition);

        // 레이가 적에게 부딪힌 경우
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 공격 범위 내에 있는지 확인
            if (Vector3.Distance(transform.position, hit.collider.transform.position) <= attackDistance)
            {
                // 상대방 플레이어인지 확인
                PhotonView targetPhotonView = hit.collider.GetComponent<PhotonView>();
               
                
                // 쿨타임이 아닐 때
                if (Time.time - lastAttackTime >= attackCooldown)
                {
					Debug.Log("Attack.cs : 킬 가능 -> " + targetPhotonView.ViewID);
                    // UI 활성화
                    attackUI.SetActive(true);
                    
                    // 공격을 활성화
                    AttackTarget(targetPhotonView);
                }
                else
                {
                    attackUI.SetActive(false);
					Debug.Log("Attack.cs : 킬 쿨타임 -> " + targetPhotonView.ViewID);
                }
            }
        }
    }
    
    // 타겟을 공격하는 함수 (좌클릭시 공격)
    private void AttackTarget(PhotonView attackedPlayerPhotonView)
    {
        if (attackedPlayerPhotonView != null && Input.GetMouseButtonDown(0))
        {
            // RPC 호출을 통해 모든 클라이언트에게 상대 캐릭터의 상태를 변경하도록 요청
            attackedPlayerViewID = attackedPlayerPhotonView.ViewID;
            photonView.RPC("SuccessfulAttack", RpcTarget.All, attackedPlayerViewID);

			Debug.Log("Attack.cs : 공격 대상 -> " + attackedPlayerViewID);
        }
    }

    // 공격당한 타겟의 상태값 변경하는 함수
    [PunRPC]
    private void SuccessfulAttack(int viewID)
    {
        GameObject otherPlayer = PhotonView.Find(viewID).gameObject;
        otherPlayerController = otherPlayer.GetComponent<PlayerController>();
        
        if (otherPlayerController != null)
        {
            string[] otherPlayerJob = jobManager.JobFromViewID(viewID);

            // 기사에게 지켜진 경우
            if (otherPlayerController.isProtectedByKnight)
            {
				Debug.Log("Attack.cs : 기사에게 지켜짐");
                photonView.RPC("ProtectedByKnight", RpcTarget.All);
            }
            // 논개를 공격한 경우
            else if (otherPlayerJob[1] == "Nongae")
            {
			    Debug.Log("Attack.cs : 논개임");
                otherPlayerController.isAlive = false;
                report.SendReportSign();
            }
            else
            {
				Debug.Log("Attack.cs : 공격 성공!");
                otherPlayerController.isAlive = false;
            }
        }
    }
    
    // 기사에게 방어되고 있는 동물을 공격할 경우, 실행되는 함수
    [PunRPC]
    private void ProtectedByKnight()
    {
        otherPlayerController.isProtectedByKnight = false;

        // 기사인 사람을 찾고, 그 사람의 isAlive 를 false 처리
        int knightViewID = jobManager.FindKnightViewID();
        Debug.Log("Attack.cs : 기사의 ViewID -> " + knightViewID);
        
        
        GameObject knight = PhotonView.Find(knightViewID).gameObject;
        PlayerController knightPlayerController = knight.GetComponent<PlayerController>();

        knightPlayerController.isAlive = false;
    }
}
