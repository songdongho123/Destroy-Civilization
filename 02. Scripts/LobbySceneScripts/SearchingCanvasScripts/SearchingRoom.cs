using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SearchingRoom : MonoBehaviourPunCallbacks
{
    // 방 제목 객체
    public TMP_Text roomTitle;

    [Header("Canvas Object")]
    public GameObject lobbyRoomCanvas;

    public Canvas searchingRoomCanvas;

    public GameObject RoomPrefb;

    private void Start() {
        searchingRoomCanvas.enabled = false;
    }
    
    // 뒤로 가기 (Lobby Room Canvas 이동)
    public void BackToLobbyRoom(){
        lobbyRoomCanvas.SetActive(true);
        searchingRoomCanvas.enabled = false;
    }

    private void Awake() {
        print("SearchingRoomm.cs Awake() : 기존 방 다 불러오기");

        if(LobbySceneManager.Instance.rooms != null){
            foreach(KeyValuePair<string, RoomInfo> room in LobbySceneManager.Instance.rooms){
                GameObject Room = Instantiate(RoomPrefb, Vector3.zero, Quaternion.identity, GameObject.Find("Content").transform);
                Room.GetComponent<Room>().roomTitle.text = room.Key;
            }
        }
    }
}
