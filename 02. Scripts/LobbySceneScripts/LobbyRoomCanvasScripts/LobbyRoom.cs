using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class LobbyRoom : MonoBehaviourPunCallbacks
{
    // 화면 전환 캔버스 참조
    [Header("Canvas Object")]
    public GameObject makingModeCanvas;
    public Canvas searchingRoomCanvas;

    // 게임 방 참가하기 입력 칸 참조
    [Header("Room Enter Input")]
    public TMP_InputField inputText;
    public TMP_Text placeholder;

    // 유저 정보 출력 칸 참조
    [Header("User Infor Text")]
    public TMP_Text userInfo;


    // 처음 Lobby Room Canvas 들어 올 때, 유저 정보 출력
    private void Start() {
        gameObject.SetActive(true);
        userInfo.text = PhotonNetwork.NickName;
    }

    // 로비 나가기 (게임 메뉴 씬 이동)
    public void BackToMenuScene(){
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log($"���� ���� ����: {cause}"); // ���� ���� ���� �α�
        SceneManager.LoadScene("MenuScene");
    }

    // 게임 방 만들기 클릭
    public void ClickMakingRoom(){
        makingModeCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    // 게임 방 찾기 클릭
    public void ClickSearchingRoom(){
        searchingRoomCanvas.enabled = true;
        gameObject.SetActive(false);
    }

    // 게임 방 참가하기 클릭
    public void ClickEnterRoom(){
        if(string.IsNullOrEmpty(inputText.text))
        {
            Debug.Log("방 코드 예외 처리");
            placeholder.color = Color.red;
        }
        else
        {
            // 특정 게임 방 들어가는 함수.
            JoinRoom();
        }
    }

    // 특정 게임 방 들어가는 함수
    // (PhotonNetwork.JoinRoom() 실행 후 -> LobbySceneMananger.cs_ public override OnJoinedRoom() 실행)
    public void JoinRoom() {
        // 입력한 값에 해당하는 방에 들어가기 (방제)
        print("LobbyRoom.cs : " + inputText.text);
        LobbySceneManager.Instance.JoinRoom(inputText.text);
    }
}
