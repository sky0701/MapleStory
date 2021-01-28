using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField IDInput;
    public GameObject DisconnectPanel;
    public GameObject Map1;
    public GameObject Map2;

    public GameObject RespawnPanel;
    

    //Audio
  



    void Awake()
    {
       
        Screen.SetResolution(1960, 1080, true);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        

    }

    void Start()
    {
        GameObject.Find("AudioManager").GetComponent<Audio_manager>().loginAudioManager();

    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    
            
    }
    public override void OnConnectedToMaster()
    {
        
        //audio_Manager.loginAudioManager();
        PhotonNetwork.LocalPlayer.NickName = IDInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null);
        
    }

    public override void OnJoinedRoom()
    {
        DisconnectPanel.SetActive(false);
        GameObject.Find("AudioManager").GetComponent<Audio_manager>().Map1AudioManager();
        Spawn();
        MonsterSpawn();

        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }
    
    public void Spawn()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        RespawnPanel.SetActive(false);
    }
     public void MonsterSpawn()
     {
         PhotonNetwork.InstantiateRoomObject("Monster", new Vector3(Random.Range(-3f, 3f), 1, 0), Quaternion.identity);
        if (GameObject.FindGameObjectsWithTag("Monster").Length < 5)
        {
            Invoke("MonsterSpawn", 10);
        }
         
     }
   /* public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectPanel.SetActive(true);
        RespawnPanel.SetActive(false);
    }*/
    
}
