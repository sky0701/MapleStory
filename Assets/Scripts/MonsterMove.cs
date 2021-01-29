using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class MonsterMove : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB_M;
    public Animator AN_M;
    public SpriteRenderer SR_M;
    public PhotonView PV_M;
    public Image MonsterHP;

    public int nextMove;
    void Awake()
    {
        MoveAI();
        // Invoke("MoveAI", 5);
        //주어진 시간이 지난 뒤, 지정된 함수를 실행하는 함수
    } 
    [PunRPC]
    public void Attacked()
    {
        AN_M.SetTrigger("attacked");


        MonsterHP.fillAmount -= 0.1f;
        if (MonsterHP.fillAmount <= 0)
        {
            PV_M.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
        Invoke("UnAttacked", 3);

    }
    public void UnAttacked()
    {
        AN_M.SetTrigger("attacked");
    }


    public void FixedUpdate()
    {
        RB_M.velocity = new Vector2(nextMove, RB_M.velocity.y);
    }

    [PunRPC]
    void FlipXRPC_M(int axis) => SR_M.flipX = nextMove == 1;
    public void MoveAI()
    {
        nextMove = Random.Range(-1,2);
       // if(nextMove<0) SR_M.flipX = false;
      //  else if(nextMove > 0) SR_M.flipX = true;
       //bool axis = SR_M.flipX;
        PV_M.RPC("FlipXRPC_M", RpcTarget.AllBuffered, nextMove);
        Invoke("MoveAI", 3);
        //최소값 이상, 최대값 미만

    }
    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);
   




    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
