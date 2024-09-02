using System.Collections;
using System.Collections.Generic;
using Highlands;
using Photon.Pun;
using TMPro;
using UnityEngine;
using static GameManager;

public class PlayerController : MonoBehaviour
{
    // 플레이어 스피드 설정
    [Header("Walk, Run Speed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    
    // 오디오 할당
    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipWalk;
    [SerializeField] private AudioClip audioClipRun;
    
    // 플레이어 본인 지정
    

    // 1. 플레이어 이동 객체
    public MoveController movement;
    // 2. 플레이어 애니메이션 객체
    private AnimatorController animator;
    // 3. 플레이어 오디오 객체
    private AudioSource audioSource;
    // 4. 캐릭터 컨트롤러
    public CharacterController characterController;
    // 5. 캐릭터 닉네임 캔버스
    public GameObject canvasName;
    public TMP_Text nameText;

    public PlayerState foundPlayerState;

    // attack 관련 변수
    public bool isAlive = true;                     // 살아있는가?
    public int attackerViewID = -1;                 // 누가 죽였는가?
    public bool isProtectedByKnight = false;        // 기사에 의해 보호받고 있는가?
    public bool isProtectedByBodyguard = false;     // 보디가드에 의해 보호받고 있는가?
    public string animal;

    // vote 관련 변수
    public bool isReporter = false;             // 투표자 인가
    public bool isVote;                         // 투표를 했는가
    public int vote;                           // 몇표를 받았는가
    
    // 마우스 커서 on / off
    public bool cursorLocked = true;

    // 투표 trigger로 감지
    GameObject nearObject;
    public VotePaper votepaper;
    bool iDown;

    //레이캐스트로 가져오는 태그값
    public string rayTag;
    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        isAlive = true;
        isProtectedByKnight = false;
        isProtectedByBodyguard = false;
        
        GameObject speakerObject = transform.Find("Speaker")?.gameObject;
        
        // 태그를 Player로 설정
        if (!gameObject.CompareTag("Player"))
        {
            gameObject.tag = "Player";
        }
        
        // 만약 해당 게임 오브젝트에 CharacterController가 없다면, 컴포넌트를 추가하여 할당
        if (GetComponent<CharacterController>() == null)
        {
            gameObject.AddComponent<CharacterController>();
        }
        characterController = GetComponent<CharacterController>();
        characterController.center = new Vector3(0, 1, 0);
        
        // 만약 해당 게임 오브젝트에 AudioSource가 없다면, 컴포넌트를 추가하여 할당
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
        
        // 레이어를 Player로 설정
        // 이 작업으로 인해 시야 변경 시 자신의 캐릭터가 단면적으로 보이는 걸 없앰
        // 현재 GameObject와 하위 GameObject들의 레이어를 Player로 설정
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Player"));
        // gameObject.layer = LayerMask.NameToLayer("Player");
        
        // C# 컴포넌트 스크립트 가져오기.
        movement = GetComponent<MoveController>();
        animator = GetComponent<AnimatorController>();
        audioSource = GetComponent<AudioSource>();
        
        animal = animator.Animal();

        // 걷는 속도, 뛰는 속도 설정
        walkSpeed = 15;
        runSpeed = 35;
    }

    // 플레이어 위에 이름
    private void Start() {
        if(GetComponent<PhotonView>().IsMine == false)
        {
            canvasName.SetActive(true);
            nameText.text = ServerData.Instance.userName;
        }
        votepaper = GetComponent<VotePaper>();

    }

    private void Update() 
    {
        if(GetComponent<PhotonView>().IsMine)
        {
            // Player 이동
            UpdateMove();
            // Payer 점프
            UpdateJump();

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = !cursorLocked;
                print("cursorLocked : " + cursorLocked);
                UpdateCursorState();
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
            {
                // Raycast가 충돌한 GameObject에 있는 Tag를 가져옴
                rayTag = hit.collider.gameObject.tag;
                Debug.Log(tag);
            }
            if (Input.GetKeyDown(KeyCode.R)&&rayTag=="DeadBody")
            {
                // Player 신고
                UpdateReport();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interaction();
            }

        }

        // 카메라 내 꺼만.
        else if(!GetComponent<PhotonView>().IsMine)
        {
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }
    

    // GameObject와 그 하위에 있는 모든 GameObject들의 레이어를 재귀적으로 설정하는 함수
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer; // 현재 GameObject의 레이어 설정
    
        // 현재 GameObject의 모든 하위 GameObject에 대해 재귀적으로 호출
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    // 마우스 커서 on / off
    private void UpdateCursorState()
    {
        if(cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    /////////////// Player 이동
    private void UpdateMove()
    {   
        // 키보드 W A S D, 화살표 키
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        
        // 플레이어가 이동 중이라면,
        if ( x != 0 || z != 0)
        {
            // 달리기 off
            bool isRun = false;
            animator.Play(animal + "_Walk Forward", 2, 0f);

            // 플레이어가 앞으로 이동 중이라면, 달리기 on
            if ( z > 0 ) isRun = Input.GetKey(KeyCode.LeftShift);

            // 플레이어가 달리는 상태라면, 스피드 - 애니메이션 - 오디오 재설정
            movement.MoveSpeed = isRun == true ? runSpeed : walkSpeed;
            animator.Play(animal + "_Run Forward", 2, 0f);
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            // 오디오가 꺼져있다면,
            if (audioSource.isPlaying == false)
            {
                // 오디오 무한 반복으로 재생
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        // 플레이어가 멈춘 상태라면,
        else
        {
            animator.Play(animal + "_Idle", 3, 0f);

            // 스피드 - 애니메이션 - 오디오 멈춤
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;

            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }

        }
        movement.MoveTo(new Vector3(x,0,z));
    }

    /////////////// Player 점프
    private void UpdateJump()
    {   
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play(animal + "_Jump", 1, 0f);
            movement.Jump();
        }

    }
    private void UpdateReport()
    { 
        GetComponent<Report>().CallReport();
    }

    ///////////////
    ///투표 기능

    public void CmdVoteEjectPlayer()
    {
        PhotonView photonView = GetComponent<PhotonView>();

        photonView.RPC("VoteEjectPlayer", RpcTarget.All);
    }

    /*// 투표 했다고 바꿔주고 
    // 누구 투표했는지 참조해오기 위해 playerId 가져와야 함(아직 못함)
    [PunRPC]
    private void VoteEjectPlayer()
    {
        isVote = true;
        Instance.SignVoteEject();

        PlayerController[] players = FindObjectsOfType<PlayerController>();

        foreach (PlayerController player in players)
        {
                player.vote += 1;
        }
    }*/

    void Interaction()
    {
        if(nearObject != null)
        {
            if(nearObject.tag == "Votepaper")
            {
                votepaper.Enter();
            }
        }
        else
        {
            Debug.Log("nearObject");
            Debug.Log(nearObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Votepaper")
            nearObject = other.gameObject;

        Debug.Log("Zone in");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Votepaper")
        {
            VotePaper votepaper = nearObject.GetComponent<VotePaper>();
            votepaper.Exit();
            nearObject = null;
        }
    }
}