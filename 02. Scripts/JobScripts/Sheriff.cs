using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 시민-보안관 : 아무나 썰 수 있음, 시민을 썰었을 때는 동시 사망, 마피아를 썰었을 때는 무한 썰기
public class Sheriff : MonoBehaviourPunCallbacks
{    
    private PlayerController playerController;
    
    // Attack 관련 변수
    private Attack attack;

	// JobManager
    public GameObject jobManagerObject;
	public JobManager jobManager;

    private void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();

        // Attack 스크립트를 추가
        attack = GetComponent<Attack>();
        if (attack == null)
        {
            attack = gameObject.AddComponent<Attack>();
        }

		// JobManager 불러오기
        jobManagerObject = GameObject.Find("JobManager");
       	jobManager = jobManagerObject.GetComponent<JobManager>();
    }
    
    private void Update()
    {
        int preAttackedPlayerViewID = -1;
        
        if (playerController.isAlive)
        {
            // attack 스크립트를 실행
            attack.ActivateAttack();
            
            // 공격당한 플레이어(참고. RPC 되면 안됨)가 있다면, 공격당한 플레이어의 직업 확인
            if (attack.attackedPlayerViewID != -1)
            {
                Debug.Log("Sheriff.cs : 보안관이 죽인 자 - " + attack.attackedPlayerViewID);
                photonView.RPC("WhoIsAttacked", RpcTarget.All, attack.attackedPlayerViewID);
                
                // 공격당한 플레이어를 저장해두고, 다시 초기화
                preAttackedPlayerViewID = attack.attackedPlayerViewID;
                attack.attackedPlayerViewID = -1;
            }
        }
    }
    
    

    // 누가 죽었는지 확인한 후, 시민인지 마피아인지에 따라 보안관의 생사를 결정하는 함수
    [PunRPC]
    private void WhoIsAttacked(int viewID)
    {
        string[] attackedPlayerJob = jobManager.JobFromViewID(viewID);
		Debug.Log("Sheriff.cs : 보안관이 죽인 자의 직업 - " + string.Join("", attackedPlayerJob[0]));
        
		// attackedPlayerJob 이 시민이면, 동시 사망하고,
        // attackedPlayerJob 이 마피아나 중립이면 성공적인 킬
        if (attackedPlayerJob[0] == "시민" || attackedPlayerJob[0] == "중립")
        {
          	playerController.isAlive = false;
        }
    }
}