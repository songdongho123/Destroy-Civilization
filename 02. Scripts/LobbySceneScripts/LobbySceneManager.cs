using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    public static LobbySceneManager Instance;

    // API POST 요청 방 삭제
    [System.Serializable]
    public class PostDataRoomDelete{
        public int roomCode;
    }

    private void Awake()    
    {
        if(Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 게임 방 생성
    // 방제 , 인원 수, 비공개/공개 , 입장허용 , 나머지 옵션 default
    public void CreatedRoom(string roomName, int maxPlayers, bool isVisible){
        PhotonNetwork.CreateRoom(roomName,
        new RoomOptions(){MaxPlayers = maxPlayers, IsVisible = isVisible, IsOpen = true},
        TypedLobby.Default, null);
    }

    // 게임 방 입장
    public void JoinRoom(string roomName){
        PhotonNetwork.JoinRoom(roomName);
    }


    // 개별 게임 방 클릭 시, 해당 게임 방 입장
    public void JoinRoomInList(string RoomTitle)
    {
        print("LobbySceneManager.cs /joinRoomInlist");
        PhotonNetwork.JoinRoom(RoomTitle);
    }


    // 게임 방 입장 (InGameLobbyScene 입장)
    public override void OnJoinedRoom()
    {
        print("LobbySceneMananger.cs OnJoinedRoom");
        // base.OnJoinedRoom();
        // PhotonNetwork.LoadLevel("InGameLobbyScene");
        PhotonNetwork.LoadLevel("GameMainScene");

    }

    // 게임 방 입장 실패
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("LobbySceneMananger.cs OnJoinRoomFailed : "  + returnCode + message);
    }



    // 방 리스트에 추가할 룸 프리팹
    public GameObject RoomPrefb;

    // 방 제목 객체
    public TMP_Text roomTitle;

    // 전체 방 목록 데이터 객체
    public Dictionary<string, RoomInfo> rooms = new Dictionary<string, RoomInfo>();


    // 방 업데이트 확인 용
    private void Update() {
        foreach(KeyValuePair<string, RoomInfo> room in rooms){
            print("room.Key : " + room.Key + " /// room.value : " + room.Value);
        }    
    }


    // 게임 방 리스트를 업데이트 시켜주는 Event 함수
    // 게임 방 생성 or 삭제 될 때, 해당 목록 만 업데이트 시켜준다.
    // (생성 => MakingRoom.cs _ CreatedRoom() / 삭제 => 포톤 자동 처리)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print(roomList + " : LobbySceneMananger.cs OnRoomListUpdate");
        // base.OnRoomListUpdate(roomList);
        print(roomList.Count + " : LobbySceneMananger.cs OnRoomListUpdate");

        // 변경된 방들
        foreach(var roomInfo in roomList){
            // if(roomInfo.RemovedFromList == true){

            //     // Room Dlete 방 삭제 POST 요청 실행
            //     PostDataRoomDelete postDataRoomDelete = new PostDataRoomDelete();
            //     postDataRoomDelete.roomCode = ServerData.Instance.lastRoom;
            //     StartCoroutine(DataManager.instance.SendApiRequest("https://j10b310.p.ssafy.io/room/delete", "DELETE", postDataRoomDelete, onComplete, onError));
            // }

            // 삭제된 거임?
            if(roomInfo.RemovedFromList == true){
                if(rooms.ContainsKey(roomInfo.Name)){
                    // 방 삭제
                    rooms.Remove(roomInfo.Name);
                }
            }
            // 추가 된거?
            else {
                // 방이 이미 있어?
                if(rooms.ContainsKey(roomInfo.Name)){
                    print("이미 있는 방이란다.");
                }
                // 방 없으면 추가
                else{
                    rooms.Add(roomInfo.Name, roomInfo);
                }
            }
        }

         // 기존 방 목록 다 지우고.
         foreach(Transform child in GameObject.Find("Content").transform){
             Destroy(child.gameObject);
         }

        // 새롭게 방 목록 갱신
        foreach(KeyValuePair<string, RoomInfo> room in rooms){
            GameObject Room = Instantiate(RoomPrefb, Vector3.zero, Quaternion.identity, GameObject.Find("Content").transform);
            Room.GetComponent<Room>().roomTitle.text = room.Key;
        }    
    }

    // API POST 요청 방 삭제 성공
    void onComplete(string response){
        print("LobbySceneManager.cs OnRoomListUpdate() : " + response);
    }
    // API POST 요청 방 삭제 실패
    void onError(string error){
        Debug.LogError("LobbySceneManager.cs OnRoomListUpdate() : " + error);
    }
}
