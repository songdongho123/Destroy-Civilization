using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StreetLampScript : MonoBehaviour
{
    public GameObject streetLamp;
    private String streetLampTag;
    public int streetLampCount;
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
                streetLampTag = hit.collider.gameObject.tag;
                Debug.Log(streetLampTag);
                if (streetLampTag == "StreetLamp")
                {
                    hit.collider.gameObject.transform.Rotate(Vector3.forward, 30f);
                    streetLampCount++;
                    //여기가 끝
                    if (streetLampCount > 3)
                    {
                        streetLampCount = 0;
                    }
                }
            }

        }

    }
    public void BrokenStreetLamp()
    {
        Debug.Log("휜다");
        transform.Rotate(Vector3.forward, 30f);
        streetLampCount++;
        //여기가 끝
        if (streetLampCount > 3)
        {
            streetLampCount = 0;
        }
    }
}
