﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortieUI : MonoBehaviour
{
    [SerializeField]
    Text stageName;
    [SerializeField]
    Text totalPrice;
    [SerializeField]
    GameObject[] soriteUnitGuard;
    [SerializeField]
    GameObject[] soriteline;
    [SerializeField]
    GameObject[] soritelineGuard;
    [SerializeField]
    GameObject[] choiceUnitGuard;
    [SerializeField]
    GameObject[] choiceUnitMark;
    [SerializeField]
    GameObject soriteGuard;
    [SerializeField]
    GameObject changeMessage;
    [SerializeField]
    StageData stageData;
    [SerializeField]
    UnitPriceCalculator calculator;
    private bool readySorite = false;
    private int choiceUnit = -1;
    private int soriteNumber = 0;
    private int soritePos = -1;
    private int[] posData = new int[5];
    private void Start()
    {
        soriteGuard.SetActive(true);
        for (int i = 0; i < posData.Length; i++)
        {
            posData[i] = -1;
        }
        soriteNumber = stageData.GetPlayerNumber(GameManager.Instance.StageCode);
        foreach (GameObject guard in soritelineGuard)
        {           
            if (soriteNumber > 0)
            {
                guard.SetActive(false);
                soriteNumber--;
            }
            else
            {
                guard.SetActive(true);
            }
        }
        soriteNumber = stageData.GetPlayerNumber(GameManager.Instance.StageCode);
        foreach (GameObject guard in choiceUnitGuard)
        {
            if (soriteNumber > 0)
            {
                guard.SetActive(false);
                soriteNumber--;
            }
            else
            {
                guard.SetActive(true);
            }
        }
    }
    private void ChoiceReset()
    {
        foreach (GameObject item in choiceUnitMark)
        {
            item.SetActive(false);
        }
    }   
    private void SoriteLineReset()
    {
        foreach (GameObject item in soriteline)
        {
            item.SetActive(true);
        }
    }
    public void OnClickChoiceUnit1()
    {
        if (choiceUnit != 0)
        {
            ChoiceReset();
            SoriteLineReset();
            choiceUnit = 0;
            choiceUnitMark[0].SetActive(true);
        }
    }
    public void OnClickChoiceUnit2()
    {
        if (choiceUnit != 1)
        {
            ChoiceReset();
            SoriteLineReset();
            choiceUnit = 1;
            choiceUnitMark[1].SetActive(true);
        }
    }
    public void OnClickChoiceUnit3()
    {
        if (choiceUnit != 2)
        {
            ChoiceReset();
            SoriteLineReset();
            choiceUnit = 2;
            choiceUnitMark[2].SetActive(true);
        }
    }
    public void OnClickChoiceUnit4()
    {
        if (choiceUnit != 3)
        {
            ChoiceReset();
            SoriteLineReset();
            choiceUnit = 3;
            choiceUnitMark[3].SetActive(true);
        }
    }
    public void OnClickChoiceUnit5()
    {
        if (choiceUnit != 4)
        {
            ChoiceReset();
            SoriteLineReset();
            choiceUnit = 4;
            choiceUnitMark[4].SetActive(true);
        }
    }
    public void OnClickline1()
    {
        if (soritePos != 0)
        {
            SoriteLineReset();
            soritePos = 0;
            soriteline[0].SetActive(false);
        }
    }
    public void OnClickline2()
    {
        if (soritePos != 1)
        {
            SoriteLineReset();
            soritePos = 1;
            soriteline[1].SetActive(false);
        }
    }
    public void OnClickline3()
    {
        if (soritePos != 2)
        {
            SoriteLineReset();
            soritePos = 2;
            soriteline[2].SetActive(false);
        }
    }
    public void OnClickline4()
    {
        if (soritePos != 3)
        {
            SoriteLineReset();
            soritePos = 3;
            soriteline[3].SetActive(false);
        }
    }
    public void OnClickline5()
    {
        if (soritePos != 4)
        {
            SoriteLineReset();
            soritePos = 4;
            soriteline[4].SetActive(false);
        }
    }
    public void OnClickSetSoritUnit()
    {
        if (soritePos >= 0)
        {
            posData[soritePos] = choiceUnit;
        }
    }
    public void OnClickSorit()
    {
        changeMessage.SetActive(true);
    }
    public void OnClickCancel()
    {
        changeMessage.SetActive(false);
    }
    public void OnClickChangeScene()
    {
        if (readySorite)
        {
            GameManager.Instance.SetSortieUnit(posData);
        }
        else
        {
            GameManager.Instance.SceneChange(2);
        }
    }

}
