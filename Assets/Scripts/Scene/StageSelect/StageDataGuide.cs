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
                guideText.text = "平原：敵部隊の殲滅\n報酬：1500　出撃可能数：3\n平原に現れた敵部隊を殲滅せよ！";
                break;
            case StageID.Stage2:
                guideText.text = "都市：敵部隊の殲滅\n報酬：1500　出撃可能数：5\n都市部の敵部隊を殲滅せよ！";
                break;
            case StageID.Stage3:
                guideText.text = "森林地帯：森林横断任務\n報酬：2000　出撃可能数：4\n森林地帯を占拠する敵部隊の防衛を突破せよ！";
                break;
            case StageID.Stage4:
                guideText.text = "山岳基地：起動兵器ギガントの破壊\n報酬：5000　出撃可能数：5\n山岳基地を守る起動兵器ギガントを破壊せよ！";
                break;
            case StageID.Stage5:
                guideText.text = "平原：戦車部隊の撃滅\n報酬：2000　出撃可能数：4\n平原に敵の重戦車部隊が確認された、これを全て破壊せよ！";
                break;
            case StageID.Stage6:
                guideText.text = "平原：平原横断任務\n報酬：3000　出撃可能数：3\n平原に展開する敵部隊の包囲網を突破せよ！";
                break;
            case StageID.Stage7:
                guideText.text = "都市：対空砲装備部隊の撃滅\n報酬：3000　出撃可能数：5\n都市部にて対空砲装備部隊が確認された、これを全て破壊せよ！";
                break;
            case StageID.Stage8:
                guideText.text = "都市：敵重装部隊の殲滅\n報酬：1000　出撃可能数：4\n都市部に展開する敵重装甲部隊を殲滅せよ！";
                break;
            case StageID.Stage9:
                guideText.text = "森林地帯：敵部隊の殲滅\n報酬：2000　出撃可能数：4\n森林地帯で敵部隊に囲まれた、これを返り討ちにせよ！";
                break;
            case StageID.Stage10:
                guideText.text = "森林地帯：敵重装甲部隊の撃滅\n報酬：3000　出撃可能数：5\n森林地帯にて重装甲部隊が確認された、これを全て破壊せよ！";
                break;
            case StageID.Stage11:
                guideText.text = "山岳基地：大型起動兵器迎撃作戦\n報酬：1000　出撃可能数：4\n山岳基地にて大型起動兵器の襲撃を確認！迎撃せよ！";
                break;
            case StageID.Stage12:
                guideText.text = "山岳基地：基地からの脱出\n報酬：1000　出撃可能数：4\n山岳基地より脱出せよ！";
                break;
            default:
                break;
        }
    }
}
