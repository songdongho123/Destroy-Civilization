using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemes : MonoBehaviour
{
    // API POST 데이터 캐릭터 해금
    [Serializable]
    public class PostDataCharacterOpen{
        public int userCode;
        public int characterCode;
    }



    // 모자 아이템 클릭 시,
    public void ClickHeadItem(){

    }

    // (테스트) 캐릭터 아이템 클릭 시,
    public void ClickShritsItem(int characterCode){

        // API POST 요청 캐릭터 해금 (1번 캐릭터 해금)
        PostDataCharacterOpen postDataCharacterOpen = new PostDataCharacterOpen();
        postDataCharacterOpen.userCode = ServerData.Instance.userCode;
        postDataCharacterOpen.characterCode = characterCode;

        StartCoroutine(DataManager.instance.SendApiRequest("https://j10b310.p.ssafy.io", "POST", postDataCharacterOpen, onCompleteCharacterOpen, onErrorCharacterOpen));
    }
    // API POST 요청 캐릭터 해금 성공
    void onCompleteCharacterOpen(string response){
        print("Items.cs ClickShritsItem() : " + response);
    }
    // API POST 요청 캐릭터 해금 실패
    void onErrorCharacterOpen(string error){
        Debug.LogError("Items.cs ClickShritsItem() : " + error);
    }
}
