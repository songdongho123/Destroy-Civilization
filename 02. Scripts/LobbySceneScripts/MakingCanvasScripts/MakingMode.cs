using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakingMode : MonoBehaviour
{
    [Header("Canvas Object")]
    public GameObject lobbyRoomCanvas;
    public GameObject makingMapCanvas;
    
    [Header("Select Mode List / Description")]
    public Button[] buttons;
    public TMP_Text description;
    

    // 모드 데이터 객체 선언
    [System.Serializable]
    // 저장할 데이터 SaveData 객체 선언
    class SaveData
    {
        // SaveData 객체 안에 리스트 modes 필드 선언
        public List<ModeData> modes = new List<ModeData>();
    
        [System.Serializable]
        // SaveData 객체 안에 리스트 modes 필드 안에 ModeData 객체 선언
        public class ModeData
        {
            // ModeData 필드 선언
            public Button id;
            public string name;

            // ModeData 객체가 생성 될 때, 필드 할당하여 생성
            public ModeData(Button id, string name)
            {
                this.id = id;
                this.name = name;
            }
        }
    }

    
    private void Start() {
        SaveData data = new SaveData();
        
        data.modes.Add(new SaveData.ModeData(buttons[0], "기본 모드로, 야생 동물팀과 스파이 동물팀의 대결 모드!"));
        data.modes.Add(new SaveData.ModeData(buttons[1], "(준비 중)"));
        string json = JsonUtility.ToJson(data);
        string filePath = Path.Combine(Application.persistentDataPath, "Game Mode Description.json");
    
        // 데이터 저장 (파일 생성)
        File.WriteAllText(filePath, json);
    }

    // 뒤로 가기 (Lobby Room Canvas 이동)
    public void BackToLobbyRoom(){
        lobbyRoomCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    // 맵 선택 하기 (Making Map Canvas 이동)
    public void ClickMakingMap(){
        makingMapCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    // 모드 버튼 클릭 시, 해당 설명 출력
    public void ClickSelectMode(Button selectItem){
        // 로컬 Game Mode Description.json  파일 로드
		string filePath = Path.Combine(Application.persistentDataPath, "Game Mode Description.json");
				
		// 경로 상에 파일이 있다면,
        if(File.Exists(filePath))
        {
		    // JSON 파일을 읽어오고,
            string json = File.ReadAllText(filePath);
            // JSON 데이터를 유니티 데이터로 변환 (JSON 데이터 역직렬화)
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // 데이터를 하나 씩 살펴보면서
            for(int i = 0; i < data.modes.Count; i++ )
            {
                // 클릭한 버튼에 해당하는 데이터 매핑
                if(data.modes[i].id == selectItem)
                {
                    // 해당 모드 데이터의 설명 출력
                    Debug.Log(data.modes[i].name);
                    description.text = data.modes[i].name;

                    // 선택한 모드 이미지를 lobbySceneMananger.modeImage 에 할당
                    Debug.Log(selectItem.image.sprite);
                    ServerData.Instance.modeImage = selectItem.image.sprite;
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
