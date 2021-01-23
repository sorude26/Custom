using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGuide : MonoBehaviour
{
    [SerializeField]    
    StageID stageCode;
    private void Start()
    {
        if (!GameManager.Instance.GetStageFlag(stageCode))
        {

        }
    }
    public void OnClickGuide()
    {
        StageSelectUI.Instance.WritingGuide(stageCode);
    }
    public void OnClickSortie()
    {
        StageSelectUI.Instance.OpenMessage(stageCode);
    }
}
