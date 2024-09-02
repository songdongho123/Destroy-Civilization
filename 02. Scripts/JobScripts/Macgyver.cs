using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Macgyver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //미션 스코어 2배
        QuestManager.missionScore = QuestManager.missionScore* 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
