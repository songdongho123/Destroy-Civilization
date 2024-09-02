using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public TMP_InputField messageInput;
    public GameObject message;
    public GameObject content;

    // 메세지 보내는 함수
    public void SendMessage(){
        // 모든 플레이어들의 GetMessage 함수를 메세지 내용을 담아서 RPC 통신을 보낸다.
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, (PhotonNetwork.NickName + " : " + messageInput.text));
        messageInput.text = "";
    }

    // RPC 통신을 받아서 모든 플레이어들은 GetMessage 함수를 실행 시킨다.
    [PunRPC]
    public void GetMessage(string rceiveMessage){
        GameObject msg = Instantiate(message, Vector3.zero, Quaternion.identity, content.transform);
        msg.GetComponent<Message>().myMessage.text = rceiveMessage;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return)){
            messageInput.ActivateInputField();
            messageInput.Select();

            if(messageInput.text != null && Input.GetKeyDown(KeyCode.Return)){
                SendMessage();
            }
        }
        // if(Input.GetKeyDown(KeyCode.Return)){
        //     SendMessage();
        // }
    }
}

