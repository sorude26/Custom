using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
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
}
public class SoundManager : MonoBehaviour
{
    static public SoundManager Instance { get; private set; }
    private AudioSource audioSource;
    private AudioClip[] soundBGM;
    private AudioClip[] soundEffect;
    Dictionary<BGMType, List<AudioClip>> soundBGMList = new Dictionary<BGMType, List<AudioClip>>();
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
    }
}
