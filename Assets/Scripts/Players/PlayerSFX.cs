using System.Collections;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("SFX")]
    public AudioSource secondAudioSource;
    public void PlaySFX(AudioClip sfxClip,float sfxVolume)
    {
        secondAudioSource.clip = sfxClip;
        secondAudioSource.volume = sfxVolume;
        secondAudioSource.Play();
    }
}
