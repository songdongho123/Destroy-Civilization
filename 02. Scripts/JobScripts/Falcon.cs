using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 중립-송골매 : 자기 자신만 살아남기, 죽이기 가능
public class Falcon : MonoBehaviourPunCallbacks
{
	private PlayerController playerController;
    
	// Attack 관련 변수
	private Attack attack;

    void Start()
    {
	    playerController = gameObject.GetComponent<PlayerController>();

	    // Attack 스크립트를 추가
	    attack = GetComponent<Attack>();
	    if (attack == null)
	    {
		    attack = gameObject.AddComponent<Attack>();
	    }
    }

    void Update()
    {
	    if (playerController.isAlive)
	    {
		    // attack 스크립트를 실행
		    attack.ActivateAttack();
	    }
    }
}