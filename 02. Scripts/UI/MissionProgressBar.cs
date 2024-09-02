using UnityEngine;
using UnityEngine.UI;

public class MissionProgressBar : MonoBehaviour
{
    public int progress; // �ν����Ϳ��� ������ ���൵
    public Slider Slider;

    void Update()
    {
        if (Slider.value <= 0) {
            transform.Find("Fill Area").gameObject.SetActive(false);
        } 
        else
        {
            transform.Find("Fill Area").gameObject.SetActive(true);
        }
        Slider.value = progress / 100f;
    }
}