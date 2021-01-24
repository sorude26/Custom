using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortieUI : MonoBehaviour
{
    [SerializeField]
    Text StageName;
    [SerializeField]
    Text totalPrice;
    [SerializeField]
    List<GameObject> soriteUnitGurd;
    [SerializeField]
    List<GameObject> soriteline;
    [SerializeField]
    List<GameObject> soritelineGurd;
    [SerializeField]
    List<GameObject> choiceUnitGurd;
    [SerializeField]
    List<GameObject> choiceUnitMark;
    [SerializeField]
    GameObject soriteGurd;
    [SerializeField]
    GameObject changeMessage;
    [SerializeField]
    StageData stageData;
    [SerializeField]
    UnitPriceCalculator calculator;
    private bool readySorite = false;
    private int choiceUnit = 0;
    private int soritePos = 0;
    private void Start()
    {
        soriteGurd.SetActive(true);
    }
    public void OnClickChoiceUnit1()
    {
        choiceUnit = 0;
    }
    public void OnClickline1()
    {
        soritePos = 0;
    }
    public void OnClickSorit()
    {
        changeMessage.SetActive(false);
    }
    public void Cancel()
    {

    }
    public void ChangeScene()
    {
        if (true)
        {

        }
        else
        {

        }
    }
}
