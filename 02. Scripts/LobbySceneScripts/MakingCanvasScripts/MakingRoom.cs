using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MakingRoom : MonoBehaviourPunCallbacks
{
    public MakingMap makingMap;
    public MakingMode makingMode;

    public GameObject makingMapCanvas;

    [Header("Selected Mod Map IMG")]
    public GameObject modePanel;
    public GameObject mapPanel;

    [Header("Room Options")]
    public TMP_InputField roomTitleInput;
    public Button publicButton;
    public Button privateButton;
    public Slider numberSlider;
    public TMP_Text numberText;

    // 방 옵션 (인원 수) 데이터
    int numberplayer = 3;
    // 공개 비공개 데이터
    bool publicOption = true;

    // API POST 요청 방 만들기
    [System.Serializable]
    public class PostDataRoomCreat{
        public int serverCode;
        public string roomTilte;
        public int roomPeople;
        public int roomLimit;
        public string gameCode;
        public string mapCode;
        public int lastRoom;
    }

    private void Update() {
        // 앞에서 선택한 모드 이미지 출력
        Debug.Log("[MakingRoom.cs] makingMode.modeImage : " + ServerData.Instance.modeImage);
        Image modeImg = modePanel.GetComponent<Image>();
        modeImg.sprite = ServerData.Instance.modeImage;

        // 앞에서 선택한 맵 이미지 출력
        Debug.Log("[MakingRoom.cs] makingMap.mapImage : " + ServerData.Instance.mapImage);
        Image mapImg = mapPanel.GetComponent<Image>();
        mapImg.sprite = ServerData.Instance.mapImage;

        // 인원 수 슬라이드 바
        numberSlider.onValueChanged.AddListener((n) => {
            numberplayer = (int)n;
            numberText.text = n.ToString("0");
        });
    }

    // 뒤로 가기 (Making Map Canvas 이동)
    public void BackToLobbyRoom(){
        makingMapCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    // 공개 비공개 버튼 토글
    public void ClickToggleButton(Button selectButton){
        if(selectButton == publicButton){
            publicButton.image.color = Color.green;
            privateButton.image.color = Color.white; 
            publicOption = true;
            Debug.Log("Public Button 클릭 : " + selectButton);
        }
        else if(selectButton == privateButton){
            publicButton.image.color = Color.white;
            privateButton.image.color = Color.green; 
            publicOption = false;
            Debug.Log("Private Button 클릭 : " + selectButton);
        }
    }

    // 게임 방 생성하기 클릭
    public void ClickGoToGameLobbyScene(){
        CreatedRoom();
    }

    // 게임 방 생성하는 함수
    // (PhotonNetwork.CreateRoom() 실행 후 -> LobbySceneMananger.cs_ public override OnJoinedRoom() 실행)
    public void CreatedRoom()
    {   
        print("MakingRoom.cs CreatedRoom() : ");

        // Room Create 방 만들기 POST 요청 실행
        PostDataRoomCreat postDataRoomCreat = new PostDataRoomCreat();
        postDataRoomCreat.serverCode = ServerData.Instance.serverCode;
        postDataRoomCreat.roomTilte = roomTitleInput.text;
        postDataRoomCreat.roomPeople = 1;
        postDataRoomCreat.roomLimit = numberplayer;
        postDataRoomCreat.gameCode = "기본 모드";
        postDataRoomCreat.mapCode = "도시 맵";
        postDataRoomCreat.lastRoom = ServerData.Instance.lastRoom + 1;
        print(postDataRoomCreat.lastRoom);
        print(ServerData.Instance.lastRoom + 1);


        // StartCoroutine(DataManager.instance.SendApiRequest("https://j10b310.p.ssafy.io/room/create", "POST", postDataRoomCreat, onComplete, onError));

        // 방제 , 인원 수, 비공개/공개 , 입장허용 , 나머지 옵션 default
        LobbySceneManager.Instance.CreatedRoom(roomTitleInput.text, numberplayer, publicOption);
    }

    // API POST 요청 방 만들기 성공
    void onComplete(string response){
        print("MakingRoom.cs CreatedRoom() : " + response);
    }

    // API POST 요청 방 만들기 실패
    void onError(string error){
        Debug.LogError("MakingRoom.cs CreatedRoom() : " + error);
    }









    ////////////////////////
    ///////////////////////
    /// API XXX
    

    // [System.Serializable]
    // public class LocalDataRoom{
    //     public string roomTilte;
    //     public int roomPeople;
    //     public string gameCode;
    //     public string mapCode;
    // }
    // public void CreatedRoom(){
    //     print("MakingRoom.cs CreatedRoom() : ");

    //     // LocalDataRoom localDataRoom = new LocalDataRoom();
    //     // localDataRoom.roomTilte = roomTitleInput.text;
    //     // localDataRoom.roomPeople = numberplayer;
    //     // localDataRoom.gameCode = "기본 모드";
    //     // localDataRoom.mapCode = "도시 맵";
    //     // DataManager.instance.SaveJsonData(localDataRoom, "APIPostRoomCreateData.json");

    //     // 방제 , 인원 수, 비공개/공개 , 입장허용 , 나머지 옵션 default
    //     LobbySceneManager.Instance.CreatedRoom(roomTitleInput.text, numberplayer, publicOption);
        
    // }    
}
