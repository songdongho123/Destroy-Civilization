using UnityEngine;

public class CameraController1 : MonoBehaviour {
    [Header("Target")]
    public GameObject target;               // 카메라가 따라다닐 타겟
    public GameObject target2;
    public Vector3 targetPos;               // 타겟의 위치
    
    [Header("Offset")]
    public float offsetX = 0.0f;            // 카메라의 x좌표
    public float offsetY = 1.0f;           // 카메라의 y좌표
    public float offsetZ = -1.0f;          // 카메라의 z좌표
    public float cameraSpeed = 5.0f;       // 카메라의 속도
    
    [Header("Angle")]
    public float angleX = 0.0f;             // 카메라의 x각도
    public float angleY = 0.0f;             // 카메라의 y각도
    public float angleZ = 0.0f;             // 카메라의 z각도
    
    [Header("Rotate Speed")]
    // 카메라 x 축 회전 스피드
    [SerializeField] private float rotCamXAxisSpeed = 2;
    // 카메라 y 축 회전 스피드
    [SerializeField] private float rotCamYAxisSpeed = 2;
    
    [Header("Rotate Limit")]
    // 최대 ~ 최소 회전 제한 변수
    [SerializeField] private float limitMinX = -80;
    [SerializeField] private float limitMaxX = 50;

    private void Awake()
    {
        // 타겟 : 플레이어 설정
        target = gameObject.transform.parent.gameObject;
        target2 = GameObject.FindGameObjectWithTag("Player");

        // 카메라 좌표, 속도 재설정
        offsetX = 0.0f;
        offsetY = 1.7f;
        offsetZ = 0.0f;
        cameraSpeed = 1000.0f;
        
        // 마우스 커서 설정.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // Player 레이어를 카메라의 Culling Mask에서 제외
        // 이 작업으로 인해 시야 변경 시 자신의 캐릭터가 단면적으로 보이는 걸 없앰
        // Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
    }

    private void Update() 
    {
        // 마우스 좌우, 위아래 이동
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        // 회전
        UpdateRotate(mouseX, mouseY);
    }
    
    void FixedUpdate()
    {
        // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
        targetPos = new Vector3(
            target.transform.position.x + offsetX,
            target.transform.position.y + offsetY,
            target.transform.position.z + offsetZ
        );
        
        transform.rotation = Quaternion.Euler(angleX, angleY, angleZ);
        
        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);
        
        // 카메라가 항상 타겟을 바라보도록 함
        transform.LookAt(target.transform);
        
        // 캐릭터의 회전 설정
        target.transform.rotation = Quaternion.Euler(0, angleY, 0);
    }
    
    // 회전 함수
    public void UpdateRotate(float mouseX, float mouseY)
    {
        // 각도 = 마우스 위치 * 카메라 회전 스피드
        angleY += mouseX * rotCamYAxisSpeed;
        angleX -= mouseY * rotCamXAxisSpeed;

        // 최소, 최대 각도 제한
        angleX = ClampAngle(angleX, limitMinX, limitMaxX);

        // 숫자 각도를 운동량에 맞게 변환
        transform.rotation = Quaternion.Euler(angleX, angleY, 0);
    }

    // 회전 각 - 최대, 최소 제한 범위 기능
    public float ClampAngle(float angle, float min, float max)
    {
        // 최대 360 ~ 최소 -360
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        // 각 제한
        return Mathf.Clamp(angle,min,max);
    }
}