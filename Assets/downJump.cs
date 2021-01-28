using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class downJump : MonoBehaviour
{

    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Player.layer);
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        
        if(col.gameObject.tag == "Uptile")
        {
            Debug.Log("트리거중");
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                DownJump_();
                Debug.Log("눌렀다!"); 
            }
        else
        {

        }

        }
        
    }

    public void OnTriggerExit2D(Collider2D col)
    {
       
            OffDownJump_();
            Debug.Log("충돌끝");

    }
    void DownJump_()
    {
        Debug.Log("실행됬어!");
        Player.layer = 14;
    }
    void OffDownJump_()
    {
        Player.layer = 9;
    }
}
