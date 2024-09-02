using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlueMinkScript : MonoBehaviour
{
    public static int blueMinkStack;
    public GameObject blueMink;
    public  GameObject blueMinkBroken;
    private String blueMinkTag;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("e눌림");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
            {
                // Raycast가 충돌한 GameObject에 있는 Tag를 가져옴
                blueMinkTag = hit.collider.gameObject.tag;
                Debug.Log(blueMinkTag);
                if (blueMinkTag == "bluemink")
                {
                    hit.collider.gameObject.SetActive(false);
                }
            }
            
        }
    }
    public void BrokenBlueMink()
    {
       //꽃 망치기
        blueMink.SetActive(false);
        blueMinkBroken.SetActive(true);
        blueMinkStack++;
        //게임 끝
        if (blueMinkStack > 30)
        {
            blueMinkStack = 0;
            Debug.Log("끝");
        }
    } 
}
