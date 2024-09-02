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
        //상호 작용
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("e눌림");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
            {
                // Raycast가 충돌한 GameObject에 있는 Tag를 가져옴
                tag = hit.collider.gameObject.tag;
                Debug.Log(tag);
            }
            if (tag == "bush")
            {
                hit.collider.gameObject.GetComponent<BushScript>().BushTouch();
            }
            else if (tag == "Mission2")
            {
                Debug.Log("아니");
            }
            else if (tag == "bluemink")
            {
                Debug.Log("꽃");
                hit.collider.gameObject.GetComponent<BlueMinkScript>().BrokenBlueMink();
            }
            tag = null;
        }
        //스킬 
        else if (Input.GetKeyDown(KeyCode.Q))
        {

        }
        //공격
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {

        }
        //취소 및 메뉴
        else if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
        //맵
        else if (Input.GetKeyDown(KeyCode.Tab))
        {

        }
        

    }
    public String RayShoot() 
    {
         RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            // Raycast가 충돌한 GameObject에 있는 Tag를 가져옴
            tag = hit.collider.gameObject.tag;
            Debug.Log(tag);
        }
        
        return tag; 
    }
}
