using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceParts : MonoBehaviour
{
    [SerializeField]
    GameObject choiceView;
    [SerializeField]
    GameObject HeadList;
    [SerializeField]
    GameObject BodyList;
    [SerializeField]
    GameObject LArmList;
    [SerializeField]
    GameObject RArmList;
    [SerializeField]
    GameObject LegList;
    
    BaseStage baseStage;
    bool view = false;
    bool open = false;
    private void Start()
    {
        baseStage = BaseStage.Instance;
        choiceView.SetActive(false);
        HeadList.SetActive(false);
        BodyList.SetActive(false);
        LArmList.SetActive(false);
        RArmList.SetActive(false);
        LegList.SetActive(false);
    }
    private void Update()
    {
        if (baseStage.viewOpen && !view)
        {
            open = false;
            choiceView.SetActive(false);
            HeadList.SetActive(false);
            BodyList.SetActive(false);
            LArmList.SetActive(false);
            RArmList.SetActive(false);
            LegList.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        if (view)
        {
            view = false;
            baseStage.viewOpen = false;
            baseStage.SwithGard = true;
        }
    }
    public void OnClickView()
    {
        if (!open)
        {
            open = true;
            view = true;
            baseStage.viewOpen = true;
            choiceView.SetActive(true);
        }
        else
        {
            open = false;
            choiceView.SetActive(false);
            HeadList.SetActive(false);
            BodyList.SetActive(false);
            LArmList.SetActive(false);
            RArmList.SetActive(false);
            LegList.SetActive(false);
        }
    }
    public void OnClickHead()
    {
        HeadList.SetActive(true);
        BodyList.SetActive(false);
        LArmList.SetActive(false);
        RArmList.SetActive(false);
        LegList.SetActive(false);
        baseStage.SwithGard = true;
        baseStage.GuideHead();
    }
    public void OnClickBody()
    {
        HeadList.SetActive(false);
        BodyList.SetActive(true);
        LArmList.SetActive(false);
        RArmList.SetActive(false);
        LegList.SetActive(false);
        baseStage.SwithGard = true;
        baseStage.GuideBody();
    }
    public void OnClickLArm()
    {
        HeadList.SetActive(false);
        BodyList.SetActive(false);
        LArmList.SetActive(true);
        RArmList.SetActive(false);
        LegList.SetActive(false);
        baseStage.SwithGard = true;
        baseStage.GuideLArm();
    }
    public void OnClickRArm()
    {
        HeadList.SetActive(false);
        BodyList.SetActive(false);
        LArmList.SetActive(false);
        RArmList.SetActive(true);
        LegList.SetActive(false);
        baseStage.SwithGard = true;
        baseStage.GuideRArm();
    }
    public void OnClickLeg()
    {
        HeadList.SetActive(false);
        BodyList.SetActive(false);
        LArmList.SetActive(false);
        RArmList.SetActive(false);
        LegList.SetActive(true);
        baseStage.SwithGard = true;
        baseStage.GuideLeg();
    }
}
