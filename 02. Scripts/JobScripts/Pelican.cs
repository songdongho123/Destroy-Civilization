using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class Pelican : MonoBehaviourPunCallbacks
{
    

    private SignalMan signalMan;

    public Recorder voiceRecorder; // Inspector에서 할당해야 합니다.

    private AudioSource audioSource;

    private PlayerController playerController;

    public PhotonView targetPhotonView = null;

    public bool isAlive;
    

    private void Awake()
    {
    
        
        playerController = GetComponent<PlayerController>();
        isAlive = PlayerManager.Instance.CheckIfPlayerIsAlive(photonView);
    

        GameObject speakerObject = transform.Find("Speaker")?.gameObject;
        if (speakerObject != null)
        {
            voiceRecorder = speakerObject.GetComponent<Recorder>();
            audioSource = speakerObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("Speaker object not found in player's children!");
        }
    }

    private void Update()
    {
        
        if (isAlive) 
        {
            if(GetComponent<PhotonView>().IsMine)
            {
                if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
                {
                    SelectTarget();
                }

                if (Input.GetKeyDown(KeyCode.T) && targetPhotonView != null) // Q 버튼을 누르고 대상이 선택되어 있을 때
                {
                    GetComponent<PhotonView>().RPC("CapturePlayer", RpcTarget.All, targetPhotonView.ViewID);
                }
            }
        }else
        {
    
            targetPhotonView.RPC("ChangeStateToCaptured", targetPhotonView.Owner, photonView.ViewID);
            targetPhotonView.RPC("ChangeStateToCaptured", targetPhotonView.Owner, targetPhotonView.ViewID);
        }


    }


    public void SelectTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            PhotonView hitPhotonView = hit.collider.gameObject.GetComponent<PhotonView>();
            Debug.Log(hitPhotonView);
            if (hitPhotonView != null && !hitPhotonView.IsMine) // 대상이 로컬 플레이어가 아닌 경우
            {
                targetPhotonView = hitPhotonView; // 대상의 PhotonView를 저장
                Debug.Log(targetPhotonView);
            }
        }
    }

    


    public void InvokeCapturePlayer()
    {
        if (signalMan.targetPhotonView != null) // targetPhotonView가 public으로 선언되어 있을 때 직접 접근
        {
            // PhotonView의 RPC를 호출하여 CapturePlayer 메서드를 네트워크상의 모든 클라이언트에서 실행합니다.
            Debug.Log("펠리컨호출");
            GetComponent<PhotonView>().RPC("CapturePlayer", RpcTarget.All, signalMan.targetPhotonView.ViewID);
        }
        else
        {
            Debug.LogError("No target selected.");
        }
    }   
    
    [PunRPC]
    public void CapturePlayer(int targetViewID)
    {

    Debug.Log("잡아먹기");
    PhotonView targetPhotonView = PhotonView.Find(targetViewID);
    if (targetPhotonView != null)
        {
        
        // "Bear Attack" 오브젝트 찾기 및 비활성화
        Transform bearAttackTransform = targetPhotonView.transform.Find("Bear Attack");
        if (bearAttackTransform != null)
        {
            bearAttackTransform.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Bear Attack object not found!");
        }

        targetPhotonView.RPC("ChangeStateToCaptured", targetPhotonView.Owner, targetPhotonView.ViewID);

        targetPhotonView.RPC("OnPlayerKilled", RpcTarget.All, targetPhotonView.ViewID,GetComponent<PhotonView>().ViewID );
        
        // 잡아먹힌 플레이어를 5번 채널로 이동
        targetPhotonView.RPC("ChangeVoiceChannel", RpcTarget.All, 0,5);

        // 잡아먹은 플레이어를 0번과 5번 채널에 모두 속하도록 설정
        ChangeVoiceChannel(0, 5);
        }
    }

    [PunRPC]
    void ChangeVoiceChannel(int channel1, int channel2 = -1, PhotonMessageInfo info = default)
    {
        voiceRecorder.InterestGroup = 5;
        byte[] channels = channel2 >= 0 ? new byte[] { (byte)channel1, (byte)channel2 } : new byte[] { (byte)channel1 };
        PunVoiceClient.Instance.Client.OpChangeGroups(null, channels);
    }


    [PunRPC]
    public void OnPlayerKilled(int killedPlayerViewID, int killerPlayerViewID)
    {
        PhotonView killedPlayerView = PhotonView.Find(killedPlayerViewID);
        PhotonView killerPlayerView = PhotonView.Find(killerPlayerViewID);
        Debug.Log(killedPlayerView);
        Debug.Log(killerPlayerView);

        if (killedPlayerView != null && killerPlayerView != null)
        {
            if (killedPlayerView.IsMine)
            {
                
                Debug.Log("잡아먹히는 중");
                // B의 카메라 비활성화
                Transform killedCameraTransform = killedPlayerView.transform.Find("Player Camera");
                killedCameraTransform.gameObject.SetActive(false);

                PlayerController killedPlayerController = killedPlayerView.GetComponent<PlayerController>();

                if (killedPlayerController != null)
                {
                    // targetPhotonView의 소유자면, 상태를 변경합니다.
                
                    {
                        // killedPlayerController.currentState = PlayerController.PlayerState.Captured;


                        // targetPhotonView 소유자의 카메라를 비활성화합니다.
    
                    }
                }

                // A의 카메라 찾기 및 활성화
                // 이 예제에서는 A의 카메라가 "Player Camera"라는 이름의 자식 오브젝트라고 가정합니다.
                Transform killerCameraTransform = killerPlayerView.transform.Find("Player Camera");
                if (killerCameraTransform != null)
                {
                    killerCameraTransform.gameObject.SetActive(true);
                }
            }
        }
    }
}

