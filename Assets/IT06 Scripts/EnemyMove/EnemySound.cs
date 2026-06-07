using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip roarClip;
    public AudioClip spinClip;
    public AudioClip jumpClip;
    public AudioClip beamClip;

    public void PlayRoar()
    {
        Debug.Log("Play Roar sound / 咆哮　再生");
        audioSource.PlayOneShot(roarClip);
    }

    public void PlaySpin()
    {
        Debug.Log("Play Spin sound / 回転　再生");
        audioSource.PlayOneShot(spinClip);
    }

    public void PlayJump()
    {
        Debug.Log("Play Jump sound / ジャンプ　再生");
        audioSource.PlayOneShot(jumpClip);
    }

    public void PlayBeam()
    {
        Debug.Log("Play Beam sound / ビーム　再生");
        audioSource.PlayOneShot(beamClip);
    }
}
