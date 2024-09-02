using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Room : MonoBehaviour
{
    // 방 제목 객체
    public TMP_Text roomTitle;

    // 개별 게임 방 버튼
    public void JoinRoom(){
        print("Room.cs JoinRoom");
        GameObject.Find("Looby Manager").GetComponent<LobbySceneManager>().JoinRoomInList(roomTitle.text);
    }
}
