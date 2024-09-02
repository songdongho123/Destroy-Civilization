using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPanelPrefab;

    [SerializeField]
    private Transform playerPanelsParent;

    // 생성된 player panel 저장 list
    private List<MeetingPlayerPanel> meetingPlayerPanels = new List<MeetingPlayerPanel>();

    // 투표 scene에 자기 panel 띄워주기
    public void Open()
    {
        // 내 캐릭터를 어떻게 감지해서 가져오지
        // 자기 캐릭터를 확인해서 panel을 미리 다 만들어줌
        var myPanel = Instantiate(playerPanelPrefab, playerPanelsParent).GetComponent<MeetingPlayerPanel>();
        /*myPanel.SetPlayer();*/
        meetingPlayerPanels.Add(myPanel);

        gameObject.SetActive(true);

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (var player in players)
        {
            // 코드 수정 필요
            // player가 본인의 character인지 구분
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
            if (panel.targetPlayer)  // 예시 코드
            {
                panel.UpdatePanel();
            }

            if (panel.targetPlayer)  // 예시 코드
            {
                panel.UpdateVoteSign(true);
            }
        }
    }
}
