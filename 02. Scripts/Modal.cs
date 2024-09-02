using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modal : MonoBehaviour
{
    public GameObject modalPanel;

    // Start is called before the first frame update
    void Start()
    {
        modalPanel.SetActive(false);
    }

    // Update is called once per frame
    public void ModalToTrue()
    {
        modalPanel.SetActive(true);
    }

    public void ModalToFalse()
    {
        modalPanel.SetActive(false);
    }
}
