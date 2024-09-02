using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPanelPrefab;

    [SerializeField]
    private Transform playerPanelsParent;

    // ������ player panel ���� list
    private List<MeetingPlayerPanel> meetingPlayerPanels = new List<MeetingPlayerPanel>();

    // ��ǥ scene�� �ڱ� panel ����ֱ�
    public void Open()
    {
        // �� ĳ���͸� ��� �����ؼ� ��������
        // �ڱ� ĳ���͸� Ȯ���ؼ� panel�� �̸� �� �������
        var myPanel = Instantiate(playerPanelPrefab, playerPanelsParent).GetComponent<MeetingPlayerPanel>();
        /*myPanel.SetPlayer();*/
        meetingPlayerPanels.Add(myPanel);

        gameObject.SetActive(true);

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (var player in players)
        {
            // �ڵ� ���� �ʿ�
            // player�� ������ character���� ����
            if (player)
            {
                var panel = Instantiate(playerPanelPrefab,playerPanelsParent).GetComponent<MeetingPlayerPanel>();
                panel.SetPlayer(player);
                meetingPlayerPanels.Add(panel);
            }
        }
    }

    public void SelectPlayerPanel()
    {
        foreach (var panel in meetingPlayerPanels)
        {
            panel.Unselect();
        }
    }
    public void UpdateVote()
    {
        foreach (var panel in meetingPlayerPanels)
        {
            if (panel.targetPlayer)  // ���� �ڵ�
            {
                panel.UpdatePanel();
            }

            if (panel.targetPlayer)  // ���� �ڵ�
            {
                panel.UpdateVoteSign(true);
            }
        }
    }
}
