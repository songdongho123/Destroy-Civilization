using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionListUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private float offset;

    [SerializeField]
    private RectTransform MissionListUITransform;

    private bool isOpen = true;

    private float timer;


    public void OnPointerClick(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(OpenAndHideUI());
    }

    private IEnumerator OpenAndHideUI()
    {
        isOpen = !isOpen;
        if(timer != 0f)
        {
            timer = 1f - timer;
        }

        while (timer <= 1f)
        {
            timer += Time.deltaTime * 2f;

            float start = isOpen ? -MissionListUITransform.sizeDelta.x : offset;
            float dest = isOpen ? offset : -MissionListUITransform.sizeDelta.x;
            MissionListUITransform.anchoredPosition = new Vector2(Mathf.Lerp(start, dest, timer), MissionListUITransform.anchoredPosition.y);
            yield return null;
        }
    }
}
