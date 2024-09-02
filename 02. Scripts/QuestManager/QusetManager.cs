using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun.Demo.Cockpit;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviourPunCallbacks // Photon의 MonoBehaviourPunCallbacks를 상속받습니다.
{
    public List<string> questClassNames; // 스크립트의 클래스 이름을 문자열로 저장합니다.
    // 나머지 멤버 변수는 그대로 유지...
    public  int totalProgress = 0;
    public static int missionScore =1; 
    public int progressToWin = 1;

    // 퀘스트 관련 유저별 담는 정보들
    public List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

    public event Action OnListChanged; // 미션 동기화를 위한 이벤트

    private void Update()
    {

    // if (Input.GetKeyDown(KeyCode.G)) // Q 버튼을 누르고 대상이 선택되어 있을 때
                    
    //     {
    //         if (PhotonNetwork.IsMasterClient) // 마스터 클라이언트인지 확인
    //         {
    //             AssignQuestsToPlayers();
    //         }
    //     }

        // if (totalProgress >= progressToWin)
        // {
        //     Debug.Log("씬 전환333");
        //     SceneManager.LoadScene("GameEndScene_Animal");
        // }

    }

    [PunRPC]
    public void AssignQuestToPlayerRPC(int viewID)
    {
        
        for (int i = 0; i < 3; i++)
        {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null && questClassNames.Count > 0)
        {
            string questName = questClassNames[0];
            questClassNames.RemoveAt(0); 

            Type questType = Type.GetType(questName);
            if (questType != null)
            {
                pv.gameObject.AddComponent(questType);

                list.Add(new KeyValuePair<int, string>(viewID, questName));
                Debug.Log(list[0]);
                Debug.Log(list);

                OnListChanged?.Invoke(); // 변경 이벤트 호출로 미션 동기화
                }
            else
            {
                Debug.LogError("Quest type not found: " + questName);
            }
        }
        }
    }

    [PunRPC]
    public void AssignQuestToPlayerRPC2(int viewID)
    {
        
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null && questClassNames.Count > 0)
        {
            string questName = questClassNames[0];
            questClassNames.RemoveAt(0); 

            Type questType = Type.GetType(questName);
            if (questType != null)
            {
                pv.gameObject.AddComponent(questType);

                list.Add(new KeyValuePair<int, string>(viewID, questName));
                Debug.Log("리스트 추가");

                OnListChanged?.Invoke(); // 변경 이벤트 호출로 미션 동기화
            }
            else
            {
                Debug.LogError("Quest type not found: " + questName);
            }
        
        }
    }

    public void AssignQuestsToPlayers()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv != null)
            {
                Debug.Log(pv.ViewID);
                photonView.RPC("AssignQuestToPlayerRPC", RpcTarget.All, pv.ViewID);
            }
        }
    }


    [PunRPC]
    public void CompleteQuestRPC(int viewID, string completedQuestName)
    {
        Debug.Log("컴플리트");

        
        if (PhotonNetwork.IsMasterClient)
        {
            totalProgress += missionScore;

            
            Debug.Log(totalProgress);

            photonView.RPC("AssignQuestToPlayerRPC2", RpcTarget.All, viewID);
            photonView.RPC("UpdateTotalProgressRPC", RpcTarget.Others, totalProgress, questClassNames.ToArray());
        
            if (totalProgress >= progressToWin)
            {
                Debug.Log("씬 전환");
                SceneManager.LoadScene("GameEndScene_Animal");
            }
            else
            {
                
            
                
                // 일치하는 요소를 리스트에서 찾아 제거
                var itemToRemove = list.FirstOrDefault(kvp => kvp.Key == viewID && kvp.Value == completedQuestName);
                if (itemToRemove.Key != 0 || itemToRemove.Value != null) // Default 값이 아니면, 즉 찾았으면
                {
                    
                    list.Remove(itemToRemove);

                    OnListChanged?.Invoke(); // 변경 이벤트 호출로 미션 동기화
                }
                    
                
                // 완료된 미션을 리스트에 다시 추가
                questClassNames.Add(completedQuestName);
                // 다음 미션을 할당
                // AssignQuestsToPlayers();
            }
        }

        

    
            // totalProgress의 새로운 값을 모든 클라이언트에게 전송
            
        
    }

    [PunRPC]
    public void UpdateTotalProgressRPC(int newTotalProgress, string[] newquestClassNames )
    {
        totalProgress = newTotalProgress;
        questClassNames = new List<string>(newquestClassNames);

        if (totalProgress >= progressToWin)
        {
            Debug.Log("씬 전환22");
            PhotonNetwork.LoadLevel("GameEndScene_Animal");
        }
        Debug.Log("Update 문");

    }


    public void CompleteQuest(int viewID, string completedQuestName)
    {
        // 모든 클라이언트에게 RPC를 호출하여 퀘스트 완료를 알립니다.
        photonView.RPC("CompleteQuestRPC", RpcTarget.All, viewID, completedQuestName);
    }

    // 다른 메서드들은 기존과 동일...
}
