using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text ID;
    public Collider2D CMRange;
    public Collider2D PlayerCollider;

    public GameObject Uptile;

    public GameObject RespawnPanel;

    public Image HP;

    bool Isclimb;
    bool DownJump;


    public LayerMask ladder;
    public float distance;


    CinemachineConfiner CM;

    bool isGround;
    bool isGroundUp;
    bool isAttack;
    Vector3 curPos;

    void Awake()
    { //닉네임
        ID.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        ID.color = Color.white; //컬러 관해서는 클라이언트쪽에서 안먹히는 것 같은데 확인 필요
       
        
       
    }

    void Start()
    {
        RespawnPanel = GameObject.Find("Panel").transform.Find("RespawnPanel").gameObject;
    }
    void Update()
    {   
        
        if (PV.IsMine && transform.position.x >= -13f)
        {//2d 카메라 설정
            var CMCamera = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            var CMRange = GameObject.Find("CMRange1").GetComponent<PolygonCollider2D>();
            CM = CMCamera.GetComponent<CinemachineConfiner>();
            CM.m_BoundingShape2D = CMRange;
            CMCamera.Follow = transform;
            CMCamera.LookAt = transform;
        }
        else if (PV.IsMine && transform.position.x<-13f)
        {
            Debug.Log("바꼈다");
            var CMCamera = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            var CMRange = GameObject.Find("CMRange2").GetComponent<PolygonCollider2D>();
            CM = CMCamera.GetComponent<CinemachineConfiner>();
            CM.m_BoundingShape2D = CMRange;
            CMCamera.Follow = transform;
            CMCamera.LookAt = transform;
        }
        if (PV.IsMine)
        {
            // <- -> 이동
            float axis = Input.GetAxisRaw("Horizontal");  //오른쪽 1 왼쪽 -1 안누르면 0
            RB.velocity = new Vector2(2 * axis, RB.velocity.y);
            if(axis != 0)
            {
                AN.SetBool("walk", true);
                PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis); //재접속시 동기화
            }
            else
            {
                AN.SetBool("walk", false);
            }
            //점프, 바닥체크 
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.5f), 0.07f, 1 << LayerMask.NameToLayer("Ground"));
            isGroundUp = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.5f), 0.07f, 1 << LayerMask.NameToLayer("UpGround"));
            isAttack = Physics2D.OverlapCircle((Vector2)transform.position , 5f, 1 << LayerMask.NameToLayer("Monster"));
            AN.SetBool("jump", !(isGround || isGroundUp));
            if (Input.GetKeyDown(KeyCode.LeftAlt) && (isGround||isGroundUp)) PV.RPC("JumpRPC", RpcTarget.All);

            float axisupdown = Input.GetAxisRaw("Vertical");
            if (axisupdown==-1 && (isGround || isGroundUp))
            {
                AN.SetBool("down", true);
                PV.RPC("DownRPC", RpcTarget.AllBuffered, axisupdown);
            }
            else
            {
                AN.SetBool("down", false);
            }
            //공격
            if (Input.GetKeyDown(KeyCode.LeftControl) && isAttack)
            {
                AN.SetBool("attack", true);
                Debug.Log("hit");
                GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterMove>().Attacked();
                //PV.RPC("AttackRPC", RpcTarget.All);
            }
            else
            {
                AN.SetBool("attack", false);
            }
            //raycast로 사다리 인지
            RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.up,distance,ladder);
            if(hitinfo.collider != null)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Isclimb = true;
                    AN.SetBool("climb", true);
                }
            }
            else
            {
                Isclimb = false;
            }
            if (Isclimb == true)
            {
                RB.velocity = new Vector2(RB.velocity.x, axisupdown * 2);
                RB.gravityScale = 0;
            }
            else
            {
                AN.SetBool("climb", false);
                RB.gravityScale = 3;
            }

           
        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) //너무 멀리 떨어져 있는 경우
        {
            transform.position = curPos; //curPos로 바로 받아줌. 순간이동
        }
        else
        {//가까이 있으면 lerp
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
        }
    }

    [PunRPC]
    void FlipXRPC(float axis) => SR.flipX = axis == 1; //왼쪽키(-1)일 경우 true를 반환
    [PunRPC]
    void JumpRPC()
    {
        RB.velocity = Vector2.zero;
        RB.AddForce(Vector2.up * 400);
    }
    [PunRPC]
    void DownRPC(float axisdown) {

    }
   [PunRPC]
    void AttackRPC()
    {

    }

   public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            OnDamaged(collision.transform.position);
        }
    }
  
    void OnDamaged(Vector2 targetPos)
    {//몬스터랑 부딪혔을때 무적되는 것
        HP = GameObject.Find("PlayerHealth").GetComponentInChildren<Image>();
        HP.fillAmount -= 0.05f;
        if (HP.fillAmount <= 0)
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            RespawnPanel.SetActive(true);
            //GameObject.Find("NetworkManager").GetComponent<NetworkManager>().Spawn();
            HP.fillAmount = 1;
        }
        gameObject.layer = 11;
        SR.color = new Color(1, 1, 1, 0.4f);

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        RB.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);
        Invoke("OFFDamaged",3);
    }
    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);
    void OFFDamaged() 
    { 
        gameObject.layer = 9;
        SR.color = new Color(1, 1, 1, 1);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {//변수 동기화
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
}
