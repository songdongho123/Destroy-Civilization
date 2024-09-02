using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Profile : MonoBehaviour
{
    ServerData gameObject;
    public TMP_Text nickName;

    // Start is called before the first frame update
    void Start()
    {
        nickName.text = gameObject.GetComponent<ServerData>().userName;
    }
}
