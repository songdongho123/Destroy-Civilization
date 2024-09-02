using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ServerData : MonoBehaviourPunCallbacks
{
    public static ServerData Instance;

    [Header("서버 선택 App Settings (APP ID 가 들어있음)")]
    public AppSettings settings;


    [Header("현재 유저 정보 데이터")]
    // (Back) 현재 유저 코드
    public int userCode;
    // (Back) 현재 유저 ID
    public string userId;
    // (Back) 현재 유저 이름
    public string userName;
    
    
    [Header("현재 입장한 서버 데이터")]
    // (Back) 현재 서버 코드
    public int serverCode;
    // (Back) 현재 서버에 있는 사람들.
    public int serverPeople;
    // (Back) 현재 서버 인원 제한
    public int serverLimit;
    // (Back) 현재 서버에 있는 방들 중에서 가장 마지막 방 코드
    public int lastRoom;
    // (Back) 현재 서버에 있는 방들.
    public Array rooms;


    public Sprite mapImage;
    public Sprite modeImage;

    private void Awake() {
        if(Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }





    































































//     using UnityEngine;
// using TMPro;
// using Photon.Pun;
// using Photon.Realtime;
// using System.Collections.Generic;

// public class PhotonManager : MonoBehaviourPunCallbacks
// {
//     // 게임 버전을 나타내는 문자열
//     private readonly string version = "1.0";
//     // 기본 사용자 ID
//     private string userId = "Nickname";
//     // 사용자가 입력하는 TextMeshPro Input Field
//     public TMP_InputField userIF;

//     // 캐릭터 선택 값
//     public static int CharacterValue = 1;
//     // 플레이어 게임 오브젝트들
//     public GameObject player1;
//     public GameObject player2;
//     public GameObject player3;
//     public GameObject player4;

//     // 방 이름을 입력하는 TextMeshPro Input Field
//     public TMP_InputField roomNameIF;

//     // 방 목록을 저장하는 딕셔너리
//     private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
//     // 방 목록 아이템의 프리팹
//     private GameObject roomItemPrefab;
//     // RoomList를 표시할 스크롤 영역
//     public Transform scrollContent;

//     void Awake()
//     {
//         // Photon 서버 설정 초기화

//         // 게임 버전 설정

//         // 사용자 ID 초기화

//         // RoomList 초기화

//     }

//     void Start()
//     {
//         // Photon 서버에 연결

//         // 사용자 ID를 게임 내에서 표시
//     }

//     // 마스터 서버에 연결됐을 때 호출되는 콜백 함수
//     public override void OnConnectedToMaster()
//     {
//         Debug.Log("Connected to Master!");
//         // 로비에 입장
//         PhotonNetwork.JoinLobby();
//     }

//     // 로비에 입장했을 때 호출되는 콜백 함수
//     public override void OnJoinedLobby()
//     {
//         Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
//         // 로비에 입장하면 랜덤하게 방에 입장을 시도
//         // PhotonNetwork.JoinRandomRoom();        
//     }

//     // 랜덤한 방에 입장에 실패했을 때 호출되는 콜백 함수
//     public override void OnJoinRandomFailed(short returnCode, string message)
//     {
//         Debug.Log($"JoinRandom Failed {returnCode}:{message}");

//         // PhotonNetwork.CreateRoom("Main Room");
//         // 방을 생성하는 함수 호출
//         OnMakeRoomClick();
//     }

//     // 방 목록이 업데이트될 때 호출되는 콜백 함수
//     public override void OnRoomListUpdate(List<RoomInfo> roomList)
//     {
//         // 방 목록 갱신

//         foreach (var roomInfo in roomList)
//         {
//             // 방이 삭제된 경우
//             if (roomInfo.RemovedFromList == true)
//             {
//                 // 해당 방 아이템 제거

//             }
//             else // 방이 추가된 경우
//             {
//                 // 해당 방이 이미 방 목록에 있는지 확인
//                 if (rooms.ContainsKey(roomInfo.Name) == false)
//                 {
//                     // 새로운 방을 방 목록에 추가

//                 }
//                 else // 방이 이미 방 목록에 있는 경우
//                 {
//                     // 방 목록에 이미 존재하는 방이므로 추가 작업 필요 없음

//                 }
//             }
//         }
//     }

//     // 사용자 ID 설정
//     public void SetUserId()
//     {
//         if (string.IsNullOrEmpty(userIF.text))
//         {
//             // 사용자 ID가 입력되지 않은 경우의 처리

//         }
//         else
//         {
//             // 사용자 ID가 입력된 경우의 처리

//         }
//         // 사용자 ID 갱신

//         // Photon 서버에 사용자 ID 적용
//     }

//     // 로그인 버튼 클릭 시 호출되는 함수
//     public void OnLoginClick()
//     {
//         // 로그인 처리

//         // 사용자 ID 갱신

//     }

//     // 캐릭터 선택 함수
//     public void player1Select()
//     {
//         // 캐릭터 1 선택
//         CharacterValue = 1;
//         player1.SetActive(true);
//         player2.SetActive(false);
//         player3.SetActive(false);
//         player4.SetActive(false);
//     }
//     // 기타 캐릭터 선택 함수

//     // 방 이름 설정
//     string SetRoomName()
//     {
//         if (string.IsNullOrEmpty(roomNameIF.text))
//         {
//             // 방 이름이 입력되지 않은 경우의 처리
//             roomNameIF.text = $"ROOM_{Random.Range(1, 101):000}";
//         }
//         return roomNameIF.text;
//     }

//     // 방 생성 시 호출되는 함수
//     public override void OnCreatedRoom()
//     {
//         Debug.Log("Created Room");
//         Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
//     }

//     // 방 만들기 버튼 클릭 시 호출되는 함수
//     public void OnMakeRoomClick()
//     {
//         // 사용자 ID 설정

//         // 방 설정

//         // 방 생성

//         // 방 생성 결과 처리
//     }
// }

}
