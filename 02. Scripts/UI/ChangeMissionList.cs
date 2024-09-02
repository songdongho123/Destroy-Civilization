using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChangeMissionList : MonoBehaviour
{
    public QuestManager questManager;
    public TMP_Text tmpText;

    public int viewId;

    void Start()
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!");
        questManager.OnListChanged += UpdateMissionList; // 이벤트 구독
    }

    private void OnDestroy()
    {
        questManager.OnListChanged -= UpdateMissionList; // 이벤트 구독 취소
    }

    private void UpdateMissionList()
    {
        string componentNames = "";

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();

            // PhotonView가 있는지 확인하고, IsMine 속성을 체크하여 로컬 플레이어인지 확인합니다.
            if (photonView != null && photonView.IsMine)
            {
                Debug.Log("로컬 플레이어의 ViewID: " + photonView.ViewID);
                viewId = photonView.ViewID;
                // 필요한 추가 작업을 여기에 수행합니다.
                break; // 로컬 플레이어를 찾았으므로 루프를 종료합니다.
            }
        }

        foreach (var pair in questManager.list)
        {
            if (viewId == pair.Key)
            {
                    Debug.Log("!!!!!! pair키:" + pair.Key);
                    Debug.Log($"키: {pair.Key}, 값: {pair.Value}");
                    componentNames += pair.Value + "\n";
                    tmpText.text = componentNames;
            }

        }
    }
}
