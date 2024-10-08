using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Report : MonoBehaviourPunCallbacks
{
    public static bool voting;
    

    private Camera characterCamera;         // 蝶遣斗拭 細精 朝五虞
    private GameObject reportDeactive;      // 重壱 搾醗失鉢 獄動 GameObject
    private GameObject ReportCanvas;

    public GameManager gameManager;
   
    private float reportDistance;
    void Start()
    {

        // 蝶遣斗拭 細精 朝五虞 達奄
        characterCamera = GetComponentInChildren<Camera>();

        // ReportDeactive GameObject 段奄鉢
        GameObject gameUI = GameObject.FindGameObjectWithTag("GameUI");
        
        Transform reportBtn = gameUI.transform.Find("ReportBtn");
        reportDeactive = reportBtn.Find("ReportDeactive").gameObject;
        
    }


    // 重壱 奄管
    public void CallReport()
    {
        CmdReport();
    }

    public void Update()
    {
        ActivateReport();
    }

    // 重壱 醗失鉢馬澗 敗呪
    public void ActivateReport()
    {
        reportDistance = 20f;

        RaycastHit hit;

        // 朝五虞拭辞 原酔什 是帖稽 傾戚研 庶奄
        Debug.DrawRay(transform.position, transform.forward * reportDistance, Color.yellow);

        // Raycast研 降紫馬食 中宜廃 井酔
        if (Physics.Raycast(transform.position, transform.forward, out hit, reportDistance))
        {
            // 中宜廃 神崎詮闘税 殿益亜 "DeadBody"昔 井酔拭幻 坦軒
            if (hit.collider.CompareTag("DeadBody"))
            {
                // 重壱 奄管 叔楳
                reportDeactive.gameObject.SetActive(false);
                return;
            }
        }
        // 中宜馬走 省精 井酔 暁澗 陥献 殿益税 神崎詮闘拭 中宜廃 井酔拭亀 重壱 獄動聖 醗失鉢
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

    // 重壱 奄管
    private IEnumerator StartMeeting_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<ReportUI>().Close();

        GetComponent<MeetingUI>().Open();


        
        
    }
    //3段研 室澗 欠覗
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
        Debug.Log("食奄澗");
        for (int i = count; i > 0; i--)
        {
            Debug.Log("Countdown: " + i);
            yield return new WaitForSeconds(1f); // 1段 企奄
        }
        reporting.SetActive(false);

            GameObject alivePlayer = GameObject.FindGameObjectWithTag("Player");
            alivePlayer.transform.position = new Vector3(-253, 161, -207);
        
        Debug.Log("ししせせせ");
        yield return new WaitForSeconds(10f);
        reporting2.SetActive(false);

        Debug.Log("遭脊ししししししししし");

        PlayerManager.Instance.UpdateAliveStatus();
       

    }
}   