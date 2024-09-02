using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Report : MonoBehaviourPunCallbacks
{
    public static bool voting;
    

    private Camera characterCamera;         // ĳ���Ϳ� ���� ī�޶�
    private GameObject reportDeactive;      // �Ű� ��Ȱ��ȭ ��ư GameObject
    private GameObject ReportCanvas;

    public GameManager gameManager;
   
    private float reportDistance;
    void Start()
    {

        // ĳ���Ϳ� ���� ī�޶� ã��
        characterCamera = GetComponentInChildren<Camera>();

        // ReportDeactive GameObject �ʱ�ȭ
        GameObject gameUI = GameObject.FindGameObjectWithTag("GameUI");
        
        Transform reportBtn = gameUI.transform.Find("ReportBtn");
        reportDeactive = reportBtn.Find("ReportDeactive").gameObject;
        
    }


    // �Ű� ���
    public void CallReport()
    {
        CmdReport();
    }

    public void Update()
    {
        ActivateReport();
    }

    // �Ű� Ȱ��ȭ�ϴ� �Լ�
    public void ActivateReport()
    {
        reportDistance = 20f;

        RaycastHit hit;

        // ī�޶󿡼� ���콺 ��ġ�� ���̸� ���
        Debug.DrawRay(transform.position, transform.forward * reportDistance, Color.yellow);

        // Raycast�� �߻��Ͽ� �浹�� ���
        if (Physics.Raycast(transform.position, transform.forward, out hit, reportDistance))
        {
            // �浹�� ������Ʈ�� �±װ� "DeadBody"�� ��쿡�� ó��
            if (hit.collider.CompareTag("DeadBody"))
            {
                // �Ű� ��� ����
                reportDeactive.gameObject.SetActive(false);
                return;
            }
        }
        // �浹���� ���� ��� �Ǵ� �ٸ� �±��� ������Ʈ�� �浹�� ��쿡�� �Ű� ��ư�� Ȱ��ȭ
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

    // �Ű� ���
    private IEnumerator StartMeeting_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<ReportUI>().Close();

        GetComponent<MeetingUI>().Open();


        
        
    }
    //3�ʸ� ���� ����
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
        Debug.Log("�����");
        for (int i = count; i > 0; i--)
        {
            Debug.Log("Countdown: " + i);
            yield return new WaitForSeconds(1f); // 1�� ���
        }
        reporting.SetActive(false);

            GameObject alivePlayer = GameObject.FindGameObjectWithTag("Player");
            alivePlayer.transform.position = new Vector3(-253, 161, -207);
        
        Debug.Log("����������");
        yield return new WaitForSeconds(10f);
        reporting2.SetActive(false);

        Debug.Log("���Ԥ�����������������");

        PlayerManager.Instance.UpdateAliveStatus();
       

    }
}   