using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 시민-연예인 : 죽으면 시민 동물들만 죽은 알람(모달)이 울림
public class Star : MonoBehaviourPunCallbacks
{
    private PlayerController playerController;
    private bool isAlert = false;
    private GameObject starRIPUI;

	// JobManager
    public GameObject jobManagerObject;
	public JobManager jobManager;

    private void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        
        GameObject UI = GameObject.Find("Game UI");
        if (UI != null)
        {
            starRIPUI = UI.transform.Find("Star RIP UI").gameObject;
        }

		// JobManager 불러오기
        jobManagerObject = GameObject.Find("JobManager");
       	jobManager = jobManagerObject.GetComponent<JobManager>();
    }
    
    private void Update()
    {
        if (!playerController.isAlive && !isAlert)
        {
			photonView.RPC("StarDied", RpcTarget.All);			
		}
    }

    [PunRPC]
    private void StarDied()
	{
		PhotonView pv = gameObject.GetComponent<PhotonView>();
		int viewID = pv.ViewID;

        string[] myJob = jobManager.JobFromViewID(viewID);
		
		if (myJob[0] == "시민")
		{
			Debug.Log("Star.cs : 연예인이 죽음! 모달 알림 활성화");
        	// 모달 활성화
        	if (starRIPUI != null)
        	{
         		starRIPUI.SetActive(true);
        	}
        	// 3초 뒤 모달 비활성화
        	StartCoroutine(DisableModalAfterDelay(3f));

        	isAlert = true;

        	// 되살아났다면 초기화
        	if (playerController.isAlive && isAlert)
        	{
        	    isAlert = false;
        	}
		}
	}
    
    // 일정 시간이 지난 후에 모달을 비활성화하는 코루틴 함수
    private IEnumerator DisableModalAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // delay 시간이 지난 후에 모달을 비활성화합니다.
        if (starRIPUI != null)
        {
            starRIPUI.SetActive(false);
        }
    }
}