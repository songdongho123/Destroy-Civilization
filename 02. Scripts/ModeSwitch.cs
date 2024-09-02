using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSwitch : MonoBehaviour
{
    public GameObject currentCanvas; // ���� ���̴� ������
    public GameObject otherCanvas; // ������ ������
    public GameObject modal;

    // ��ư Ŭ�� �� ȣ��� �޼ҵ�

    void Start()
    {
        currentCanvas.SetActive(true);
        otherCanvas.SetActive(false);
    }
    public void SwitchCanvas()
    {
        currentCanvas.SetActive(false); // ���� �������� ����
        otherCanvas.SetActive(true); // �ٸ� �������� ������
        modal.SetActive(false);
    }

    // 스토어 씬으로 이동
    public void SwitchStroeScene()
    {
        SceneManager.LoadScene("StoreScene");
    }
}
