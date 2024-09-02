using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.Text.RegularExpressions;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    // 새로운 PlayerStatusInfo 클래스 정의
    [System.Serializable]
    public class PlayerStatusInfo
    {
        public bool isAlive;
        public int count; // 투표를 받은 횟수
        public string job; // 직업명
        public string group; // 세력명
        public PlayerStatusInfo(bool alive, int voteCount, string jobName, string groupName )
        {
            isAlive = alive;
            count = voteCount;
            job = jobName;
            group = groupName;
        }
    }

    [System.Serializable]
    public class PlayerStatusEntry
    {
        public int viewID;
        public PlayerStatusInfo statusInfo;
    }

    [System.Serializable]
    public class PlayerStatusList
    {
        public List<PlayerStatusEntry> list = new List<PlayerStatusEntry>();
    }

    public static PlayerManager Instance; // 싱글턴 인스턴스
    public int kou;

    public static int vote_num;

    public  int wild_num;

    public  int spy_num;

    public  int neutrality_num ;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 플레이어의 view ID를 키로 하고 PlayerStatusInfo를 값으로 하는 사전
    private Dictionary<int, PlayerStatusInfo> playerStatus = new Dictionary<int, PlayerStatusInfo>();

    void Update()
    {

        // // G 키가 눌렸는지 확인
        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     PopulatePlayerStatus();



        // }
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     foreach (var kvp in playerStatus)
        // {
        //    Debug.Log("View ID: " + kvp.Key + ", isAlive: " + kvp.Value.isAlive + ", Count: " + kvp.Value.count + ", job: " + kvp.Value.job + ", group: " + kvp.Value.group);
        // }
        // }

        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     photonView.RPC("test", RpcTarget.All);

        // }




        // 세력별 숫자를 0으로 초기화
        int wild_num = 0;
        int spy_num = 0;
        int neutrality_num = 0;

        // 딕셔너리 순회
        foreach (var entry in playerStatus)
            {
                if (entry.Value.isAlive) // 플레이어가 살아있는 경우에만 계산
                {
                    switch (entry.Value.group)
                    {
                        case "Wild":
                            wild_num++;
                            break;
                        case "Spy":
                            spy_num++;
                            break;
                        case "neutrality":
                            neutrality_num++;
                            break;
                        default:
                            // 기타 세력에 대한 처리 (필요한 경우)
                            break;
                    }
                }
            }
            // 세력별 살아있는 플레이어 수 디버그 로그 출력 (필요한 경우)
            Debug.Log($"Alive - Wild: {wild_num}, Spy: {spy_num}, Neutrality: {neutrality_num}");

        


    }
   

    public void PlayerStatus()
    {
        photonView.RPC("PopulatePlayerStatus", RpcTarget.All);
    }

    [PunRPC]
    public void LoadEndScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }


    [PunRPC]
    public void PopulatePlayerStatus()
    {
        // 기존 사전을 비우기
        playerStatus.Clear();

        // 씬 내의 모든 플레이어 객체 찾기
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject playerObj in players)
        {
            PhotonView photonView = playerObj.GetComponent<PhotonView>();
            PlayerController playerController = playerObj.GetComponent<PlayerController>();

            // photonView와 playerController가 null이 아닌지 확인
            if (photonView != null && playerController != null)
            {
                
                string job = JobManager.Instance.userJobInfo[photonView.ViewID];

                string group;

                if (new List<string>{"Knight", "Sheriff", "Vigilante", "Detective", "HackerSkill", "Nongae", "Star"}.Contains(job))
                {
                    group = "Wild"; // wildJobList에 해당하는 경우, group을 "Wild"로 설정합니다.
                    
                }
                else if (new List<string>{"Bomber"}.Contains(job))
                {
                    group = "Spy"; // spyJobList에 해당하는 경우, group을 "Spy"로 설정합니다.
                    
                }
                // 여기에 더 많은 직업 그룹을 추가할 수 있습니다.
                else
                {
                    group = "neutrality"; // 중립 직업인 경우, group을 "neutrality"로 설정합니다.
                    
                }

      
            
                // 플레이어의 view ID와 PlayerStatusInfo를 사전에 추가
                playerStatus.Add(photonView.ViewID, new PlayerStatusInfo(playerController.isAlive, 0, job, group)); // 초기 투표 수는 0으로 설정
                
        
            }
        }

        // 디버깅을 위해 - 모든 view ID, 생존 상태 및 투표 수를 출력
        foreach (var kvp in playerStatus)
        {
            Debug.Log("View ID: " + kvp.Key + ", isAlive: " + kvp.Value.isAlive + ", Count: " + kvp.Value.count + ", job: " + kvp.Value.job + ", group: " + kvp.Value.group);
        }
    }


    [PunRPC]
    public void IncrementCountRPC(int key)
    {
        kou = key;
        if (PhotonNetwork.IsMasterClient)
        {
            if (playerStatus.ContainsKey(key))
            {
                playerStatus[key].count++;
                Debug.Log($"View ID: {key}, New Count: {playerStatus[key].count}");
            }
            else
            {
                Debug.LogWarning($"Player with View ID: {key} not found.");
            }


        }
    }

    public void IncrementCount(int key)
    {
        if (vote_num == 0)
        {
            photonView.RPC("IncrementCountRPC", RpcTarget.All, key);
            vote_num++; // 투표 수를 증가
        }
        
    }


    public void UpdateAliveStatus()
    {
        
        Debug.Log("dd");
        if (PhotonNetwork.IsMasterClient)
        {
            int highestCount = 0;
            int playerWithHighestCount = -1; // 초기 값은 -1로 설정하여 유효하지 않은 상태를 나타냄
            bool isTie = false; // 동점자 여부를 확인

            foreach (var entry in playerStatus)
            {
                if (entry.Value.count > highestCount)
                {
                    highestCount = entry.Value.count;
                    playerWithHighestCount = entry.Key;
                    isTie = false; // 새로운 최고값을 찾았으므로 동점자가 아님
                }
                else if (entry.Value.count == highestCount)
                {
                    isTie = true; // 동점자 발견
                }
            }

            // 가장 높은 count 값을 가진 player가 유일하고, 그 player를 찾았다면
            if (!isTie && playerWithHighestCount != -1)
            {
                // 그 player의 isAlive를 false로 설정
                if (playerStatus.ContainsKey(playerWithHighestCount))
                {
                    playerStatus[playerWithHighestCount].isAlive = false;
                    Debug.Log($"Player with View ID: {playerWithHighestCount} has been eliminated.");

                    // Player 오브젝트를 찾습니다.
                    PhotonView targetPhotonView = PhotonView.Find(playerWithHighestCount);
                    photonView.RPC("KillPlayerRPC", RpcTarget.All, playerWithHighestCount);
                    if (targetPhotonView != null && targetPhotonView.gameObject != null)
                    {
                        // 'Cat' 자식 오브젝트를 찾아 비활성화합니다.
                        Transform catTransform = targetPhotonView.gameObject.transform.Find("Cat");
                        Transform Speaker = targetPhotonView.gameObject.transform.Find("Speaker");
                        if (catTransform != null)
                        {
                            catTransform.gameObject.SetActive(false);
                            Speaker.gameObject.SetActive(false);
                        }
                        else
                        {
                            Debug.LogWarning("Cat 오브젝트를 찾을 수 없습니다.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"View ID: {playerWithHighestCount}를 가진 Player 오브젝트를 찾을 수 없습니다.");
                    }
                }
            }

            // 모든 플레이어의 count 값을 0으로 리셋
            foreach (var key in playerStatus.Keys.ToList())
            {
                playerStatus[key].count = 0;
            }


            Debug.Log("dddd");
            photonView.RPC("test", RpcTarget.All);
            SerializeAndSendPlayerStatus();

            // 변경 사항을 모든 클라이언트에 동기화하기 위한 다른 RPC 호출 또는 동기화 메커니즘을 여기에 추가합니다.
        }
    }

    [PunRPC]
    public void UpdateStatusRPC(string serializedPlayerStatus)
    {
        PlayerStatusList playerStatusList = JsonUtility.FromJson<PlayerStatusList>(serializedPlayerStatus);
        playerStatus.Clear(); // 기존 딕셔너리를 클리어합니다.
        foreach (var item in playerStatusList.list)
        {
            playerStatus[item.viewID] = item.statusInfo;
            Debug.Log($"View ID: {kou}, New Count: {playerStatus[kou].count}");
        }
    }

    public void SerializeAndSendPlayerStatus()
    {
        
        Debug.Log("ddsss");
        PlayerStatusList playerStatusList = new PlayerStatusList();
        foreach (var item in playerStatus)
        {
            playerStatusList.list.Add(new PlayerStatusEntry { viewID = item.Key, statusInfo = item.Value });
        }

        string json = JsonUtility.ToJson(playerStatusList);
        photonView.RPC("UpdateStatusRPC", RpcTarget.Others, json);
    }


    [PunRPC]
    public void KillPlayerRPC(int targetViewID)
    {
        PhotonView targetPhotonView = PhotonView.Find(targetViewID);
        Transform catTransform = targetPhotonView.gameObject.transform.Find("Cat");
        Transform Speaker = targetPhotonView.gameObject.transform.Find("Speaker");

        if (catTransform != null)
        {
            catTransform.gameObject.SetActive(false);
            Speaker.gameObject.SetActive(false);
            
        }
        else
        {
            Debug.LogWarning("Cat 오브젝트를 찾을 수 없습니다.");
        }
    }

    [PunRPC]
    public void test()
    {
        StartCoroutine(CountdownCoroutine2(2));
    }

    IEnumerator CountdownCoroutine2(int count)
    {
        
        for (int i = count; i > 0; i--)
        {
            Debug.Log("Countdown: " + i);
            yield return new WaitForSeconds(1f); // 1초 대기
        }
       
            GameObject alivePlayer = GameObject.FindGameObjectWithTag("Player");
            alivePlayer.transform.position = new Vector3(14, 10, 0);
        
    }

    public bool CheckIfPlayerIsAlive(PhotonView photonView)
    {
        // 현재 유저의 ViewID를 얻습니다.
        int currentViewID = photonView.ViewID;

        // playerStatus 딕셔너리에서 현재 유저의 PlayerStatusInfo 객체를 찾습니다.
        if (playerStatus.TryGetValue(currentViewID, out PlayerStatusInfo playerInfo))
        {
            // isAlive 상태를 반환합니다.
            return playerInfo.isAlive;
        }
        else
        {
            // 딕셔너리에서 해당 ViewID를 찾을 수 없는 경우, 에러를 로그하고 false를 반환할 수 있습니다.
            Debug.LogError($"Player with ViewID {currentViewID} not found.");
            return false;
        }
    }


}