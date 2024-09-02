using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시민-자경단 : 아무나 한 명 공격할 수 있음 
public class Vigilante : MonoBehaviour
{
    private PlayerController playerController;
    
    // Attack 관련 변수
    private Attack attack;

    private void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();

        // Attack 스크립트를 추가
        attack = GetComponent<Attack>();
        if (attack == null)
        {
            attack = gameObject.AddComponent<Attack>();
        }
    }
    
    private void Update()
    {
        // 공격을 한 번도 해본적이 없고, 본인이 아직 살아있다면
        if (attack.attackedPlayerViewID == -1 && playerController.isAlive)
        {
            // attack 스크립트를 실행
            attack.ActivateAttack();
        }
    }
}