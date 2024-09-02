using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeUiJobName : MonoBehaviour
{
    public JobManager jobManager; // JobManager ��ũ��Ʈ�� �ν��Ͻ� ����
    public TMP_Text jobName;

    // �� ���������� �׽�Ʈ�� ���� viewId ������ 1�� ����մϴ�.
    // ���� ��� �ÿ��� �������� ���ϴ� viewId ���� ����ؾ� �մϴ�.
    public int viewId;

    public void GetName(string jobData)
    {
        // // ��� PhotonView ������Ʈ�� ã���ϴ�.
        // PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        // foreach (var pv in photonViews)
        // {
        //     if (pv.IsMine) // ���� �÷��̾��� PhotonView���� Ȯ��
        //     {
        //         viewId = pv.ViewID; // ���� �÷��̾��� ViewID�� ����
        //         break; // ã������ ���� ����
        //     }
        // }

        // Debug.Log("viewId" + viewId);
        // // jobManager.userJobInfo���� ���� viewId�� �ش��ϴ� ���� �����͸� �����ɴϴ�.
        // if (jobManager.userJobInfo.TryGetValue(viewId, out string jobData))
        // {
        //     Debug.Log("�ⵥ����" + jobData);

            switch (jobData)
            {
                case "Detective": jobName.text = "탐정"; break;
                case "Bomber": jobName.text = "폭탄광"; break;
                case "HackerSkill": jobName.text = "해커"; break;
                case "Knight": jobName.text = "기사"; break;
                case "Nongae": jobName.text = "논개"; break;
                case "Sheriff": jobName.text = "보안관"; break;
                case "Star": jobName.text = "연예인"; break;
                case "Vigilante": jobName.text = "자경단"; break;
                case "Pelican": jobName.text = "대식가"; break;
                case "SignalMan": jobName.text = "통신병"; break;
                case "Macgyver": jobName.text = "맥가이버"; break;
                case "Trapper": jobName.text = "트래퍼"; break;
                case "AlterEgoSorcerer": jobName.text = "분신술사"; break;
                case "Falcon": jobName.text = "송골매"; break;
            }
        // }
        // else
        // {
        //     Debug.LogError("�ش� viewId�� ���� ���� �����͸� ã�� �� �����ϴ�: " + viewId);
        // }
    }
}
