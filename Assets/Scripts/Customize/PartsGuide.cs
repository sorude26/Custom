using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsGuide : MonoBehaviour
{
    [SerializeField]
    Text guideText = null;

    public void Clear()
    {
        guideText.text = "";
    }
    public void HeadData(PartsHead head)
    {
        guideText.text = "部品名：" + head.GetName() + "\n耐久値：" + head.MaxPartsHp + "\n装甲値：" + head.ArmorDefense + "×" + head.ArmorPoint
            + "\n重量：" + head.Weight + "\nロックオン距離：" + head.DetectionRange + "\n所持数：";
    }

    public void BodyData(PartsBody body)
    {
        guideText.text = "部品名：" + body.GetName() + "\n耐久値：" + body.MaxPartsHp + "\n装甲値：" + body.ArmorDefense + "×" + body.ArmorPoint
            + "\n重量：" + body.Weight + "\n出力：" + body.UnitOutput + "\n昇降力補正：" + body.LiftingForce + "\n移動力補正：\n所持数：";

    }

    public void LRArmData(PartsLArm arm)
    {
        guideText.text = "部品名：" + arm.GetName() + "\n耐久値：" + arm.MaxPartsHp + "\n装甲値：" + arm.ArmorDefense + "×" + arm.ArmorPoint
           + "\n重量：" + arm.Weight + "\n所持数：";
    }
    public void LRArmData(PartsRArm arm)
    {
        guideText.text = "部品名：" + arm.GetName() + "\n耐久値：" + arm.MaxPartsHp + "\n装甲値：" + arm.ArmorDefense + "×" + arm.ArmorPoint
           + "\n重量：" + arm.Weight + "\n所持数：";
    }
    public void LegData(PartsLeg leg)
    {
        guideText.text = "部品名：" + leg.GetName() + "\n耐久値：" + leg.MaxPartsHp + "\n装甲値：" + leg.ArmorDefense + "×" + leg.ArmorPoint
            + "\n重量：" + leg.Weight + "\n移動力：" + leg.MovePower + "\n昇降力：\n所持数：";
    }

    public void WeaponData(Weapon weapon)
    {
        guideText.text = "武器名：" + weapon.GetName() + "武器種：" + weapon.Type + "重量：" + weapon.Weight + "攻撃力：" + weapon.Power + "×" + weapon.TotalShotNumber
            + "有効射程距離：" + weapon.EffectiveRange + "最大射程距離：" + weapon.Range + "所持数：";
    }
}
