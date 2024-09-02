using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ���� �۾��� ���� �ʿ�
using Photon.Pun;
using TMPro;

public class ChangeUiGraphics : MonoBehaviour
{
    public JobManager jobManager; // JobManager ��ũ��Ʈ�� �ν��Ͻ� ����
    public Image skillUIGraphics; // �����ϰ��� �ϴ� �̹��� ������Ʈ

    // �� ���������� �׽�Ʈ�� ���� viewId ������ 1�� ����մϴ�.
    // ���� ��� �ÿ��� �������� ���ϴ� viewId ���� ����ؾ� �մϴ�.
    public int viewId;
    
    public void GetUI(string jobData)
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
        //     // jobData�� ���� ������ �̹����� �ε��մϴ�.
            switch (jobData)
            {
                case "Detective":
                case "Bomber":
                case "HackerSkill":
                case "Knight":
                case "Nongae":
                case "Sheriff":
                case "Star":
                case "Vigilante":
                case "Pelican":
                case "SignalMan":
                case "Macgyver":
                case "Trapper":
                case "AlterEgoSorcerer":
                case "Falcon":
                Sprite newSprite = Resources.Load<Sprite>("job/" + jobData);
                    skillUIGraphics.sprite = newSprite;
                    break;
            }
        // }
        // else
        // {
        //     Debug.LogError("�ش� viewId�� ���� ���� �����͸� ã�� �� �����ϴ�: " + viewId);
        // }
    }
}
