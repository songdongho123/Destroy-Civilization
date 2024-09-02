using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class JobManager : MonoBehaviourPunCallbacks
{
    // JobMananger.cs 정적 변수 설정
    public static JobManager Instance;

    // API POST 요청 데이터 직업 정하기
    [System.Serializable]
    public class PostDataJobRandom
    {
        public List<int> userCodes = new List<int>();
        public int spyNum;
        public int selfishNum;
        public int wildNum;
    }


    // 어떤 코드가 어떤 세력인지?
    List<string> spyJobList = new List<string> { "Bomber", "Trapper" };
    List<string> selfisJobList = new List<string> { "Pelican", "Falcon" };
    List<string> wildJobList = new List<string> { "Knight", "Sheriff", "Vigilante", "Detective", "HackerSkill", "Nongae", "Star", "Macgyver", "AlterEgoSorcerer" };


    // 전체 직업 스크립트 string 이름 가져오기
    public List<string> jobClassNames;

    public List<string> wildClassNames = new List<string> { "Knight", "Sheriff", "Vigilante", "Detective", "HackerSkill", "Nongae", "Star", "Macgyver", "AlterEgoSorcerer" };
    public List<string> spyClassList = new List<string> { "Bomber", "Trapper" };
    public List<string> selfisClassList = new List<string> { "Pelican", "Falcon" };

    public List<string> totalClassList;

    // 랜덤 직업 분배를 위한 인덱스 그릇
    public int randomIndex;

    // 랜덤 직업 분배한 후, 각 플레이어에 할당된 직업 데이터
    public Dictionary<int, string> userJobInfo = new Dictionary<int, string>();

    public string playerChecker = "1";


    private void Awake()
    {
        if (Instance != null)
        {
            // Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private int previousPlayerCount = 0; // 이전 프레임의 플레이어 수

    // 추후 In Game Lobby 에서 게임 시작 버튼을 눌렀을 때, 작동
    private void Update()
    { /*
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int currentPlayerCount = players.Length; // 현재 프레임의 플레이어 수
        if (currentPlayerCount != previousPlayerCount) // 업데이트를 통해 같지 않다면 totalClassList에 데이터 추가
        {
            //int randomNum = UnityEngine.Random.Range(0, totalClassList.Count+1);
            //int randomWild = UnityEngine.Random.Range(0, wildClassNames.Count);
            //int randomSpy = UnityEngine.Random.Range(0, spyClassList.Count);
            //int randomSel = UnityEngine.Random.Range(0, selfisClassList.Count);

            switch (totalClassList.Count)
            {
                case 0:
                case 2:
                case 3:
                case 4:
                case 5:
                case 8:
                case 9:
                case 11:
                    totalClassList.AddRange(wildClassNames.GetRange(0, 1));
                    wildClassNames.RemoveAt(0);
                    Debug.Log(totalClassList.Count + "시민측 카운트 체크");
                    break;
                case 1:
                case 7:
                    totalClassList.AddRange(spyClassList.GetRange(0, 1));
                    spyClassList.RemoveAt(0);
                    Debug.Log(totalClassList.Count + "스파이측 카운트 체크");
                    break;
                case 6:
                case 10:
                    totalClassList.AddRange(selfisClassList.GetRange(0, 1));
                    selfisClassList.RemoveAt(0);
                    Debug.Log(totalClassList.Count + "중립측 카운트 체크");
                    break;
            }

            previousPlayerCount = currentPlayerCount;
        }*/

        if (Input.GetKeyDown(KeyCode.C))
        {
            // 마스터 클라이언트만, AssignJobToPlayers() 함수 호출.
            if (PhotonNetwork.IsMasterClient)
            {
                // API POST 요청 직업 정하기
                PostDataJobRandom postDataJobRandom = new PostDataJobRandom();
                postDataJobRandom.userCodes.Add(52);
                postDataJobRandom.userCodes.Add(53);
                postDataJobRandom.userCodes.Add(54);
                postDataJobRandom.spyNum = 1;
                postDataJobRandom.selfishNum = 1;
                postDataJobRandom.wildNum = 1;
                // StartCoroutine(DataManager.instance.SendApiRequest("http://192.168.31.237:8082/job/random", "POST", postDataJobRandom, onCompleteJobRandom, onErrorJobRandom));
                // StartCoroutine(DataManager.instance.SendApiRequest("https://j10b310.p.ssafy.io/job/random", "POST", postDataJobRandom, onCompleteJobRandom, onErrorJobRandom));

                AssignJobToPlayers();

            }
        }
        ////////////////////////////
        // (데이터 확인용 출력 코드)
        if (Input.GetKeyDown(KeyCode.V))
        {
            foreach (var info in userJobInfo)
            {
                print("viewID : " + info.Key + " / job : " + info.Value);
            }
            JobFromViewID(1001);
            FindKnightViewID();
        }
        ///////////////////////////
    }

    // API POST 요청 직업 정하기 성공
    public void onCompleteJobRandom(string response)
    {
        print("JobManager.cs Update() : " + response);
    }
    // API POST 요청 직업 정하기 실패
    public void onErrorJobRandom(string error)
    {
        print("JobManager.cs Update() : " + error);
    }



    // 함수를 호출하면, 해당 ViewID 플레이어의 시민 마피아 중립 return 하는 함수
    string jobKorea;
    public string[] JobFromViewID(int viewID)
    {
        // 특정 viewID 의 해당 직업 추출
        string job = userJobInfo[viewID];

        if (wildJobList.Contains(job))
        {
            print("시민 : " + jobKorea);
            return new string[] { "시민", job };
        }
        else if (spyJobList.Contains(job))
        {
            print("스파이 : " + jobKorea);
            return new string[] { "스파이", job };
        }
        else if (selfisJobList.Contains(job))
        {
            print("중립 : " + jobKorea);
            return new string[] { "중립", job };
        }
        return new string[] { "[Error]", "JobManager.cs JobFromViewID()" };
    }

    // 함수를 호출하면, 현재 기사 직업인 플레이어 ViewID return 하는 함수
    public int FindKnightViewID()
    {
        if (userJobInfo.ContainsValue("Knight"))
        {
            foreach (var data in userJobInfo)
            {
                if (data.Value == "Knight")
                {
                    print("기사 직업인 플레이어 View ID : " + data.Key);
                    return data.Key;
                }
            }
            return 0;
        }
        else
        {
            print("현재 플레이어 중에 기사 직업이 없다.");
            return 0;
        }
    }


    /// //////////////////////////////////////////////////////////////////////////////
    /// //////////////////////////////////////////////////////////////////////////////    
    // 현재 클라이언트가 이 코드를 실행한다.
    [PunRPC]
    public void AssignJobToPlayersRPC(int viewID, int index)
    {
        // 현재 클라이언트 씬에 있는 모든 PhotonView 오브젝트 중에
        // 매개 변수 ViewID 에 해당하는 PhotonView 오브젝트를 가지고 온다. 
        PhotonView pv = PhotonView.Find(viewID);

        // ViewID 에 해당하는 PhotonView 오브젝트 플레이어를 찾고,
        // 아직 배분하지 못한 직업이 있다면,
        if (pv != null && jobClassNames.Count > 0)
        {
            // 랜덤 인덱스에 해당하는 직업 스크립트 이름 추출. 
            string jobName = jobClassNames[index];

            // 랜덤 인덱스에 해당하는 전체 직업 스크립트 요소 삭제
            jobClassNames.RemoveAt(index);

            // 직업 스트립트 이름을 직업 코드를 변환해서 추출.
            Type jobType = Type.GetType(jobName);

            // 직업 코드가 존재한다면.
            if (jobType != null)
            {
                // ViewID 에 해당하는 PhotonView 오브젝트에 스크립트 컴포넌트 추가
                pv.gameObject.AddComponent(jobType);

                // ViewID 와 해당 직업 데이터 userJobInfo 에 저장
                userJobInfo.Add(viewID, jobName);
            }
            else
            {
                // 직업 코드가 존재하지 않는다면, (or 플레이어 > 직업 코드)
                Debug.LogError("Job type not found : " + jobName);
            }
        }
    }

    // 게임 시작 초기, 직업 배분하는 함수
    public void AssignJobToPlayers()
    {
        // 현재 씬에 있는 모든 "Player" 태그로된 게임 오브젝트를 가져온다.
        // 모든 플레이어 오브젝트를 가져오기.
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 모든 "Players" 오브젝트를 반복해서 돌린다.
        foreach (var player in players)
        {
            // "Player" 오브젝트의 PhotonView 컴포넌트 가져오기
            PhotonView pv = player.GetComponent<PhotonView>();

            if (pv != null)
            {
                // 출력 예상 값 : 1001 , 2001, 3001, 4001 ...
                print(pv.ViewID);

                // 인덱스 랜덤 부여 (직업 랜덤 분배를 위함)
                //randomIndex = UnityEngine.Random.Range(0, jobClassNames.Count);
                randomIndex = UnityEngine.Random.Range(0, jobClassNames.Count);
                Debug.Log("!!!! jobClassNames.Count :" + jobClassNames.Count);
                Debug.Log("!!!! totalClassNames.Count :" + jobClassNames.Count);

                // "Player" 오브젝트의 ViewID 와 인덱스 랜덤을 매개변수로 가지고,
                // 모든 클라이언트 측에 있는 AssignJobToPlayersRPC() 함수를 호출한다.
                photonView.RPC("AssignJobToPlayersRPC", RpcTarget.All, pv.ViewID, randomIndex);
            }
        }
    }
}
