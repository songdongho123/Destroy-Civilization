using UnityEngine;
using UnityEngine.UI;

public class TogglePopUp : MonoBehaviour
{
    public GameObject PopUp; // 활성화/비활성화할 오브젝트를 여기에 연결하세요.

    // 버튼 클릭 시 호출될 함수
    public void ToggleObject()
    {
        // objectToToggle이 비활성화된 경우 활성화하고, 활성화된 경우 비활성화합니다.
        PopUp.SetActive(!PopUp.activeSelf);
    }
}