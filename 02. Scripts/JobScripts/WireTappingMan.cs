using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class WireTappingMan : MonoBehaviour
{
    public GameObject recorderPrefab;
    private bool isDeviceSpawned = false; // 도청 장치가 이미 설치되었는지 추적하는 변수

    void Start()
    {
        // Resources.Load는 Start 또는 Awake 함수 내에서 호출하는 것이 좋습니다.
        recorderPrefab = Resources.Load<GameObject>("Recorder");
    }

    void Update()
    {
        // T 키가 눌렸고, 아직 도청 장치가 설치되지 않았다면 도청 장치 설치
        if (Input.GetKeyDown(KeyCode.T) && !isDeviceSpawned)
        {
            SpawnEavesdroppingDevice();
            isDeviceSpawned = true; // 도청 장치가 설치되었음을 표시
        }
    }

    public void SpawnEavesdroppingDevice()
    {
        if (recorderPrefab != null && !isDeviceSpawned)
        {
            // 플레이어 앞에 도청 장치 생성
            Debug.Log("도청장치 생성");
            Vector3 spawnPosition = transform.position + transform.forward * 2; // 현재 플레이어 위치에서 앞으로 2단위
            Quaternion spawnRotation = Quaternion.identity; // 회전 없음

            Instantiate(recorderPrefab, spawnPosition, spawnRotation);
        }
        else
        {
            Debug.LogError("Eavesdropping device prefab is not assigned or device already spawned.");
        }
    }
}
