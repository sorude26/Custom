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
    [SerializeField]
    GameObject HeadGard;
    [SerializeField]
    GameObject BodyGard;
    [SerializeField]
    GameObject LArmGard;
    [SerializeField]
    GameObject RArmGard;
    [SerializeField]
    GameObject LegGard;
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
        HeadGard.SetActive(false);
        BodyGard.SetActive(false);
        LArmGard.SetActive(false);
        RArmGard.SetActive(false);
        LegGard.SetActive(false);
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
            HeadGard.SetActive(false);
            BodyGard.SetActive(false);
            LArmGard.SetActive(false);
            RArmGard.SetActive(false);
            LegGard.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        if (view)
        {
            SoundManager.Instance.PlaySE(SEType.ChoiceButton);
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
            HeadGard.SetActive(false);
            BodyGard.SetActive(false);
            LArmGard.SetActive(false);
            RArmGard.SetActive(false);
            LegGard.SetActive(false);
        }
    }
    public void OnClickHead()
    {
        HeadList.SetActive(true);
        BodyList.SetActive(false);
        LArmList.SetActive(false);
        RArmList.SetActive(false);
        LegList.SetActive(false);
        HeadGard.SetActive(true);
        BodyGard.SetActive(false);
        LArmGard.SetActive(false);
        RArmGard.SetActive(false);
        LegGard.SetActive(false);
        baseStage.SwithGard = true;
        baseStage.GuideHead();
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
    }
    public void OnClickBody()
    {
        HeadList.SetActive(false);
        BodyList.SetActive(true);
        LArmList.SetActive(false);
        RArmList.SetActive(false);
        LegList.SetActive(false);
        HeadGard.SetActive(false);
        BodyGard.SetActive(true);
        LArmGard.SetActive(false);
        RArmGard.SetActive(false);
        LegGard.SetActive(false);
        baseStage.SwithGard = true;
        baseStage.GuideBody();
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
    }
    public void OnClickLArm()
    {
        HeadList.SetActive(false);
        BodyList.SetActive(false);
        LArmList.SetActive(true);
        RArmList.SetActive(false);
        LegList.SetActive(false);
        HeadGard.SetActive(false);
        BodyGard.SetActive(false);
        LArmGard.SetActive(true);
        RArmGard.SetActive(false);
        LegGard.SetActive(false);
        baseStage.SwithGard = true;
        baseStage.GuideLArm();
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
    }
    public void OnClickRArm()
    {
        HeadList.SetActive(false);
        BodyList.SetActive(false);
        LArmList.SetActive(false);
        RArmList.SetActive(true);
        LegList.SetActive(false);
        HeadGard.SetActive(false);
        BodyGard.SetActive(false);
        LArmGard.SetActive(false);
        RArmGard.SetActive(true);
        LegGard.SetActive(false);
        baseStage.SwithGard = true;
        baseStage.GuideRArm();
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
    }
    public void OnClickLeg()
    {
        HeadList.SetActive(false);
        BodyList.SetActive(false);
        LArmList.SetActive(false);
        RArmList.SetActive(false);
        LegList.SetActive(true);
        HeadGard.SetActive(false);
        BodyGard.SetActive(false);
        LArmGard.SetActive(false);
        RArmGard.SetActive(false);
        LegGard.SetActive(true);
        baseStage.SwithGard = true;
        baseStage.GuideLeg();
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
    }
}
