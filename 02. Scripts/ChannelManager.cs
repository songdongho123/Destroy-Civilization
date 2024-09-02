using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

// public class ChannelManager : MonoBehaviour
public class ChannelManager : MonoBehaviourPunCallbacks
{
    public GameObject modalPanel; // ���� ������ ����� ��� ó���� ���� ����


    // API GET 요청 서버 선택
    [System.Serializable]
    public class ResponsGetDataServerSelect{
        public int serverCode;
        public int serverPeople;
        public int serverLimit;
        public int lastRoom;
        public Array rooms;
    }


    // API POST 요청 서버 업데이트
    [System.Serializable]
    public class PutDataRommUpdate{
        public int serverCode;
        public int serverPeople;
    }


    // 배열 요청을 위한 클래스와 함수
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"array\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }
        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }


    /// 서버 접속 전에 세팅 정보
    private readonly string version = "1.0f"; // 게임 버전
    
    private void Awake() {
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = ServerData.Instance.userName;

        // API POST 요청 서버 업데이트
        PutDataRommUpdate putDataRommUpdate = new PutDataRommUpdate();
        putDataRommUpdate.serverCode = 1;
        putDataRommUpdate.serverPeople = 0;
        // StartCoroutine(DataManager.instance.SendApiRequest("https://j10b310.p.ssafy.io/server/update", "PUT", putDataRommUpdate, onComplete2, onError2));


        // 포톤 서버와 통신 횟수(30)
        Debug.Log(PhotonNetwork.SendRate);
    }
    // API POST 요청 서버 업데이트 성공
    void onComplete2(string response){
        print("$$$$" + response);
    }
    // API POST 요청 서버 업데이트 실패
    void onError2(string error){
        Debug.LogError("***" + error);
    }

    void Start()
    {
        modalPanel.SetActive(false); // ó���� ��� ��Ȱ��ȭ
    }
    public void ConnectToChannel(int channelNumber)
    {
        // 서버 버튼 클릭 시 호출될 함수
        // API GET 요청 서버 선택
        // StartCoroutine(DataManager.instance.SendApiRequest("https://j10b310.p.ssafy.io/server/list", "GET", null, onComplete, onError));
        
        string appId = GetAppIdByChannel(channelNumber); // channelNumber�� �´� appId �Ҵ�

        // ���ο� AppSettings ��ü�� �����ϰ�, AppIdRealtime�� ����
        AppSettings settings = new AppSettings()
        {
            AppIdRealtime = appId,
        };

        // (global 전역) 게임 매니저에 appId 저장
        ServerData.Instance.settings = settings;

        PhotonNetwork.ConnectUsingSettings(settings); // ���� ���� ����
    }

    ///////////////////////////////////////////////////
    //////////////////////////////////////////////////////

    /// 서버 접속 후 -> 마스터 서버 -> Lobby Scene 이동
    public override void OnConnectedToMaster()
    {
        // base.OnConnectedToMaster();
        Debug.Log("Connected To MAster");
        PhotonNetwork.JoinLobby();
    } 
    public override void OnJoinedLobby()
    {   
        // StartCoroutine(RequestGetServerList());
        // base.OnJoinedLobby();
        SceneManager.LoadScene("LobbyScene");
    }

    // API GET 요청 서버 선택 성공
    void onComplete(string response){
        print("ChannelManager.cs ConnectToChannel() : " + response);
        
        // Json 직렬화
        ResponsGetDataServerSelect[] responseData = JsonHelper.FromJson<ResponsGetDataServerSelect>(response);

        // 현재 서버 정보 추출 (ServerData 저장)
        ServerData.Instance.serverCode = responseData[0].serverCode;
        ServerData.Instance.serverPeople = responseData[0].serverPeople;
        ServerData.Instance.serverLimit = responseData[0].serverLimit;
        ServerData.Instance.lastRoom =  responseData[0].lastRoom;
        ServerData.Instance.rooms = responseData[0].rooms;

        print("ChannelManager.cs ConnectToChannel() : 서버 입장!!");
    }
    // API GET 요청 서버 선택 실패
    void onError(string error){
        Debug.LogError("ChannelManager.cs ConnectToChannel() : " + error);
    }


    //////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////

    // ���� ���� ����

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log($"���� ���� ����: {cause}"); // ���� ���� ���� �α�
        modalPanel.SetActive(true); // ���� ���������� ��� UI�� �����ش�.
    }

    public void ModalBtnClick() // ��ư Ŭ���ϸ� ��� ���ִ� ����
    {
        modalPanel.SetActive(false);
    }



    public void PlayerCounter() // �÷��̾� �� Ȯ���Լ�
    {
        int playerCount = PhotonNetwork.CountOfPlayersOnMaster; // ������ ������ �ִ� �÷��̾� ��, �κ� ����
        Debug.Log("���� �κ� �ִ� ���� ��: " + playerCount);
    }

    private string GetAppIdByChannel(int channelNumber) // channelNumber�� ������ AppId�� return
    {
        switch (channelNumber)
        {
            case 1: return "1bed5af5-423f-4e36-872f-365e84daed36"; // ���� App id PUN
            case 2: return "aa2de7f0-7461-498a-92c6-1b3c375c584f"; // ��ȣ App    
            case 3: return "d9302e11-06d7-49f0-8c92-ff23cec623c9"; // ���� App
            case 4: return "f794b165-4232-4fb2-80af-3b441256c86d"; // ���� App
            case 5: return "c8286018-b331-478d-a7ed-3749f12d7894"; // ���� App
            case 6: return "b55a1d31-ae4b-43af-95b6-a200413aa09a"; // ���� App

            default: return "null";
        }
    }

    private string GetChatAppIdByChannel(int channelNumber) // ���߿� ä�� ������ ��� �Ҵ��� Chat ���� AppId
    {
        // �� �޼���� ä�� ��ȣ�� ���� �ش��ϴ� AppId�� ��ȯ�ؾ� �մϴ�.
        switch (channelNumber)
        {
            case 1: return "0d2182a0-fd5e-4ea4-a679-fba05aa7809d"; // ���� App id Chat
            case 2: return "7631419a-1062-4a40-a537-6dbf6d93c1a5"; // ��ȣ App    
            case 3: return "27092388-5f9f-4aec-9f8b-83d65f965f79"; // ���� App
            case 4: return "46181d98-613b-43b6-9585-eff76ca5c770"; // ���� App
            case 5: return "92bd46a3-78f1-4979-b2ce-a39702dea496"; // ���� App
            case 6: return "47c3ddeb-878b-4190-a962-31d66fb2a66c"; // ���� App

            default: return "null";
        }
    }
}
