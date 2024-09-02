using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameLobbySceneManager : MonoBehaviourPunCallbacks
{
    public GameObject GameStartSceneAniaml;
    public GameObject GameStartSceneSpy;
    public GameObject InGameLobbyScene;
    public Canvas GameMainUI;
    public Canvas InGameLobbyUI;
    public static int rootCount;
    public GameObject GameEnd;

    public PhotonTransformView photontransformView;

    ChangeUiGraphics changeUiGraphics;
    ChangeUiJobName changeUiJobName;


    QuestManager questManager;

    // 플레이어 인 게임 로비 입장 무작위 배치
    void Start()
    {
        GameMainUI.enabled = false;
        InGameLobbyUI.enabled = true;
        questManager = FindObjectOfType<QuestManager>();
        changeUiGraphics = GetComponent<ChangeUiGraphics>();
        changeUiJobName = GetComponent<ChangeUiJobName>();

        // 랜덤 스폰 위치 설정
        Transform[] spown = GameObject.Find("SpownerLobby").GetComponentsInChildren<Transform>();
        int idx = UnityEngine.Random.Range(1, spown.Length);

        GameObject player = PhotonNetwork.Instantiate("Player", spown[idx].position, spown[idx].rotation, 0);
    }

    // 게임 시작 버튼 누를 때, 함수
    public void GameStartCanvasON(){

        JobManager.Instance.AssignJobToPlayers();
        // if (PhotonNetwork.IsMasterClient) // 마스터 클라이언트인지 확인

        PlayerManager.Instance.PlayerStatus();
        // {
        questManager.AssignQuestsToPlayers();
        // }

        // 게임 시작 5초 후 MainScene으로 이동
        photonView.RPC("ChangeSceneWithDelay", RpcTarget.All, 6.0f);

        GameEnd.SetActive(true);
        rootCount ++;
        // changeUiGraphics.GetUI();
        // changeUiJobName.GetName();
        

    }


    [PunRPC]
    // 일정 시간 후에 씬 전환을 위한 메소드
    public void ChangeSceneWithDelay(float delay)
    {   

        // 플레이어 오브젝트를 찾습니다.
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 각 플레이어 오브젝트를 반복하면서 자신을 식별할 수 있는지 확인합니다.
        foreach (GameObject player in players)
        {
            PhotonView pv = player.GetComponent<PhotonView>();

            if(pv != null && pv.IsMine)
            {
                int myViewID = pv.ViewID;
                print("myViewID : " + myViewID);
                print("(JobManager.Instance.JobFromViewID(myViewID) : " + JobManager.Instance.JobFromViewID(myViewID)[0] + "///  JobManager.Instance.JobFromViewID(myViewID)[0] : " + JobManager.Instance.JobFromViewID(myViewID)[1]);
                if(JobManager.Instance.JobFromViewID(myViewID)[0] == "시민")
                {
                    GameStartSceneAniaml.SetActive(true);
                    Text text = GameStartSceneAniaml.transform.GetChild(1).GetComponent<Text>();
                    text.text = "당신은 " + JobManager.Instance.JobFromViewID(myViewID)[1] + " 입니다.";
                    changeUiGraphics.GetUI(JobManager.Instance.JobFromViewID(myViewID)[1]);
                    changeUiJobName.GetName(JobManager.Instance.JobFromViewID(myViewID)[1]);
                } 
                else if (JobManager.Instance.JobFromViewID(myViewID)[0] == "스파이")
                {
                    GameStartSceneSpy.SetActive(true);
                    Text text = GameStartSceneSpy.transform.GetChild(1).GetComponent<Text>();
                    text.text = "당신은 " + JobManager.Instance.JobFromViewID(myViewID)[1] + " 입니다.";
                    changeUiGraphics.GetUI(JobManager.Instance.JobFromViewID(myViewID)[1]);
                    changeUiJobName.GetName(JobManager.Instance.JobFromViewID(myViewID)[1]);
                }
                else if (JobManager.Instance.JobFromViewID(myViewID)[0] == "중립")
                {
                    return;
                }
                break;
            }
        }

        // A 플레이어 실행
        // GameStartSceneSpy.SetActive(true);
        // B 플레이어 실행
        // GameStartSceneAniaml.SetActive(true);

        // InGameLobbyScene.SetActive(false);
        StartCoroutine(ChangeAfterDelay(delay));
    }

   // 지연 후 씬 로드를 위한 코루틴
   IEnumerator ChangeAfterDelay(float delay)
   {
       ///////// 대기 시간 동안에, 랜덤 위치 배분 / 직업 배분 / 미션 배분
       if(PhotonNetwork.IsMasterClient)
       {
            
            GameMainRandomSpawn();
       }

       // 지정된 시간만큼 대기
       yield return new WaitForSeconds(delay);

       // MainScene 로드 (GameStartScene OFF)
       print("// MainScene 로드 (GameStartScene OFF)");
        GameStartSceneAniaml.SetActive(false);
        GameStartSceneSpy.SetActive(false);
        InGameLobbyUI.enabled = false;
        GameMainUI.enabled = true;
   }



    
    // 플레이어 메인 게임 입장 무작위 배치
    public void GameMainRandomSpawn()
    {

        // 현재 씬에 있는 모든 "Player" 태그로된 게임 오브젝트를 가져온다.
        // 모든 플레이어 오브젝트를 가져오기.
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 모든 "Players" 오브젝트를 반복해서 돌린다.
        foreach (var player in players)
        {
            // "Player" 오브젝트의 PhotonView 컴포넌트 가져오기
            PhotonView pv = player.GetComponent<PhotonView>();
            MoveController moveController = player.GetComponent<MoveController>();
            photontransformView = player.GetComponent<PhotonTransformView>();
            CharacterController characterController = player.GetComponent<CharacterController>();


            if(pv != null)
            {  
                Debug.Log($"Handling player with ViewID: {pv.ViewID}, Before disabling components. Position: {pv.transform.position}");
                characterController.enabled = false;
                photontransformView.enabled = false;

                // 출력 예상 값 : 1001 , 2001, 3001, 4001 ...
                print(pv.ViewID);
        
                print("RPC 호출 전: " + pv.transform.position); 
                // "Player" 오브젝트의 ViewID 를 매개변수로 가지고,
                // 모든 클라이언트 측에 있는 AssignJobToPlayersRPC() 함수를 호출한다.
                photonView.RPC("RandomSpawnPlayersRPC", RpcTarget.All, pv.ViewID);
                // print("RPC 호출 후: " + pv.transform.position);

                characterController.enabled = true;
                photontransformView.enabled = true;
                Debug.Log($"Handled player with ViewID: {pv.ViewID}, After re-enabling components. Position: {pv.transform.position}");

                
            }

            // 미니맵
            SetTargetForMiniMap(player.transform);
        }

    }

    


    [PunRPC]
    public void RandomSpawnPlayersRPC(int viewID)
    {   
        print("// 랜덤 스폰 위치 설정");
        // 랜덤 스폰 위치 설정
        Transform[] spown = GameObject.Find("Spowner").GetComponentsInChildren<Transform>();
        
        int idx = UnityEngine.Random.Range(1, spown.Length);

        // 현재 클라이언트 씬에 있는 모든 PhotonView 오브젝트 중에
        // 매개 변수 ViewID 에 해당하는 PhotonView 오브젝트를 가지고 온다. 
        PhotonView pv = PhotonView.Find(viewID);
        

        if (pv != null)
        {
            Debug.Log($"Before moving player with ViewID: {viewID}, Position: {pv.transform.position}");
            pv.transform.position = spown[idx].position;
            Debug.Log($"After moving player with ViewID: {viewID}, New Position: {pv.transform.position}");
        }
        else
        {
            Debug.LogError($"PhotonView with ViewID: {viewID} not found.");
        }
      
    }

    




    void SetTargetForMiniMap(Transform playerTransform)
    {
        // CanvasMiniMap 오브젝트 찾기
        GameObject miniMapCanvas = GameObject.Find("Game UI/CanvasMiniMap");
        if (miniMapCanvas != null)
        {
            // 미니맵 컴포넌트 찾기 (이 예제에서는 MiniMapController라고 가정)
            MiniMapController miniMapController = miniMapCanvas.GetComponent<MiniMapController>();
            if (miniMapController != null)
            {
                // 미니맵의 타겟 설정
                miniMapController.SetTarget(playerTransform);
            }
            else
            {
                Debug.LogError("MiniMapController 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("UI/CanvasMiniMap 오브젝트를 찾을 수 없습니다.");
        }
    }

    public void ExitInGameLobbyRoom(){
        print("Ext In Game Lobby Csnene");
        
        // 게임 방 떠나기
        PhotonNetwork.LeaveRoom();
        // 씬 전환
        SceneManager.LoadScene("MenuScene");
    }
}
