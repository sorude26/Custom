using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGuide : MonoBehaviour
{
    [SerializeField]    
    StageID stageCode;
    
    public void OnClickGuide()
    {
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
        StageSelectUI.Instance.WritingGuide(stageCode);
    }
    public void OnClickSortie()
    {
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
        StageSelectUI.Instance.OpenMessage(stageCode);
    }
}
