using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ��� ���
 * 1. arrowObject �ϳ��� Hierarchy�� ������ݴϴ� (ȭ��ǥ Texture)
 * 2. �ش� ������Ʈ�� Rotation�� 80����, Position�� 2�� �� ��, ������ ���⿡ �°� �����մϴ�.
 * 3. �ش� ������Ʈ�� �� ��ũ��Ʈ ������ Add�� �� arrowObject�� �̵���ų ������Ʈ�� �巡���Ͽ� ����մϴ�.
 * ���� �ڵ忡���� �÷��̾��� ����� true�� �����ϰ� 0,0,0���� ���� ����ϰ� ������, 
 * ���� ����Ͻ� ���� deathPosition�� ���� ĳ���� ��ġ�� ���� �ڵ带 ���� �޾� ����Ͻø� �ʹ̴�
 */
public class Detective : MonoBehaviour
{
    public Transform arrowObject; // ȭ��ǥ ������Ʈ (Unity �ν����Ϳ��� �Ҵ�)
    public Vector3 deathPosition; // ���� ������Ʈ�� ��ġ
    private bool isPlayerDead = true; // �÷��̾��� ��� ����

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isPlayerDead)
        {
            // ���� ������Ʈ�� ��ġ�� ȭ�� ���� �ִ��� Ȯ��
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(deathPosition);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            arrowObject.gameObject.SetActive(true);

            if (onScreen)
            {
                spriteRenderer.color = Color.red;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }

    // �÷��̾� ����� ���õ� ������ ó���ϴ� �Լ�
    public void OnPlayerDeath(Vector3 position)
    {
        deathPosition = position;
        isPlayerDead = true;
    }
}
