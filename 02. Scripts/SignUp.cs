using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;
using Unity.VisualScripting;
using WebSocketSharp.Server;
using UnityEngine.SceneManagement;

public class SignUp : MonoBehaviour
{
    public TMP_InputField userNameInputField;
    public TMP_InputField userIdInputField;
    public TMP_InputField userPasswordInputField;
    public TMP_InputField userAnswerInputField;
    public TMP_Text noticeText;


    // API POST 데이터 회원가입
    [System.Serializable]
    public class PostDataSignup{
        public string userName;
        public string userId;
        public string userPassword;
        public string userAnswer;
    }
    // API POST 데이터 회원가입 (응답 값)
    [System.Serializable]
    public class ResponsPostDataSignup{
        public int userCode;
        public string userId;
        public string userName;
        public string userPassword;
        public string userAnswer;
    }
    // API POST 데이터 회원가입 (로컬 저장)
    [System.Serializable]
    public class LocalPostDataSignup{
        public int userCode;
        public string userId;
        public string userName;
        public string userPassword;
        public string userAnswer;
    }



    // API POST 데이터 아이디 중복
    [System.Serializable]
    public class PostDataIDCheck{
        public string userId;
    }
    // API POST 데이터 아이디 중복 (응답 값)
    [System.Serializable]
    public class ResponsisChek{
        public bool idCheck;
    }
    // 아이디 중복 확인
    public bool isCheckID = true;



    // 버튼 클릭 시 호출될 함수
    public void PostSignUp()
    {
        // 회원 가입 공백 있음?
        if(userNameInputField.text == "" || userIdInputField.text == "" || userPasswordInputField.text == "" || userAnswerInputField.text == ""){

            Debug.LogWarning("회원 가입 공백 있음");
        }
        else{
            // API POST 요청 아이디 중복
            PostDataIDCheck postDataIDCheck = new PostDataIDCheck();
            postDataIDCheck.userId = userIdInputField.text;
            
            // API POST 요청 아이디 중복
            StartCoroutine(DataManager.instance.SendApiRequest("https://j10b310.p.ssafy.io/user/check", "POST", postDataIDCheck, onCompleteCheck, onErrorCheck));    
        }
    }


    // API POST 요청 아이디 중복 성공
    void onCompleteCheck(string response){
        print("Signup.cs PostSignUp() Check : " + response);

        noticeText.enabled = false;

        // Json 직렬화
        var responseData = JsonUtility.FromJson<ResponsisChek>(response);

        // 아이디 중복 값 추출
        isCheckID = responseData.idCheck;

        // API POST 요청 회원가입을 감싸는 코투린 함수
        // (아이디 중복 확인 API 요청을 동기적으로 처리하기 위한 장치)
        StartCoroutine(PostSignUpRequest());
    }

    // API POST 요청 아이디 중복 실패
    void onErrorCheck(string error){
        Debug.LogError("Signup.cs PostSignUp() Check : " + error);
        noticeText.enabled = true;
    }


    IEnumerator PostSignUpRequest()
    {
        // API POST 요청 회원가입
        PostDataSignup postDataSignup = new PostDataSignup();
        postDataSignup.userName = userNameInputField.text;
        postDataSignup.userId = userIdInputField.text;
        postDataSignup.userPassword = userPasswordInputField.text;
        postDataSignup.userAnswer = userAnswerInputField.text;

        // 아이디 중복 확인이 완료되지 않았으면 대기
        while (isCheckID)
        {
            yield return null;
        }

        // 아이디 중복이 없다면 회원가입 요청 보내기
        if (!isCheckID)
        {
            yield return StartCoroutine(DataManager.instance.SendApiRequest("https://j10b310.p.ssafy.io/user/signup", "POST", postDataSignup, onComplete, onError));
        }
        else
        {
            Debug.LogError("Sign Up.cs PostSignUpRequest() : 아이디 중복 입니다.");
        }
    }

    // API POST 요청 회원가입 성공
    void onComplete(string response){
        print("Signup.cs PostSignUpRequest() : " + response);

        // Json 직렬화
        var responseData = JsonUtility.FromJson<ResponsPostDataSignup>(response);

        // 회원가입 정보 추출 (GameManager 저장)
        ServerData.Instance.userCode = responseData.userCode;
        ServerData.Instance.userId = responseData.userId;
        ServerData.Instance.userName = responseData.userName;


        // 회원가입 정보 추출 (Local 저장)
        LocalPostDataSignup localPostDataSignup = new LocalPostDataSignup();
        localPostDataSignup.userCode = responseData.userCode;
        localPostDataSignup.userId = responseData.userId;
        localPostDataSignup.userName = responseData.userName;
        localPostDataSignup.userPassword = responseData.userPassword;
        localPostDataSignup.userAnswer = responseData.userAnswer;

        DataManager.instance.SaveJsonData(localPostDataSignup, "Post Data Signup.json");

        // 회원 가입 성공 시, 메뉴 씬 이동
        print("Signup.cs PostSignUpRequest() : 회원 가입 성공!");
        SceneManager.LoadScene("MenuScene");
    }

    // API POST 요청 회원가입 실패
    void onError(string error){
        // 찾기 살패 시, 실패 팝업 창 생성
        print("Signup.cs PostSignUpRequest() : " + error);
    }










    ////////////////////////
    ///////////////////////
    /// API XXX
    
    
    // [System.Serializable]
    // public class LocalDataSignup{
    //     public int userCode;
    //     public string userName;
    // }
    // public void PostSignUp(){
    //     print("Signup.cs PostSignUp() : ");

    //     LocalDataSignup localDataSignup = new LocalDataSignup();
    //     localDataSignup.userCode = 1;
    //     localDataSignup.userName = userNameInputField.text;
    //     DataManager.instance.SaveJsonData(localDataSignup, "APIPostSingupResponseData.json");

    //     // 찾기 성공 시, 성공 팝업 창 생성
    //     print("Signup.cs PostSignUp() : 1번 우저 회원 가입 성공!");
    // }
}
