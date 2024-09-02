using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 시민-기사 : 마피아가 시민 동물을 죽일 때, 대신해서 죽을 수 있음
public class Knight : MonoBehaviourPunCallbacks
{
    private PlayerController playerController;
	private PlayerController otherPlayerController;

    public float detectDistance = 3f; // 탐지 거리
    public static int protectedPlayerViewID; // 탐지된 유저의 포톤 ID
    private Camera characterCamera; // 캐릭터에 붙은 카메라

	private bool canProtect = true;
	private GameObject knightUI;

    void Start()
    {
		playerController = gameObject.GetComponent<PlayerController>();

        // 캐릭터에 붙은 카메라 찾기
        characterCamera = GetComponentInChildren<Camera>();

        detectDistance = 3.5f;

        GameObject UI = GameObject.Find("Game UI");
        if (UI != null)
        {
            GameObject skillUI = UI.transform.Find("SkillUI").gameObject;
			knightUI = skillUI.transform.Find("Image").gameObject;
		}
    }
    
	void Update()
	{
		if (playerController.isAlive && canProtect)
		{
			ActivateDetect();
		}
	}

    // 기사로써 대상을 보호하는 기능 활성화하는 함수 (보호대상 탐지)
    public void ActivateDetect()
    {
        RaycastHit hit;

        // 카메라에서 마우스 위치로 레이를 쏘기
        Ray ray = characterCamera.ScreenPointToRay(Input.mousePosition);

        // 레이가 적에게 부딪힌 경우
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 탐지 범위 내에 있는지 확인
            if (Vector3.Distance(transform.position, hit.collider.transform.position) <= detectDistance)
            {
                PhotonView targetPhotonView = hit.collider.GetComponent<PhotonView>();
                Debug.Log("Knight.cs : 탐지된 자 - " + targetPhotonView.ViewID);
               
                // UI 활성화
                knightUI.SetActive(true);
                
                // 보호를 활성화
                ProtectTarget(targetPhotonView);
            }
        }
    }

    // 기사로써 대상을 보호하는 기능 활성화하는 함수 (보호대상 확인)
    private void ProtectTarget(PhotonView otherPlayerPhotonView)
    {
        if (otherPlayerPhotonView != null && Input.GetKeyDown(KeyCode.Q))
        {
            // UI 비활성화
            knightUI.SetActive(false);
			// 보호는 한 사람만
			canProtect = false;

			// RPC 호출을 통해 모든 클라이언트에게 상대 캐릭터의 상태를 변경하도록 요청
            photonView.RPC("SuccessfulProtect", RpcTarget.All, otherPlayerPhotonView.ViewID);
            protectedPlayerViewID = otherPlayerPhotonView.ViewID;
        
    		Debug.Log("Knight.cs : 보호된 자 - " + protectedPlayerViewID);
		}
    }

    // 기사로써 대상을 보호하는 기능 활성화하는 함수 (보호대상의 값 변경)
    [PunRPC]
    private void SuccessfulProtect(int otherPlayerViewID)
    {
        // 상대 캐릭터의 상태 변경을 모든 클라이언트에 반영
        GameObject otherPlayer = PhotonView.Find(otherPlayerViewID).gameObject;
        
        otherPlayerController = otherPlayer.GetComponent<PlayerController>();
        if (otherPlayerController != null)
        {
            // Debug.Log("Knight.cs : 보호 성공!");
            otherPlayerController.isProtectedByKnight = true;
        }
    }

}