using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushScript : MonoBehaviour
{
    public static int stackBush;
    public GameObject bigBush;
    public GameObject smallBush;
    private string bushTag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //����ĳ��Ʈ�� ã�� 
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
            {
                // Raycast�� �浹�� GameObject�� �ִ� Tag�� ������
                bushTag = hit.collider.gameObject.tag;
                Debug.Log(bushTag);
                if (bushTag == "bush")
                {
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
    }
    public void BushTouch()
    {
        //3�������� �����
        stackBush++;
        Debug.Log(stackBush);
        if (stackBush > 3)
        {
            bigBush.SetActive(false);
            smallBush.SetActive(true);
            stackBush = 0;
            /*Invoke(ToString(),20f);*/
        }
        
    }
}
