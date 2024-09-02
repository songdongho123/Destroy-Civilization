using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 필요합니다.

public class SaveInputField : MonoBehaviour
{
    public InputField inputField; // 인스펙터에서 할당할 InputField
    public Button confirmButton; // 인스펙터에서 할당할 Button
    public VotePaper votePaper; // 추가: VotePaper 클래스의 인스턴스 변수

    private void Start()
    {
        // 확인 버튼에 클릭 이벤트 리스너 추가
        confirmButton.onClick.AddListener(OnConfirmClicked);
    }

    // 확인 버튼이 클릭되었을 때 호출될 메서드
    private void OnConfirmClicked()
    {
        string inputValue = inputField.text; // InputField로부터 텍스트 값을 가져옵니다.

        // 입력값을 정수로 변환
        if (int.TryParse(inputValue, out int viewId))
        {
            // PlayerManager의 인스턴스를 통해 count 값을 증가
            PlayerManager.Instance.IncrementCount(viewId);
            votePaper.Exit();
        }
        else
        {
            Debug.LogError("입력값이 유효한 숫자가 아닙니다.");
        }
    }
}
