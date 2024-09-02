using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private string animal;
    private Animator animator;

    private void Awake()
    {   
        // 어떤 동물인지 확인
        animal = Animal();
        
        // 자식 오브젝트에서 Animator 컴포넌트 가져오기
        animator = GetComponentInChildren<Animator>();
        
        // Animator의 runtimeAnimatorController 속성을 사용하여 컨트롤러 설정 (안됨 ㅎㅎ)
        // animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/" + animal);
    }
    
    public string Animal()
    {
        // 캐릭터의 자식 컴포넌트 이름을 가져옴
        string firstChild = transform.GetChild(0).name;
        string secondChild = transform.GetChild(1).name;

        // 캐릭터의 자식 컴포넌트의 이름 중 동일한 부분을 추출 -> 어떤 동물인지 확인 (ex. Cat)
        if (firstChild.Length < secondChild.Length)
        {
            return firstChild;
        }
        else
        {
            return secondChild;
        }
    }

    public float MoveSpeed
    {
        set => animator.SetFloat("movementSpeed", value);
        get => animator.GetFloat("movementSpeed");
    }

    public void Play(string stateName, int layer, float normalizedTime)
    {
        animator.Play(stateName, layer, normalizedTime);
    } 
}
