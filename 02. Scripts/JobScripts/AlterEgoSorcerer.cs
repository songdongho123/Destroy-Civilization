using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterEgoSorcerer : MonoBehaviour
{
    public static int AlterEgoSkillCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //q누르면 투표상태가 아니라면 이동
        if (Input.GetKeyDown(KeyCode.Q) && !Report.voting)
        {
            //1번만 사용할 수 있게 설정
            if (AlterEgoSkillCount != 1 && AlterEgoSkillCount != 2)
            {
                AlterEgoSorcererSkill();
            }
            else if (AlterEgoSkillCount == 1)
            {
                AlterEgoChange();
            }
        }
    }
    //처음 스킬쓰면 분신 위치 설정
    public void AlterEgoSorcererSkill()
    {

        GameObject skillObject = GameObject.FindGameObjectWithTag("SkillObject");
        Transform trapTransform = skillObject.transform.Find("AlterEgoObject");
        GameObject alter = trapTransform.gameObject;
        
            alter.SetActive(true);
            alter.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Debug.Log("alter");
        AlterEgoSkillCount = 1;


    }
    //분신 위치로 이동
    public void AlterEgoChange()
    {
        GameObject skillObject = GameObject.FindGameObjectWithTag("SkillObject");
        Transform trapTransform = skillObject.transform.Find("AlterEgoObject");
        GameObject alter = trapTransform.gameObject;

        alter.SetActive(true);
        transform.position = new Vector3(alter.transform.position.x, alter.transform.position.y, alter.transform.position.z);
        AlterEgoSkillCount = 2;
    }
}
