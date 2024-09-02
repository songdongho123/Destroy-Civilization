using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{   
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime = 1f;  // fadeTime의 기본값을 설정해주었습니다.

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        // 페이드 인을 시작합니다.
        StartCoroutine(FadeInThenOut(image, fadeTime));
    }

    private IEnumerator FadeInThenOut(Image targetImage, float time)
    {
        // 페이드 인
        yield return Fade(1, 0, targetImage, time);
        // 페이드 인이 완료된 후 1.5초 기다림
        yield return new WaitForSeconds(2.5f);
        // 페이드 아웃
        yield return Fade(0, 1, targetImage, time);
    }

    private IEnumerator Fade(float start, float end, Image targetImage, float time)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / time;

            Color color = targetImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            targetImage.color = color;

            yield return null;
        }
    }
}
