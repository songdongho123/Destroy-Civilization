using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class TransScene : MonoBehaviourPunCallbacks
{
   // 일정 시간 후에 씬 전환을 위한 메소드

    void Start()
    {
        // 게임 시작 5초 후 MainScene으로 이동
        ChangeSceneWithDelay(6.0f);
    }
   public void ChangeSceneWithDelay(float delay)
   {
       StartCoroutine(ChangeAfterDelay(delay));
   }

   // 지연 후 씬 로드를 위한 코루틴
   IEnumerator ChangeAfterDelay(float delay)
   {
       // 지정된 시간만큼 대기
       yield return new WaitForSeconds(delay);

       // MainScene 로드
         //    SceneManager.LoadScene("MainScene");
        
        // 씬 전환
        SceneManager.LoadScene("GameMainScene");
   }
}