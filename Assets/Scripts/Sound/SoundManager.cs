using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    None,   // 未定義
    Title,
    Base,
    Sortie,
    Customize,
    Result,
    Ending,
    Stage1p,
    Stage1e,
}
public enum SEType
{
    None,   // 未定義
    ClickBotton,
    MoveWalk,
    MoveTank,
    MoveHelicopter,
    Break,
    Shot1,
    Shot2,
    Shot3,
    Explosion1,
    Explosion2,
    Explosion3,
    Hit,
    Attack,
}
public class SoundManager : MonoBehaviour
{
    static public SoundManager Instance { get; private set; }
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] soundBGM;
    [SerializeField]
    private AudioClip[] soundEffect;
    Dictionary<BGMType, AudioClip> soundBGMList = new Dictionary<BGMType, AudioClip>();
    Dictionary<SEType, AudioClip> soundSEList = new Dictionary<SEType, AudioClip>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < soundBGM.Length; i++)
        {
            var soundID = (BGMType)(i + 1);
            soundBGMList.Add(soundID, soundBGM[i]);
        }
        for (int i = 0; i < soundEffect.Length; i++)
        {
            var soundID = (SEType)(i + 1);
            soundSEList.Add(soundID, soundEffect[i]);
        }
    }
    private void Start()
    {
        PlayBGM(BGMType.Title);
    }
    public void PlayBGM(BGMType type) 
    {
        audioSource.clip = soundBGMList[type];
        audioSource.Play();
        audioSource.loop = true;
    }

    public void PlaySE(SEType type)
    {
        audioSource.PlayOneShot(soundSEList[type]);
    }
}
