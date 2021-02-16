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
    RectTransform[] soriteUnitMark;
    [SerializeField]
    GameObject[] soriteline;
    [SerializeField]
    GameObject[] soritelineGuard;
    [SerializeField]
    GameObject[] choiceUnitGuard;
    [SerializeField]
    GameObject[] choiceUnitMark;
    [SerializeField]
    GameObject choiceResetMark;
    [SerializeField]
    Text[] soriteUnitData;
    [SerializeField]
    GameObject soriteGuard;
    [SerializeField]
    GameObject changeMessage;
    [SerializeField]
    StageData stageData;
    [SerializeField]
    UnitPriceCalculator calculator;
    [SerializeField]
    Image[] soriteUnitImage;
    private bool readySorite = false;
    private bool ready = false;
    private int choiceUnit = -1;
    private bool[] choice = new bool[5];
    private int choideNumber = 0;
    private int soriteNumber = 0;
    private int soritePos = -1;
    private int[] posData = new int[5];
    private void Start()
    {
        GameManager.Instance.ResetSortieUnit();
        stageName.text = stageData.GetStageName(GameManager.StageCode);
        soriteGuard.SetActive(true);
        changeMessage.SetActive(false);
        choiceResetMark.SetActive(false);
        for (int i = 0; i < posData.Length; i++)
        {
            posData[i] = -2;
            choice[i] = false;
        }
        soriteNumber = stageData.GetPlayerNumber(GameManager.StageCode);
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
        foreach (GameObject guard in choiceUnitGuard)
        {
            guard.SetActive(true);
        }
        choideNumber = 5;
        for (int i = 0; i < choideNumber; i++)
        {
            choiceUnitGuard[i].SetActive(false);
            choice[i] = true;
        }
        foreach (Text unitData in soriteUnitData)
        {
            unitData.text = "";
        }
        foreach (Image item in soriteUnitImage)
        {
            item.rectTransform.anchoredPosition = new Vector3(0, 1000, 0);
        }
        totalPrice.text = "" + 0;
    }
    private void ChoiceReset()
    {
        SoundManager.Instance.PlaySE(SEType.ChoiceUnit);
        foreach (GameObject item in choiceUnitMark)
        {
            item.SetActive(false);
        }
        choiceResetMark.SetActive(false);
    }   
    private void SoriteLineReset()
    {        
        soritePos = -1;
        foreach (GameObject item in soriteline)
        {
            item.SetActive(true);
        }
    }
    public void OnClickChoiceUnit1()
    {
        if (choiceUnit != 0 && choice[0])
        {
            ChoiceReset();
            SoriteLineReset();            
            choiceUnit = 0;
            choiceUnitMark[0].SetActive(true);
        }
    }
    public void OnClickChoiceUnit2()
    {
        if (choiceUnit != 1 && choice[1])
        {
            ChoiceReset();
            SoriteLineReset();
            choiceUnit = 1;
            choiceUnitMark[1].SetActive(true);
        }
    }
    public void OnClickChoiceUnit3()
    {
        if (choiceUnit != 2 && choice[2])
        {
            ChoiceReset();
            SoriteLineReset();
            choiceUnit = 2;
            choiceUnitMark[2].SetActive(true);
        }
    }
    public void OnClickChoiceUnit4()
    {
        if (choiceUnit != 3 && choice[3])
        {
            ChoiceReset();
            SoriteLineReset();
            choiceUnit = 3;
            choiceUnitMark[3].SetActive(true);
        }
    }
    public void OnClickChoiceUnit5()
    {
        if (choiceUnit != 4 && choice[3])
        {
            ChoiceReset();
            SoriteLineReset();
            choiceUnit = 4;
            choiceUnitMark[4].SetActive(true);
        }
    }
    public void OnClickChoiceReset()
    {
        if (choiceUnit != -1)
        {
            ChoiceReset();
            SoriteLineReset();
            choiceUnit = -1;
            choiceResetMark.SetActive(true);
        }
    }
    public void OnClickline1()
    {
        if (soritePos != 0)
        {
            SoriteLineReset();
            soritePos = 0;
            soriteline[0].SetActive(false);
            OnClickSetSoritUnit();
        }
    }
    public void OnClickline2()
    {
        if (soritePos != 1)
        {
            SoriteLineReset();
            soritePos = 1;
            soriteline[1].SetActive(false);
            OnClickSetSoritUnit();
        }
    }
    public void OnClickline3()
    {
        if (soritePos != 2)
        {
            SoriteLineReset();
            soritePos = 2;
            soriteline[2].SetActive(false);
            OnClickSetSoritUnit();
        }
    }
    public void OnClickline4()
    {
        if (soritePos != 3)
        {
            SoriteLineReset();
            soritePos = 3;
            soriteline[3].SetActive(false);
            OnClickSetSoritUnit();
        }
    }
    public void OnClickline5()
    {
        if (soritePos != 4)
        {
            SoriteLineReset();
            soritePos = 4;
            soriteline[4].SetActive(false);
            OnClickSetSoritUnit();
        }
    }
    public void OnClickSetSoritUnit()
    {
        SoundManager.Instance.PlaySE(SEType.ClickBotton);
        if (soritePos >= 0)
        {
            if (choiceUnit >= 0)
            {
                for (int i = 0; i < posData.Length; i++)
                {
                    if (posData[i] == choiceUnit)
                    {
                        if (i != soritePos)
                        {
                            posData[i] = posData[soritePos];
                            break;
                        }                        
                    }
                }
                posData[soritePos] = choiceUnit;
            }
            else
            {
                if (posData[soritePos] >= 0)
                {
                    posData[soritePos] = choiceUnit;
                }
            }
            for (int i = 0; i < soriteUnitImage.Length; i++)
            {
                bool check = true;
                for (int j = 0; j < posData.Length; j++)
                {
                    if (posData[j] == i)
                    {
                        soriteUnitImage[i].rectTransform.anchoredPosition = soriteUnitMark[j].anchoredPosition;
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    soriteUnitImage[i].rectTransform.anchoredPosition = new Vector3(0, 1000, 0);
                }
            }
            
            foreach (var item in posData)
            {
                if (item >= 0)
                {
                    soriteGuard.SetActive(false);
                    ready = true;
                    return;
                }
            }
            soriteGuard.SetActive(true);
            ready = false;            
        }
    }
    public void OnClickSorit()
    {
        if (ready)
        {
            changeMessage.SetActive(true);
            readySorite = true;
            SoundManager.Instance.PlaySE(SEType.ChoiceButton);
        }
    }
    public void OnClickCancel()
    {
        changeMessage.SetActive(false);
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
    }
    public void OnClickReturn()
    {
        changeMessage.SetActive(true);
        readySorite = false;
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
    }
    public void OnClickChangeScene()
    {
        if (readySorite)
        {
            GameManager.Instance.SetSortieUnit(posData);
        }
        else
        {
            GameManager.Instance.StartChange(2);
        }
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
    }

}
