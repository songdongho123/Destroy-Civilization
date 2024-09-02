using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance; // 싱글톤 패턴

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    // 일반화된 API 요청 메서드
    public IEnumerator SendApiRequest(string url, string method, object requestData, System.Action<string> onComplete, System.Action<string> onError)
    {
        UnityWebRequest www;

        // GET 요청
        if (method == "GET")
        {
            www = UnityWebRequest.Get(url);
        }

        // POST 요청
        else if (method == "POST")
        {   
            string jsonData = JsonUtility.ToJson(requestData);
            www = UnityWebRequest.Post(url, jsonData, "application/json");
            www.SetRequestHeader("Content-Type", "application/json");
        }
        // PUT 요청
        else if (method == "PUT")
        {
            string jsonData = JsonUtility.ToJson(requestData);
            www = UnityWebRequest.Put(url, jsonData);
            www.SetRequestHeader("Content-Type", "application/json");
        }
        // DELETE 요청
        else if (method == "DELETE")
        {
            www = UnityWebRequest.Delete(url);
        }

        // 나머지 요청 예외 처리;
        else
        {
            onError("Unsupported HTTP method");
            yield break;
        }

        // 요청 보내기
        yield return www.SendWebRequest();

        // 요청 완료 시
        if (www.isDone)
        {
            // 에러가 없을 경우
            if (www.error == null)
            {
                onComplete(www.downloadHandler.text); // 응답 본문 전달
            }
            // 에러가 있을 경우
            else
            {
                onError("Request failed: " + www.error);
            }
        }
    }


    // JSON 데이터를 파일로 저장하는 메서드
    public void SaveJsonData(object data, string fileName)
    {   
        // 직렬화
        string jsonData = JsonUtility.ToJson(data);
        
        // 저장 경로
        string path = Path.Combine(Application.persistentDataPath, fileName);

        // 저장
        File.WriteAllText(path, jsonData);
        Debug.Log("Data saved to :  " + path);
    }

    // Json 데이터를 읽어 올 때, 매서드
    public T LoadJsonData<T>(string fileName)
    {
        // 저장 경로
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if(File.Exists(path))
        {
            // 읽기
            string jsonData = File.ReadAllText(path);

            // 역직렬화
            return JsonUtility.FromJson<T>(jsonData);
        }
        else
        {
            Debug.LogError("File not Found : " + path);
            // value tyoe이면 0 , reference type 이면 null
            return default(T);
        }
    }
}
