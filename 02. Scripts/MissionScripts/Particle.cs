using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Transform player; // 플레이어의 위치

    public float Range = 10.0f; // 플레이어와 객체 사이의 최대 거리
    public GameObject[] childObjectes;




    void OnMouseOver()
    {
        if (player) {
            float distance = Vector3.Distance(player.position, transform.position);
            print(distance);
            if (distance <= Range)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ParticleON();
                }
            }
        }
    }

    private void ParticleON()
    {
        for(int i = 0; i < childObjectes.Length; i ++){
            childObjectes[i].SetActive(true);
            ParticleSystem particleSystem = childObjectes[i].GetComponent<ParticleSystem>();
            if(particleSystem != null)
            {
                particleSystem.Play();
            }
        }
    }
}
