﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDataGuide : MonoBehaviour
{
    [SerializeField]
    Text guideText;
    public void WritingGuide(StageID ID)
    {
        switch (ID)
        {
            case StageID.Stage0:
                guideText.text = "1";
                break;
            case StageID.Stage1:
                guideText.text = "平原：敵部隊の殲滅";
                break;
            case StageID.Stage2:
                guideText.text = "都市：敵部隊の殲滅";
                break;
            case StageID.Stage3:
                guideText.text = "4";
                break;
            default:
                break;
        }
    }
}