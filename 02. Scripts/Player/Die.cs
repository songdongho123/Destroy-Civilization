using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Die : MonoBehaviourPunCallbacks
{
    private PlayerController playerController;
    private CharacterController characterController;
    private bool createDeadBody = false;
    
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
		// 플레이어가 공격받았을 경우
        if (!createDeadBody && !playerController.isAlive && photonView.IsMine)
        {
	        Debug.Log("Die.cs : 사망");
			photonView.RPC("ActiveDeath", RpcTarget.All);
			createDeadBody = true;
        }
    }

    // 
    [PunRPC]
    private void ActiveDeath()
    {
	    // ** 캐릭터의 충돌 감지 비활성화 -> 안됨 -> 유령상태 포기 -> 살아있는 캐릭터의 카메라를 관전하는 방식으로 변경해야 함 **
	    // characterController.detectCollisions = false;

	    Transform speaker = GameObject.FindGameObjectWithTag("Player").transform.Find("Speaker");
	    
	    // 시체 오브젝트를 자식 오브젝트로 찾아서 처리
	    foreach (Transform child in transform)
	    {
		    Debug.Log(child.name);
		    // 자식 오브젝트가 시체인 경우 활성화
		    if (child.name == "Dead Player")
		    {
			    // Dead Player 오브젝트에 Rigidbody 컴포넌트 추가
			    Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
			    // Rigidbody 설정 (질량과 중력 설정)
			    rb.mass = 1f;
			    rb.useGravity = true;

			    // Dead Player 오브젝트에 Box Collider 컴포넌트 추가
			    BoxCollider collider = child.gameObject.AddComponent<BoxCollider>();
			    // Collider 설정 (예시로 크기 설정)
			    collider.size = new Vector3(1f, 1f, 1f);

			    child.gameObject.SetActive(true);
			    child.parent = null;
		    }
		    
		    // 시체가 아닌 경우 비활성화
		    else if (child.name != "Camera")
		    {
			    child.gameObject.SetActive(false);
		    }
	    }

	    speaker.gameObject.SetActive(false);
    }
}