using System.Collections;
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
                guideText.text = "森林地帯：森林横断任務";
                break;
            case StageID.Stage4:
                guideText.text = "山岳基地：大型起動兵器ギガントの破壊";
                break;
            case StageID.Stage5:
                guideText.text = "平原：戦車部隊の殲滅";
                break;
            case StageID.Stage6:
                guideText.text = "平原：平原横断任務";
                break;
            case StageID.Stage7:
                guideText.text = "都市：対空砲装備部隊の撃滅";
                break;
            case StageID.Stage8:
                guideText.text = "都市：敵重装部隊の殲滅";
                break;
            case StageID.Stage9:
                break;
            case StageID.Stage10:
                break;
            case StageID.Stage11:
                break;
            case StageID.Stage12:
                break;
            default:
                break;
        }
    }
}
