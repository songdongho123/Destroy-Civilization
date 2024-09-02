using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportButtonUI : MonoBehaviourPunCallbacks
{
    private PlayerController myCharacter;

    [SerializeField]
    private Button repoertButton;

    public void SetInteractable(bool isInteractable)
    {
        repoertButton.interactable = isInteractable;
    }

    private void Start()
    {
        // Find the local player's character
        GameObject localPlayer = GameObject.FindGameObjectWithTag("Player");
        if (localPlayer != null)
        {
            myCharacter = localPlayer.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Local player's character not found.");
        }
    }

    public void StartReport()
    {
        
    }
}
