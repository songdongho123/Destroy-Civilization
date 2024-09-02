using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class Community
{
    public int id;
    public string title;
    public string content;
    public string created_at;
    public string updated_at;
    public int user;
}

public class JsonDataManager : MonoBehaviour
{
    public Transform[] spawnPositions; // Spawn 할 위치들의 배열
    public Text jobText;

    public Text[] nameText;
    private void Start()
    {   
        jobText = GetComponent<Text>();
        LoadJsonData();
    }

    void LoadJsonData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "communities.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            Community[] communities = JsonHelper.FromJson<Community>(dataAsJson);
            SpawnCharacters(communities);
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

    void SpawnCharacters(Community[] communities)
    {
        for (int i = 0; i < communities.Length; i++)
        {
            if (i < spawnPositions.Length)
            {
                // 동물 이름에 따라 Prefab 경로 설정
                string prefabPath = communities[i].content + "idle"; // 예: "bearIdle"
                GameObject characterPrefab = Resources.Load<GameObject>(prefabPath);
                if (characterPrefab != null)
                {
                    // 인스턴스화하고, 설정된 위치에 배치
                    GameObject characterInstance = Instantiate(characterPrefab, spawnPositions[i]);

                    // 해당 위치에 Text를 설정합니다.
                    if (i < nameText.Length)
                    {
                        nameText[i].text = communities[i].title; // JSON에서 가져온 제목으로 설정
                    }
                    else
                    {
                        Debug.LogWarning("Not enough Text objects for the number of communities.");
                    }
                }
                else
                {
                    Debug.LogError("Prefab not found for content: " + communities[i].content);
                }
            }
            else
            {
                Debug.LogError("Not enough spawn positions for the number of communities.");
                break;
            }
        }
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"array\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}
