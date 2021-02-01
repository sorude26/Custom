using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]
    AudioData audioData;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGBM()
    {
        audioSource.PlayOneShot(audioData.GetBGM(0));
    }
}
