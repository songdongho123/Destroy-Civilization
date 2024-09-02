using UnityEngine;
using UnityEngine.UI;

public class MissionProgressBar : MonoBehaviour
{
    public int progress; // 인스펙터에서 설정할 진행도
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