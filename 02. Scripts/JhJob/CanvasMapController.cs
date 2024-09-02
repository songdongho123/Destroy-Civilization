using UnityEngine;

public class CanvasMapController : MonoBehaviour
{
    public GameObject canvasMap; // CanvasMap 오브젝트를 에디터에서 할당

    void Start()
    {
        canvasMap.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canvasMap.SetActive(true);
            Debug.Log("keydown임");
        }

        if (Input.GetKey(KeyCode.J))
        {
            canvasMap.SetActive(true);
            Debug.Log("key임");
        }
    }
}
