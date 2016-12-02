using UnityEngine;
using System.Collections;

public class FirstPersonSFX : MonoBehaviour {

    public AudioSource asPlayerVoice;
    public AudioSource asGunShoot;
    public AudioClip[] aPlayerHurtGroans;
    public AudioClip aGunShoot;
    
    public void PlayHurtGroan()
    {
        if (!asPlayerVoice.isPlaying || asPlayerVoice.time < 1.0f)
        {
            int iRandom = Random.Range(0, aPlayerHurtGroans.Length);
            asPlayerVoice.clip = aPlayerHurtGroans[iRandom];

            asPlayerVoice.Play();
        }
    }

    public void PlayGunShot()
    {
        asGunShoot.clip = aGunShoot;
        asGunShoot.Play();
    }
}
