using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static AudioClip clicksound, diesound, jumpsound, portalsound, skillsound;
    // Start is called before the first frame update
    static AudioSource audioSrc;
    void Start()
    {
        clicksound = Resources.Load<AudioClip>("click");
        diesound = Resources.Load<AudioClip>("die");
        jumpsound = Resources.Load<AudioClip>("jump");
        portalsound = Resources.Load<AudioClip>("portal");
        skillsound = Resources.Load<AudioClip>("skill");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "click":
                audioSrc.PlayOneShot(clicksound);
                break;
            case "die":
                audioSrc.PlayOneShot(diesound);
                break;
            case "jump":
                audioSrc.PlayOneShot(jumpsound);
                break;
            case "portal":
                audioSrc.PlayOneShot(portalsound);
                break;
            case "skill":
                audioSrc.PlayOneShot(skillsound);
                break;
        }
    }

}
