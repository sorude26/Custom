﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectUI : MonoBehaviour
{
    public static StageSelectUI Instance { get; private set; }
    [SerializeField]
    private StageDataGuide dataGuide;
    private StageID stageID;
    [SerializeField]
    GameObject massge;
    [SerializeField]
    List<GameObject> stageList;
    private bool returnBase = false;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        massge.SetActive(false);
        foreach (GameObject stage in stageList)
        {
            stage.SetActive(false);
        }
    }
    public void OpenMessage(StageID ID)
    {
        stageID = ID;
        WritingGuide(ID);
        massge.SetActive(true);
        returnBase = false;
    }
    public void WritingGuide(StageID ID)
    {
        dataGuide.WritingGuide(ID);
    }
    public void OnClickCancel()
    {
        massge.SetActive(false);
        returnBase = false;
    }
    public void OnClickReturnBase()
    {
        massge.SetActive(true);
        returnBase = true;
    }
    public void OnClickSceneChange()
    {
        if (returnBase)
        {
            GameManager.Instance.StartChange(3);
        }
        else
        {
            GameManager.Instance.SetStageID(stageID);
        }       
    }
    public void OnClickStage1()
    {
        foreach (GameObject stage in stageList)
        {
            stage.SetActive(false);
        }
        stageList[0].SetActive(true);
    }
    public void OnClickStage2()
    {
        foreach (GameObject stage in stageList)
        {
            stage.SetActive(false);
        }
        stageList[1].SetActive(true);
    }
    public void OnClickStage3()
    {
        foreach (GameObject stage in stageList)
        {
            stage.SetActive(false);
        }
        stageList[2].SetActive(true);
    }
    public void OnClickStage4()
    {
        foreach (GameObject stage in stageList)
        {
            stage.SetActive(false);
        }
        stageList[3].SetActive(true);
    }
}