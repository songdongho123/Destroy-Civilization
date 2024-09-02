using UnityEngine;
using UnityEngine.UI;

public class HackerSkill : MonoBehaviour
{
    public GameObject modalPanel; // 모달 패널 오브젝트
    //public RawImage modalImage; // 모달 내 RawImage 컴포넌트
    //public Texture tex1; // 할당할 첫 번째 텍스처
    //public Texture tex2; // 할당할 두 번째 텍스처

    void Start()
    {
        //modalPanel.SetActive(false); // 게임 시작 시 모달 창을 숨김
        //modalImage.texture = null; // 기본적으로 Texture를 할당하지 않음

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
            // 'Q' 키를 누를 때마다 모달 창의 활성화 상태를 토글
            modalPanel.SetActive(!modalPanel.activeSelf);
        }

/*        // 마우스 좌클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Raycast로 오브젝트 감지
            if (Physics.Raycast(ray, out hit))
            {
                // 클릭한 오브젝트가 Test1이라면
                if (hit.collider.gameObject.name == "Test1")
                {
                    Debug.Log("1번눌림");
                    modalImage.texture = tex1; // RawImage의 Texture를 Tex1로 변경
                }
                // 클릭한 오브젝트가 Test2라면
                else if (hit.collider.gameObject.name == "Test2")
                {
                    modalImage.texture = tex2; // RawImage의 Texture를 Tex2로 변경
                }
            }
        }*/
    }
}
