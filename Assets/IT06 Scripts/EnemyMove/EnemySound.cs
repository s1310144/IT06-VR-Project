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
        audioSource.PlayOneShot(roarClip);
    }

    public void PlaySpin()
    {
        audioSource.PlayOneShot(spinClip);
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpClip);
    }

    public void PlayBeam()
    {
        audioSource.PlayOneShot(beamClip);
    }
}
