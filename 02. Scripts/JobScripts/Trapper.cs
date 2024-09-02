using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapper : MonoBehaviourPunCallbacks
{
    public static int trapperSkillCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q)&&!Report.voting)
        {
            Debug.Log("미션 점수" + QuestManager.missionScore);
            if (trapperSkillCount != 1)
            {
                if (GetComponent<PhotonView>().IsMine)
                {
                    photonView.RPC("TrapperSkill", RpcTarget.All, transform.position);
                }
            }
        }
    }
    [PunRPC]
    public void TrapperSkill(Vector3 trapPosiition)
    {
        
        GameObject skillObject = GameObject.FindGameObjectWithTag("SkillObject");
        Transform trapTransform = skillObject.transform.Find("TrapObject");
        GameObject trap = trapTransform.gameObject;
        trap.SetActive(true);

        trap.transform.position = new Vector3(trapPosiition.x, 0, trapPosiition.z);
        trapperSkillCount = 1;
        Debug.Log("trap");
    }
}
