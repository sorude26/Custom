using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGuide : MonoBehaviour
{
    [SerializeField]
    private int StageCode = 0;

    [SerializeField]
    StageData stageData;
    public void OnClickGuide()
    {
        stageData.GetData();
    }
}
