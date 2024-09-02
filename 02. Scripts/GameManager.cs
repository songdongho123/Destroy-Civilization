using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameManager : MonoBehaviourPun
{
    public static GameManager Instance;

    // 플레이어 상태
    public enum PlayerState
    {
        
        Ready,  // 준비됨 (게임이 진행되지 않을 때)
        Alive,  // 생존
        Dead,   // 사망
    }

    // 플레이어 정보를 나타내는 클래스
    [PunRPC]
    [System.Serializable]
    public class PlayerInfo
    {
        public PhotonView playerId; // PhotonView for player ID
        public PlayerState state;   // 상태
        public string role;         // 역할
        public string job;          // 직업

        public PlayerInfo(PhotonView view)
        {
            playerId = view;
            state = PlayerState.Ready;
            role = string.Empty;
            job = string.Empty;
        }
    }

    // 플레이어 정보를 저장하는 리스트
    public List<PlayerInfo> playerInfos = new List<PlayerInfo>();
    private string fileName = "playerInfo.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    void Start()
    {
        // 게임 시작 시 플레이어 정보를 로드
        LoadPlayerInfo(fileName);
    }
    public void SavePlayerInfo()
    {
        string json = JsonUtility.ToJson(new PlayerInfoListWrapper(playerInfos));
        DataManager.instance.SaveJsonData(json, fileName);
        Debug.Log("PlayerInfo saved to: " + Path.Combine(Application.persistentDataPath, fileName));
    }

    //JSON 파일 Load
    public void LoadPlayerInfo(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerInfoListWrapper wrapper = JsonUtility.FromJson<PlayerInfoListWrapper>(json);
            playerInfos = wrapper.playerInfoList;
            Debug.Log("PlayerInfo loaded from: " + filePath);
        }
    }

    [System.Serializable]
    private class PlayerInfoListWrapper
    {
        public List<PlayerInfo> playerInfoList;

        public PlayerInfoListWrapper(List<PlayerInfo> list)
        {
            playerInfoList = list;
        }
    }



    

    // 투표 기능

    [PunRPC]
    public void SignVoteEject()
    {
        GetComponent<MeetingUI>().UpdateVote();
    }
}
