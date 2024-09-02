using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadbody : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        // player가 본인인지, player가 유령이 아닌 상태인지 확인해서 Button 활성화
    }

    private void OnTriggerExit(Collider collision)
    {
        // player가 본인인지, player가 유령이 아닌 상태인지 확인해서 Button 비활성화

    }
}
