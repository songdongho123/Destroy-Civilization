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
            jobModal.SetActive(false); // 처음에는 모달을 꺼진 상태로 설정
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Y))
        {
            // jobModal이 null이 아닐 경우에만 실행
            if (jobModal != null)
            {
                // jobModal의 현재 활성화 상태를 반전시킴 (켜져있으면 끄고, 꺼져있으면 켬)
                jobModal.SetActive(!jobModal.activeSelf);
            }
        }
    }
}
