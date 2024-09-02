using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetJobModal : MonoBehaviour
{
    public GameObject jobModal;
    // Start is called before the first frame update
    void Start()
    {
        if (jobModal != null)
        {
            jobModal.SetActive(false); // ó������ ����� ���� ���·� ����
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Y))
        {
            // jobModal�� null�� �ƴ� ��쿡�� ����
            if (jobModal != null)
            {
                // jobModal�� ���� Ȱ��ȭ ���¸� ������Ŵ (���������� ����, ���������� ��)
                jobModal.SetActive(!jobModal.activeSelf);
            }
        }
    }
}
