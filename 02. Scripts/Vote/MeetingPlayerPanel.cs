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

    

    // ������ panel���� �����ϱ� ���� ����
    public PhotonView targetPlayer;

    // �� player�� �Ű������� �˸�
    [SerializeField]
    private GameObject voteSign;

    // � player�� ������ ��ǥ���� �˸�
    [SerializeField]
    private GameObject voterPrefeb;

    // ���� ��ǥ�ߴ��� voter�� ����
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

        // �����̸� ������ �г��� �� ���������� ���̰�
        if (targetPlayer != null )
        {
            nicknameText.color = Color.red;
        }

        // ���� ��� panel �ǵ帮�� ���ϰ� �����


        // ���� ���¶�� ���� ��� panel ��Ӱ� �����(���� ���� ǥ��)
        deadPlayer.SetActive(false);
        reportsign.SetActive(targetPhotonView);
    }

    public void OnClickPlayerPanel()
    {
        // �÷��̾ �̹� ��ǥ�ߴٸ� ��ǥ���� ���ϰ� ���ƾ���
        /*if (targetPlayer.isVote) return;*/

        // �÷��̾ �׾��ٸ� �ǵ帮�� ���ϰ� ���ƾ� ��
        if (targetPlayer != null) // ���� �ڵ�
        {
           GetComponent<MeetingUI>().SelectPlayerPanel();
            voteButtons.SetActive(true);
        }
    }
    public void Select()
    {
        // ���� ĳ���� �� �����ͼ�

        // � player���� ��ǥ�ߴ��� ǥ��

        // ��Ȱ��ȭ
        /*UnSelect();*/
    }

    public void Unselect()
    {
        voteButtons.SetActive(false);
    }
}
