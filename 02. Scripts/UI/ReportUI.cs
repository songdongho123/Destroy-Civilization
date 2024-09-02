using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportUI : MonoBehaviour
{
    public GameObject reportImg;

    /*private ReportUI reportUI;*/

    private void Start()
    {
        /*reportUI = reportCanvas.GetComponent<ReportUI>();*/
    }

    public void Open()
    {
        reportImg.SetActive(true);
        /*reportUI.enabled = true;*/
    }
    public void Close()
    {
        reportImg.SetActive(false);
        /*   reportUI.enabled = false;*/
    }
}
