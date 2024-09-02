using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TireScript : MonoBehaviour
{
    public GameObject tire;
    public static int tireNum;
    private String tireTag;
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
                tireTag = hit.collider.gameObject.tag;
                Debug.Log(tireTag);
                if (tireTag == "Tire")
                {
                    hit.collider.gameObject.SetActive(false);
                    tireNum++;
                    if (tireNum == 4)
                    {
                        Debug.Log("타이어");
                        tireNum = 0;
                    }
                }
            }

        }
    }
        public void GetTire()
    {
        tire.SetActive(false);
        tireNum++;
        if (tireNum == 4)
        {
            Debug.Log("타이어");
            tireNum = 0;
        }
    }
}
