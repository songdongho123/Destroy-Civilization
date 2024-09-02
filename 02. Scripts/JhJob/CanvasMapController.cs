using UnityEngine;

public class CanvasMapController : MonoBehaviour
{
    public GameObject canvasMap; // CanvasMap ������Ʈ�� �����Ϳ��� �Ҵ�

    void Start()
    {
        canvasMap.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canvasMap.SetActive(true);
            Debug.Log("keydown��");
        }

        if (Input.GetKey(KeyCode.J))
        {
            canvasMap.SetActive(true);
            Debug.Log("key��");
        }
    }
}
