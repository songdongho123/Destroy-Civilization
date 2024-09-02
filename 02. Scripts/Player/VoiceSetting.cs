using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class VoiceSetting  : MonoBehaviourPunCallbacks
{
    private PlayerController playerController;
    private PhotonView photonView;

    private bool isWhisperMode = false; // 귓속말 모드 상태

    private AudioSource audioSource;

    public Recorder voiceRecorder; // Inspector에서 할당해야 합니다.

    public PhotonView targetPhotonView = null;

    private void Awake()
    {
        
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

    public void ToggleWhisperMode()
    {
        
    
        Debug.Log("귓속말 모드.");
        isWhisperMode = !isWhisperMode;
        byte whisperGroup = isWhisperMode ? (byte)1 : (byte)0;


         if (targetPhotonView != null)
        {
            voiceRecorder.InterestGroup = whisperGroup;
            targetPhotonView.RPC("ChangeAudioSourceSettings", RpcTarget.All, whisperGroup, isWhisperMode);
            GetComponent<PhotonView>().RPC("ChangeAudioSourceSettings", RpcTarget.All, whisperGroup, isWhisperMode);
        }
        else
        {
            Debug.Log("No target selected for whisper mode.");
        }
    
    }

    [PunRPC]
    public void ChangeAudioSourceSettings(byte group, bool isWhisper)
    {

        
        Debug.Log("통신확인");
        if (voiceRecorder != null)
        {
            voiceRecorder.InterestGroup = group;
        }
        
        AudioSource localAudioSource = audioSource; // 현재 플레이어의 AudioSource
    

        if (isWhisper)
        {
            // 귓속말 모드 활성화 시 설정
            if (localAudioSource != null)
            {
                localAudioSource.minDistance = 1000;
                localAudioSource.maxDistance = 10000;
                localAudioSource.rolloffMode = AudioRolloffMode.Linear;
                VoiceConnection voiceConnection = FindAnyObjectByType<VoiceConnection>();

                if (voiceConnection != null)
                {
                    AudioSource audioSource = voiceConnection.GetComponent<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.spatialBlend = 1f;
                    }
                }
                
                
            }

            PunVoiceClient.Instance.Client.OpChangeGroups(null, new byte[] { group });
        }
        else
        {
            if (localAudioSource != null)
            {
                localAudioSource.minDistance = 1;
                localAudioSource.maxDistance = 5;
                localAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            }

            PunVoiceClient.Instance.Client.OpChangeGroups(new byte[] {  group }, null);
        }
    }
}


