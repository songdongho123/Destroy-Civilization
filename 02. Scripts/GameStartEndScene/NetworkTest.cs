using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(UnityWebRequestPOSTTEST());
    }

    IEnumerator UnityWebRequestPOSTTEST()
    {
        string url = "http://127.0.0.1:8000/communities/";

        UnityWebRequest www = UnityWebRequest.Get(url); // GET 요청
        yield return www.SendWebRequest(); // 응답 대기

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(www.downloadHandler.text); // 데이터 출력
            // DataManager를 사용하여 데이터를 파일로 저장
            DataManager.instance.SaveJsonData(www.downloadHandler.text, "communities.json");
        }
        else
        {
            Debug.Log("오류 발생: " + www.error);
            // 응답 본문 출력
            Debug.Log("응답 본문: " + www.downloadHandler.text);
        }
    }
}
