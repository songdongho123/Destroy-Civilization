using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Report : MonoBehaviourPunCallbacks
{
    public static bool voting;
    

    private Camera characterCamera;         // 캐릭터에 붙은 카메라
    private GameObject reportDeactive;      // 신고 비활성화 버튼 GameObject
    private GameObject ReportCanvas;

    public GameManager gameManager;
   
    private float reportDistance;
    void Start()
    {

        // 캐릭터에 붙은 카메라 찾기
        characterCamera = GetComponentInChildren<Camera>();

        // ReportDeactive GameObject 초기화
        GameObject gameUI = GameObject.FindGameObjectWithTag("GameUI");
        
        Transform reportBtn = gameUI.transform.Find("ReportBtn");
        reportDeactive = reportBtn.Find("ReportDeactive").gameObject;
        
    }


    // 신고 기능
    public void CallReport()
    {
        CmdReport();
    }

    public void Update()
    {
        ActivateReport();
    }

    // 신고 활성화하는 함수
    public void ActivateReport()
    {
        reportDistance = 20f;

        RaycastHit hit;

        // 카메라에서 마우스 위치로 레이를 쏘기
        Debug.DrawRay(transform.position, transform.forward * reportDistance, Color.yellow);

        // Raycast를 발사하여 충돌한 경우
        if (Physics.Raycast(transform.position, transform.forward, out hit, reportDistance))
        {
            // 충돌한 오브젝트의 태그가 "DeadBody"인 경우에만 처리
            if (hit.collider.CompareTag("DeadBody"))
            {
                // 신고 기능 실행
                reportDeactive.gameObject.SetActive(false);
                return;
            }
        }
        // 충돌하지 않은 경우 또는 다른 태그의 오브젝트에 충돌한 경우에도 신고 버튼을 활성화
        reportDeactive.gameObject.SetActive(true);
    }


    [PunRPC]
    public void CmdReport()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();

        playerController.isReporter = true;
        StartMeeting();
    }
    public void StartMeeting()
    {
        Debug.Log("StartMeeting");
        /*photonview.RPC("SendReportSign", RpcTarget.All);*/
        SendReportSign();
    }

    /*[PunRPC]*/
    public void SendReportSign()
    {
        Debug.Log("SendReport");
        photonView.RPC("Reporting", RpcTarget.All);
        /*StartCoroutine(StartMeeting_Coroutine());*/
    }
    [PunRPC]
    public void Reporting()
    {
        
       
        StartCoroutine(CountdownCoroutine(2));
        
    }

    // 신고 기능
    private IEnumerator StartMeeting_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<ReportUI>().Close();

        GetComponent<MeetingUI>().Open();


        
        
    }
    //3초를 세는 루프
    IEnumerator CountdownCoroutine(int count)
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        Transform reportImg = canvas.transform.Find("ReportUI");
        GameObject reporting = reportImg.gameObject;

        GameObject canvas2 = GameObject.FindGameObjectWithTag("VoteCanvas");
        Transform reportImg2 = canvas2.transform.Find("VotePaper");
        // reportImg2.transform.position = new Vector3(0,0,0);
        GameObject reporting2 = reportImg2.gameObject;


        reporting.SetActive(true);
        yield return new WaitForSeconds(1f);
        reporting2.SetActive(true);
       
        if (!voting)
        {
            voting = true;
        }
        Debug.Log("여기는");
        for (int i = count; i > 0; i--)
        {
            Debug.Log("Countdown: " + i);
            yield return new WaitForSeconds(1f); // 1초 대기
        }
        reporting.SetActive(false);

            GameObject alivePlayer = GameObject.FindGameObjectWithTag("Player");
            alivePlayer.transform.position = new Vector3(-253, 161, -207);
        
        Debug.Log("ㅇㅇㅋㅋㅋ");
        yield return new WaitForSeconds(10f);
        reporting2.SetActive(false);

        Debug.Log("진입ㅇㅇㅇㅇㅇㅇㅇㅇㅇ");

        PlayerManager.Instance.UpdateAliveStatus();
       

    }
}   