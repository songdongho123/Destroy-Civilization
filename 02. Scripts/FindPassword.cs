using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class FindPassword : MonoBehaviour
{
    public TMP_InputField userIdInputField;
    public TMP_InputField userAnswerInputField;
    public TMP_Text noticeText;


    // API POST 데이터 비밀번호 찾기
    [System.Serializable]
    public class PostDataPassword{
        public string userId;
        public string userAnswer;
    }

    // 회원가입 정보 불러오기 (로컬 로드)
    [System.Serializable]
    public class LocalLoadDataSignup{ 
        public int userCode;
        public string userId;
        public string userName;
        public string userPassword;
        public string userAnswer;
    }



    private void Start() {
        noticeText.enabled = false;
    }

    // 버튼 클릭 시 호출될 함수
    public void PostPassword()
    {
        LocalLoadDataSignup localLoadDataSignup = DataManager.instance.LoadJsonData<LocalLoadDataSignup>("Post Data Signup.json");

        // 회원 가입 함?
        if(localLoadDataSignup != null){
            // 유저 아이디 일치?
            if(localLoadDataSignup.userId == userIdInputField.text){
                // 답변 일치?
                if(localLoadDataSignup.userAnswer == userAnswerInputField.text){
                    print("FindPassword.cs PostPassword : 비밀번호 찾기 성공");
                    noticeText.text = localLoadDataSignup.userPassword;
                    noticeText.enabled = true;
                }
                // 답변 불일치
                else{
                    Debug.LogWarning("답변 틀림");
                    noticeText.enabled = false;
                }
            }
            // 유저 아이디 불일치
            else{
                Debug.LogWarning("아이디 틀림");
                noticeText.enabled = false;
            }
        }
        // 회원 가입 안함
        else if(localLoadDataSignup == null){
            Debug.LogWarning("없는 아이디.");
            Debug.LogError("FindPassword.cs PostPassword : 비밀번호 찾기 실패");
            noticeText.enabled = false;
        }
        
        // API POST 요청 비밀번호
        // PostDataPassword postDataPassword = new PostDataPassword();
        // postDataPassword.userId = userIdInputField.text;
        // postDataPassword.userAnswer = userAnswerInputField.text;
        // StartCoroutine(DataManager.instance.SendApiRequest("http://192.168.31.237:8082/user/password", "POST", postDataPassword, onComplete, onError));
    }

    // API POST 요청 비밀번호 성공
    void onComplete(string response){
        // 찾기 성공 시, 비밀번호 팝업 창에 띄어 줌
        print("FindPassword.cs PostPassword() : " + response);  
    }
    // API POST 요청 비밀번호 실패
    void onError(string error){
        // 찾기 살패 시, 실패 팝업 창 생성
        Debug.LogError("FindPassword.cs PostPassword() : " + error);
    }
}
