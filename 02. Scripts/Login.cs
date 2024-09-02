using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
// using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public TMP_InputField userIdInputField;
    public TMP_InputField userPasswordInputField;
    public TMP_Text noticeText;

    // 회원가입 정보 불러오기 (로컬 로드)
    [System.Serializable]
    public class LocalLoadDataSignup{ 
        public int userCode;
        public string userId;
        public string userName;
        public string userPassword;
        public string userAnswer;
    }

    // API POST 데이터 로그인
    [System.Serializable]
    public class PostDataLogin{
        public string userId;
        public string userPssword;
    }

    // API POST 데이터 로그인 (응답 값)
    [System.Serializable]
    public class ResponsePostDataLogin{
        public int userCode;
        public string userName;
    }


    // API POST 데이터 유저 정보 리스트 조회
    [System.Serializable]
    public class PostDataUserInfoList{
        public int userCode;
    }

    // API POST 데이터 유저 정보 리스트 조회 (응답 값)
    [System.Serializable]
    public class ResponsePostDataUserInfoList{        }



    private void Start() {
        noticeText.enabled = false;
    }

    // 버튼 클릭 시 호출될 함수
    public void PostLogin()
    {
        LocalLoadDataSignup localLoadDataSignup = DataManager.instance.LoadJsonData<LocalLoadDataSignup>("Post Data Signup.json");

        // 회원 가입 함?
        if(localLoadDataSignup != null){
            // 유저 아이디 일치?
            if(localLoadDataSignup.userId == userIdInputField.text){
                // 비밀번호 일치?
                if(localLoadDataSignup.userPassword == userPasswordInputField.text){
                    print("Login.cs PostLogin : 로그인 성공");
                    noticeText.enabled = false;
                    ServerData.Instance.userCode = localLoadDataSignup.userCode;
                    ServerData.Instance.userId = localLoadDataSignup.userId;
                    ServerData.Instance.userName = localLoadDataSignup.userName;
                    SceneManager.LoadScene("MenuScene");
                }
                // 비밀번호 불일치
                else{
                    Debug.LogWarning("비밀번호 틀림");
                    noticeText.enabled = true;
                }
            }
            // 유저 아이디 불일치
            else{
                Debug.LogWarning("아이디 틀림");
                Debug.LogError("Login.cs PostLogin : 로그인 실패");
                noticeText.enabled = true;
            }
        }
        // 회원 가입 안함
        else if(localLoadDataSignup == null){
            Debug.LogWarning("회원가입 필요.");
            Debug.LogError("Login.cs PostLogin : 로그인 실패");
            noticeText.enabled = true;
        }
        
        // API POST 요청 로그인
        PostDataLogin postDataLogin = new PostDataLogin();
        postDataLogin.userId = userIdInputField.text;
        postDataLogin.userPssword = userPasswordInputField.text;

        StartCoroutine(DataManager.instance.SendApiRequest("https://j10b310.p.ssafy.io/user/login", "POST", postDataLogin, onComplete, onError));
    }

    // API POST 요청 로그인 성공
    void onComplete(string response){
        print("Login.cs PostLogin() : " + response);

        // 로그인 하는 동시에 유저 데이터 리스트 조회
        PostDataUserInfoList postDataUserInfoList = new PostDataUserInfoList();
        postDataUserInfoList.userCode = ServerData.Instance.userCode;
        print(postDataUserInfoList.userCode);
        SceneManager.LoadScene("MenuScene");
        // API POST 요청 유저 정보 리스트
        // StartCoroutine(DataManager.instance.SendApiRequest("https://j10b310.p.ssafy.io/data/list", "POST", postDataUserInfoList, onCompleteUserList, onErrorUserList));
    }

    // API POST 요청 로그인 실패
    void onError(string error){
        Debug.LogError("Login.cs PostLogin() : " + error);
        //    // 로그인 실패 시, 실패 팝업 띄움
        noticeText.enabled = true;
    }

    // API POST 요청 유저 정보 리스트 성공
    void onCompleteUserList(string response){
        print("Login.cs onComplete() : " + response);
        SceneManager.LoadScene("MenuScene");
    }

    // API POST 요청 유저 정보 리스트 실패
    void onErrorUserList(string error){
        Debug.LogWarning("Login.cs onComplete() : " + error + " -- 아마도 데이터 없음");
        SceneManager.LoadScene("MenuScene");
    }



    /////////////////////////////////
    ////////////////////////////////
    //////// 테스트 용 아이디
    public void TestLogin(){
        print("Login.cs TestLogin : 테스트 로그인 성공");
        ServerData.Instance.userId = "테스트 유저 ID";
        ServerData.Instance.userName = "테스트 유저 이름";
        SceneManager.LoadScene("MenuScene");
    }
}
