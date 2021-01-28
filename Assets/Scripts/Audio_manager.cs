using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_manager : MonoBehaviour

{
    public GameObject DisconnectPanel;
    public GameObject Map1;
    public GameObject Map2;

    public AudioClip login;
    public AudioClip bgm_map1;
    public AudioClip bgm_map2;

    AudioSource loginAudio;
    AudioSource bgmAudio1;
    AudioSource bgmAudio2;


    // Start is called before the first frame update
    void Awake()
    {
        loginAudio = DisconnectPanel.GetComponent<AudioSource>();
        bgmAudio1 = Map1.GetComponent<AudioSource>();
        bgmAudio2 = Map2.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
                
    }
    public void loginAudioManager()
    {
        loginAudio.clip = login;
        loginAudio.loop = true;
        loginAudio.Play();
        bgmAudio1.Stop();
        bgmAudio2.Stop();
    }
    public void Map1AudioManager()
    {
        bgmAudio1.clip = bgm_map1;
        bgmAudio1.loop = true;
        bgmAudio1.Play();
        loginAudio.Stop();
        bgmAudio2.Stop();
    }
    public void Map2AudioManager()
    {
        bgmAudio2.clip = bgm_map2;
        bgmAudio2.loop = true;
        bgmAudio2.Play();
        loginAudio.Stop();
        bgmAudio1.Stop();
    }



}
