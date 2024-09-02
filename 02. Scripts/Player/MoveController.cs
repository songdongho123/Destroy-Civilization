using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// // 자동으로 CharacterController 컴포넌트 추가
// [RequireComponent(typeof(CharacterController))]
public class MoveController : MonoBehaviourPunCallbacks
{
    // 컴포넌트 가져오기
    public CharacterController characterController;
    
    // 이동 속도
    [SerializeField] public float moveSpeed;
    // 이동 방향
    [SerializeField] public Vector3 moveForce;
    // 점프력
    [SerializeField] public float jumpForce;
    // 중력
    [SerializeField] public float gravity;
 private void Start()
    {
        
    }
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Debug.Log("ttqtqqt " + characterController.gameObject.transform);
		jumpForce = 7f;
		gravity = -10f;
    }

    // 플레이어 중력 적용 기능
    private void Update()
    {   
        
        Debug.Log($"IsGrounded: {characterController.isGrounded}, MoveForce: {moveForce}, Position: {transform.position}");
        // 플레이어가 공중에 있다면,
        if ( !characterController.isGrounded)
        {
            // 중력 설정
            moveForce.y += gravity * Time.deltaTime;
        }

        Debug.Log("dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");
        characterController.Move(moveForce * Time.deltaTime);
    }

    // 이동 속도 재 설정
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }




    // 플레이어 움직임 설정
    [PunRPC]
    public void PerformMoveTo(Vector3 direction)
    {
        // 움직이려는 방향 설정
        direction = transform.rotation * new Vector3(direction.x,0, direction.z);

        // 움직이는 힘 설정
        moveForce = new Vector3(direction.x*moveSpeed, moveForce.y, direction.z*moveSpeed);
    }

    // RPC 움직임 호출
    public void MoveTo(Vector3 direction)
    {
        if(GetComponent<PhotonView>().IsMine)
        {
            GetComponent<PhotonView>().RPC("PerformMoveTo", RpcTarget.All, direction);
        }
    }



     // 플레이어 점프 설정
    [PunRPC]
    public void PerformJump()
    {
        // 플레이어가 땅에 있다면,
        if (characterController.isGrounded)
        {
            // 점프력 설정
            moveForce.y = jumpForce;
        }
    }

    // RPC 점프 호출
    public void Jump()
    {
        // // 플레이어가 땅에 있다면,
        // if (characterController.isGrounded)
        // {
        //     // 점프력 설정
        //     moveForce.y = jumpForce;
        // }
        // 현재 이 캐릭터가 나의 캐릭터이고, 내 캐릭터가 땅에 있다면 
        if (GetComponent<PhotonView>().IsMine && characterController.isGrounded)
        {
            // PerformJump 함수를 서버 내에 모든 사람들이 실행하도록 호출한다.
            GetComponent<PhotonView>().RPC("PerformJump", RpcTarget.All);
        }
    }

    public void SetPosition(Vector3 newPosition)
    {
        StartCoroutine(SetPositionCoroutine(newPosition));
    }

    private IEnumerator SetPositionCoroutine(Vector3 newPosition)
    {
        characterController.enabled = false; // CharacterController 비활성화
        yield return null; // 한 프레임 대기
        transform.position = newPosition; // 위치 변경
        moveForce = Vector3.zero; // MoveForce 초기화
        yield return new WaitForSeconds(0.1f); // 충분한 대기 시간 제공
        characterController.enabled = true;  // CharacterController 활성화
    }
}
