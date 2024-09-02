using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class KeyContoroller : MonoBehaviour
{

    public float raycastDistance = 10f;
   
    public GameObject manager;
    public String tag;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("ScriptsManager");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red);
        //��ȣ �ۿ�
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("e����");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
            {
                // Raycast�� �浹�� GameObject�� �ִ� Tag�� ������
                tag = hit.collider.gameObject.tag;
                Debug.Log(tag);
            }
            if (tag == "bush")
            {
                hit.collider.gameObject.GetComponent<BushScript>().BushTouch();
            }
            else if (tag == "Mission2")
            {
                Debug.Log("�ƴ�");
            }
            else if (tag == "bluemink")
            {
                Debug.Log("��");
                hit.collider.gameObject.GetComponent<BlueMinkScript>().BrokenBlueMink();
            }
            tag = null;
        }
        //��ų 
        else if (Input.GetKeyDown(KeyCode.Q))
        {

        }
        //����
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {

        }
        //��� �� �޴�
        else if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
        //��
        else if (Input.GetKeyDown(KeyCode.Tab))
        {

        }
        

    }
    public String RayShoot() 
    {
         RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            // Raycast�� �浹�� GameObject�� �ִ� Tag�� ������
            tag = hit.collider.gameObject.tag;
            Debug.Log(tag);
        }
        
        return tag; 
    }
}
