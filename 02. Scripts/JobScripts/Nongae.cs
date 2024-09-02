using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시민-논개 : 죽으면 죽인 동물이 1초 만에 자진 신고함
// 해당 코드는 attack.cs 에 포함됨
public class Nongae : MonoBehaviour
{
    private PlayerController playerController;
    
    private void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }
    
    private void Update()
    {
    }
}