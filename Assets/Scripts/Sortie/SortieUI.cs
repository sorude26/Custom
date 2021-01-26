using System.Collections;
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
            choiceUnit = 0;
            choiceUnitMark[0].SetActive(true);
        }
    }
    public void OnClickChoiceUnit2()
    {
        choiceUnit = 1;
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
    public void OnClickSetSoritUnit()
    {
        if (choiceUnit >= 0 && soritePos >= 0)
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
