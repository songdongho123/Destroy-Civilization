using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public float timer = 60f; // 카운트다운 타이머 초기값
    private bool isCounting = false; // 카운트다운이 진행 중인지 확인하는 플래그

    // Update is called once per frame
    void Update()
    {
        // Q키를 눌렀을 때 카운트다운 시작
        if (Input.GetKeyDown(KeyCode.Q) && !isCounting)
        {
            isCounting = true;
        }

        // 카운트다운 진행
        if (isCounting)
        {
            timer -= Time.deltaTime;
            Debug.Log(timer);
            if (timer <= 0)
            {
                Debug.Log("펑");
                timer = 60f; // 타이머 초기화 (필요한 경우)
                isCounting = false; // 카운트다운 중지
            }
        }
    }

    /*
    // 다른 Player 객체와의 충돌을 감지
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && isCounting)
        {
            Player otherPlayer = other.gameObject.GetComponent<Player>();
            if (!otherPlayer.isCounting) // 상대방이 카운트다운 중이 아닐 때만 옮김
            {
                otherPlayer.timer = this.timer; // 타이머 옮기기
                otherPlayer.isCounting = true; // 상대방 카운트다운 시작
                this.isCounting = false; // 현재 플레이어의 카운트다운 중지
                this.timer = 60f; // 현재 플레이어의 타이머 초기화
            }
        }
    }
    */

    // 이 로직이 캐릭터쪽에 들어가며 카운트를 관리해줘야함.
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("함수가 실행됩니다.");

        if (other.gameObject.name == "Cubea")
        {
            Debug.Log("충돌!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            // Cube와 충돌했을 때의 로직
            this.isCounting = false; // 현재 플레이어의 카운트다운 중지
            this.timer = 60f; // 현재 플레이어의 타이머 초기화
        }
    }

}