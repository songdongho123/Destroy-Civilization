using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakingMap : MonoBehaviour
{
    [Header("Canvas Object")]
    public GameObject makingModeCanvas;
    public GameObject makingRoomCanvas;
    
    [Header("Select Mode List / Description")]
    public Button[] buttons;
    public TMP_Text description;
    

    // 모드 데이터 객체 선언
    [System.Serializable]
    // 저장할 데이터 SaveData 객체 선언
    class SaveData
    {
        // SaveData 객체 안에 리스트 mapes 필드 선언
        public List<MapData> mapes = new List<MapData>();
    
        [System.Serializable]
        // SaveData 객체 안에 리스트 mapes 필드 안에 MapData 객체 선언
        public class MapData
        {
            // MapData 필드 선언
            public Button id;
            public string name;

            // MapData 객체가 생성 될 때, 필드 할당하여 생성
            public MapData(Button id, string name)
            {
                this.id = id;
                this.name = name;
            }
        }
    }
    
    private void Start() {
        SaveData data = new SaveData();
        
        data.mapes.Add(new SaveData.MapData(buttons[0], "도시에서 일어나는 우당탕탕 문명파괴!"));
        data.mapes.Add(new SaveData.MapData(buttons[1], "(준비 중)"));
        string json = JsonUtility.ToJson(data);
        string filePath = Path.Combine(Application.persistentDataPath, "Game Map Description.json");
    
        // 데이터 저장 (파일 생성)
        File.WriteAllText(filePath, json);
    }
    
    // 뒤로 가기 (Making Mode Canvas 이동)
    public void BackToMakingMode(){
        makingModeCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    // 방 생성 하기 (Making Room Canvas 이동)
    public void ClickMakingRoom(){
        makingRoomCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    // 모드 버튼 클릭 시, 해당 설명 출력
    public void ClickSelectMap(Button selectItem){
        // 로컬 Game Map Description.json  파일 로드
		string filePath = Path.Combine(Application.persistentDataPath, "Game Map Description.json");
				
		// 경로 상에 파일이 있다면,
        if(File.Exists(filePath))
        {
		    // JSON 파일을 읽어오고,
            string json = File.ReadAllText(filePath);
            // JSON 데이터를 유니티 데이터로 변환 (JSON 데이터 역직렬화)
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            // 데이터를 하나 씩 살펴보면서
            for(int i = 0; i < data.mapes.Count; i++ )
            {
                // 클릭한 버튼에 해당하는 데이터 매핑
                if(data.mapes[i].id == selectItem)
                {
                    // 해당 맵 데이터의 설명 출력
                    Debug.Log(data.mapes[i].name);
                    description.text = data.mapes[i].name;

                    // 선택한 모드 이미지를 lobbySceneMananger.mapImage 에 할당
                    Debug.Log(selectItem.image.sprite);
                    ServerData.Instance.mapImage = selectItem.image.sprite;
                }
            }
        }
        // 파일이 없을 때.
        else
        {
            Debug.Log("nnnnnnnnnnnoooo file");
        }
    }
}
