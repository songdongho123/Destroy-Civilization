using System.Collections;
using System.Collections.Generic;
using Photon.Voice.Unity;
using UnityEngine;

public class Dock : MonoBehaviour
{
    public float detectionRadius = 10f; // 도청장치 감지 범위
    public float minDistance = 10f; // 변경할 최소 거리
    public float maxDistance = 10000f; // 변경할 최대 거리
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("접촉");

            // 'Speaker' 태그를 가진 자식 오브젝트의 AudioSource를 찾음
            Speaker speakerTransform = other.GetComponentInChildren<Speaker>();
            AudioSource audioSource = speakerTransform.GetComponent<AudioSource>();

            // Transform speakerTransform = other.transform.Find("Speaker");
            Debug.Log(speakerTransform);
            if (speakerTransform)
            {
                Debug.Log("스피커 접근");
                Debug.Log(speakerTransform);

                audioSource.minDistance = minDistance;
                audioSource.maxDistance = maxDistance;
                
                
                // if (speakerAudioSource != null)
                // {
                //     // 오디오 소스의 minDistance와 maxDistance를 변경
                //     Debug.Log("변경 적용");
                //     speakerAudioSource.minDistance = minDistance;
                //     speakerAudioSource.maxDistance = maxDistance;
                // }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 'Speaker' 태그를 가진 자식 오브젝트의 AudioSource를 찾음
            // AudioSource speakerTransform = other.gameObject.GetComponentInChildren<AudioSource>(true);

            Speaker speakerTransform = other.GetComponentInChildren<Speaker>();
            AudioSource audioSource = speakerTransform.GetComponent<AudioSource>();
            // Transform speakerTransform = other.transform.Find("Speaker");
            if (speakerTransform)
            {
                // AudioSource speakerAudioSource = speakerTransform.GetComponent<AudioSource>();
                // if (speakerAudioSource != null)
                // {
                //     // 오디오 소스의 minDistance와 maxDistance를 원래대로 복구
                //     Debug.Log("변경 해제");
                //     speakerAudioSource.minDistance = 1;
                //     speakerAudioSource.maxDistance = 2;
                // }

                Debug.Log("스피커 퇴근");
                Debug.Log(speakerTransform);

                audioSource.minDistance = 1;
                audioSource.maxDistance = 5;
            }
        }
    }
}
