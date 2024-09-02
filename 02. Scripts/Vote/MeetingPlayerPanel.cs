using Photon.Pun;
using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MeetingPlayerPanel : MonoBehaviour
{
    [SerializeField]
    private Image characterImg;

    [SerializeField]
    private TextMeshProUGUI nicknameText;

    [SerializeField]
    private GameObject deadPlayer;

    [SerializeField]
    private GameObject reportsign;

    [SerializeField]
    private GameObject voteButtons;

    

    // 누구의 panel인지 구분하기 위한 변수
    public PhotonView targetPlayer;

    // 이 player가 신고했음을 알림
    [SerializeField]
    private GameObject voteSign;

    // 어떤 player가 누구를 투표할지 알림
    [SerializeField]
    private GameObject voterPrefeb;

    // 누가 투표했는지 voter들 정렬
    [SerializeField]
    private Transform voterParentTransform;

    public void UpdatePanel()
    {
        var voter = Instantiate(voterPrefeb, voterParentTransform).GetComponent<Image>();
        voterParentTransform.gameObject.SetActive(true);
    }

    public void UpdateVoteSign(bool isVoted)
    {
        voteSign.SetActive(isVoted);
    }

    public void SetPlayer(PlayerController target)
    {
        PhotonView targetPhotonView = target.GetComponent<PhotonView>();
        targetPlayer = targetPhotonView;

        nicknameText.text = target.name;

        // 스파이면 스파이 닉네임 색 빨간색으로 보이게
        if (targetPlayer != null )
        {
            nicknameText.color = Color.red;
        }

        // 죽은 사람 panel 건드리지 못하게 만들기


        // 죽은 상태라면 죽은 사람 panel 어둡게 만들기(죽은 상태 표시)
        deadPlayer.SetActive(false);
        reportsign.SetActive(targetPhotonView);
    }

    public void OnClickPlayerPanel()
    {
        // 플레이어가 이미 투표했다면 투표하지 못하게 막아야함
        /*if (targetPlayer.isVote) return;*/

        // 플레이어가 죽었다면 건드리지 못하게 막아야 함
        if (targetPlayer != null) // 예시 코드
        {
           GetComponent<MeetingUI>().SelectPlayerPanel();
            voteButtons.SetActive(true);
        }
    }
    public void Select()
    {
        // 본인 캐릭터 값 가져와서

        // 어떤 player에게 투표했는지 표시

        // 비활성화
        /*UnSelect();*/
    }

    public void Unselect()
    {
        voteButtons.SetActive(false);
    }
}
