using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    public PlayerManager.PlayerStatusInfo state;
    private string trapRayTag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            // Raycast가 충돌한 GameObject에 있는 Tag를 가져옴
            trapRayTag = hit.collider.gameObject.tag;
            if (trapRayTag == "Player")
            {
               state.isAlive = false;
            }
        }
    }
}
