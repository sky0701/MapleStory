using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Portal : MonoBehaviourPunCallbacks, IPunObservable
{


    public PhotonView PV;
    public Rigidbody2D RB;
    public GameObject Map1;
    public GameObject Map2;
    public Animator AN;
    Vector3 Map2Pos;
    public bool Isclimb;
    
    void Start()
    {
        Map2Pos = new Vector3(45,16,0);
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        //포탈
        Debug.Log("들어옴");
        if (col.gameObject.tag == "Portal" && Input.GetKeyDown(KeyCode.UpArrow) && PV.IsMine)
        {
            Debug.Log("포탈접촉");
            SoundEffectManager.PlaySound("portal");
            RB.transform.position = Map2Pos;
            GameObject.Find("AudioManager").GetComponent<Audio_manager>().Map2AudioManager();


        }

       
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    
}
