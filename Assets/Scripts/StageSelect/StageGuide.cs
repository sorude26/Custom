using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGuide : MonoBehaviour
{
    [SerializeField]    
    StageID stageCode;
    public void OnClickGuide()
    {
        StageDataGuide.Instance.WritingGuide(stageCode);
    }
}
