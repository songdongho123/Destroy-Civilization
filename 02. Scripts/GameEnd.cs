using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.Text.RegularExpressions;

public class GameEnd : MonoBehaviourPunCallbacks
{
   
      
      void Update()
      {
        Debug.Log(PlayerManager.Instance.spy_num );
        Debug.Log(PlayerManager.Instance.neutrality_num );
        Debug.Log(PlayerManager.Instance.wild_num );
        if (InGameLobbySceneManager.rootCount >100)
        {
            if (PlayerManager.Instance.spy_num == 0 && PlayerManager.Instance.neutrality_num == 0)
            {
                photonView.RPC("LoadEndScene", RpcTarget.All, "GameEndScene_Animal");
            }
            // wild_num 과 neutrality_num 이 0이면 GameEndScene_Spy 씬으로 전환
            else if (PlayerManager.Instance.wild_num == 0 && PlayerManager.Instance.neutrality_num == 0)
            {
                photonView.RPC("LoadEndScene", RpcTarget.All, "GameEndScene_Spy");
            }
            // wild_num 과 spy_num 이 0이면 GameEndScene_Animal 씬으로 전환
            else if (PlayerManager.Instance.wild_num == 0 && PlayerManager.Instance.spy_num == 0)
            {
                photonView.RPC("LoadEndScene", RpcTarget.All, "GameEndScene_Animal");
            }
        }
      }

      [PunRPC]
        public void LoadEndScene(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
}
