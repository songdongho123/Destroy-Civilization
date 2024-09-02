using UnityEngine;
using UnityEngine.UI;

public class HackerSkill : MonoBehaviour
{
    public GameObject modalPanel; // ��� �г� ������Ʈ
    //public RawImage modalImage; // ��� �� RawImage ������Ʈ
    //public Texture tex1; // �Ҵ��� ù ��° �ؽ�ó
    //public Texture tex2; // �Ҵ��� �� ��° �ؽ�ó

    void Start()
    {
        //modalPanel.SetActive(false); // ���� ���� �� ��� â�� ����
        //modalImage.texture = null; // �⺻������ Texture�� �Ҵ����� ����

        GameObject parentObject = GameObject.Find("Game UI");
        if (parentObject != null)
        {
            modalPanel = parentObject.transform.Find("HackerModal").gameObject;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 'Q' Ű�� ���� ������ ��� â�� Ȱ��ȭ ���¸� ���
            modalPanel.SetActive(!modalPanel.activeSelf);
        }

/*        // ���콺 ��Ŭ�� ����
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Raycast�� ������Ʈ ����
            if (Physics.Raycast(ray, out hit))
            {
                // Ŭ���� ������Ʈ�� Test1�̶��
                if (hit.collider.gameObject.name == "Test1")
                {
                    Debug.Log("1������");
                    modalImage.texture = tex1; // RawImage�� Texture�� Tex1�� ����
                }
                // Ŭ���� ������Ʈ�� Test2���
                else if (hit.collider.gameObject.name == "Test2")
                {
                    modalImage.texture = tex2; // RawImage�� Texture�� Tex2�� ����
                }
            }
        }*/
    }
}
