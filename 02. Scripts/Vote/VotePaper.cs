using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VotePaper : MonoBehaviour
{
    public RectTransform VoteUI;
    public Animator anime;

    Player enterPlayer;

    public void Enter()
    {
        Debug.Log("Enter Vote");
        VoteUI.anchoredPosition = Vector3.zero;
    }

    public void Exit()
    {   
        anime.SetTrigger("Shark-Hello");
        VoteUI.anchoredPosition = Vector3.down * 1000;
    }
}
