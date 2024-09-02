using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    public TMP_Text myMessage;

    private void Start() {
        GetComponent<RectTransform>().SetAsFirstSibling();
    }
}
