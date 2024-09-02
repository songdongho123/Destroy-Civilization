using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour
{
    public Slider progressBar; // UI 프로그레스 바에 대한 참조
    private float fillSpeed = 0.33f; // 3초 동안 차오르도록 설정
    private bool isHolding = false; // 클릭을 계속 누르고 있는지의 여부

    private void Start()
    {
        progressBar.gameObject.SetActive(false);
    }
    public void StartFilling()
    {
        if (!isHolding) // isHolding이 false일 때만 코루틴 시작
        {
            isHolding = true;
            StartCoroutine(FillProgress());
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 버튼을 처음 누를 때
        {
            isHolding = true;
            StartCoroutine(FillProgress());
        }
        if (Input.GetMouseButtonUp(0)) // 마우스 버튼을 떼었을 때
        {
            isHolding = false;
            StopCoroutine(FillProgress());
            progressBar.value = 0; // 프로그레스 바 초기화
            progressBar.gameObject.SetActive(false);
        }
    }

    IEnumerator FillProgress()
    {
        while (isHolding && progressBar.value < 1)
        {
            progressBar.value += fillSpeed * Time.deltaTime; // 프로그레스 바 업데이트
            yield return null;
        }

        if (progressBar.value >= 1)
        {
            // 프로그레스 바가 100% 찼을 때의 동작
            Debug.Log("프로그레스 바 미션 성공"); // 여기에 미션 완료에 대한 요소 추가
            progressBar.gameObject.SetActive(false);
            isHolding = false;
        }
    }
}
