using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Category : MonoBehaviour
{
    public GameObject headItem;
    public GameObject shirtsitem;

    // 모자 카테고리 클릭 시, 모자 아이템 출력
    public void ClickHeadCategory(){
        headItem.SetActive(true);
        shirtsitem.SetActive(false);
    }

    // 셔츠 카테고리 클릭 시, 셔츠 아이템 출력
    public void ClickShirtsCategory(){
        headItem.SetActive(false);
        shirtsitem.SetActive(true);
    }
}
