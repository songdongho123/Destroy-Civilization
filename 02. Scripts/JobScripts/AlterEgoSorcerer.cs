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
        //q������ ��ǥ���°� �ƴ϶�� �̵�
        if (Input.GetKeyDown(KeyCode.Q) && !Report.voting)
        {
            //1���� ����� �� �ְ� ����
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
    //ó�� ��ų���� �н� ��ġ ����
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
    //�н� ��ġ�� �̵�
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
