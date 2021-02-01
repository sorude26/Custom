using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{

}
public enum SEType
{

}
public class AudioData : MonoBehaviour
{
    [SerializeField] AudioClip[] BGMdata;
    [SerializeField] AudioClip[] SEdata;
    public static AudioData Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }
     
    public AudioClip GetBGM(int i) 
    {
        return BGMdata[i];
    }
    public AudioClip GetSE(int i)
    {
        return BGMdata[i];
    }
}
