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
        guideText.text = "部品名：" + head.GetName() + "\n耐久値：" + head.GetMaxHP() + "\n装甲値：" + head.GetArmorDefense() + "×" + head.GetArmorPoint()
            + "\n重量：" + head.GetWeight() + "\nロックオン距離：" + head.GetDetectionRange() + "\n所持数：";
    }
    public void BodyData(PartsBody body)
    {
        guideText.text = "部品名：" + body.GetName() + "\n耐久値：" + body.GetMaxHP() + "\n装甲値：" + body.GetArmorDefense() + "×" + body.GetArmorPoint()
            + "\n重量：" + body.GetWeight() + "\n出力：" + body.GetUnitOutput() + "\n昇降力補正：" + body.GetLiftingForce() + "\n移動力補正：\n所持数：";
    }
    public void LRArmData(PartsLArm arm)
    {
        guideText.text = "部品名：" + arm.GetName() + "\n耐久値：" + arm.GetMaxHP() + "\n装甲値：" + arm.GetArmorDefense() + "×" + arm.GetArmorPoint()
           + "\n重量：" + arm.GetWeight() + "\n所持数：";
    }
    public void LRArmData(PartsRArm arm)
    {
        guideText.text = "部品名：" + arm.GetName() + "\n耐久値：" + arm.GetMaxHP() + "\n装甲値：" + arm.GetArmorDefense() + "×" + arm.GetArmorPoint()
           + "\n重量：" + arm.GetWeight() + "\n所持数：";
    }
    public void LegData(PartsLeg leg)
    {
        guideText.text = "部品名：" + leg.GetName() + "\n耐久値：" + leg.GetMaxHP() + "\n装甲値：" + leg.GetArmorDefense() + "×" + leg.GetArmorPoint()
            + "\n重量：" + leg.GetWeight() + "\n移動力：" + leg.GetMovePower() + "\n昇降力：\n所持数：";
    }
    public void WeaponData(Weapon weapon)
    {
        guideText.text = "武器名：" + weapon.GetName() + "\n武器種：" + weapon.GetWeaponType() + "\n攻撃力：" + weapon.GetPower() + "×" + weapon.GetShotNumber()
            + "\n有効射程距離：" + weapon.GetEffectiveRange() + "\n最大射程距離：" + weapon.GetRange() + "\n重量：" + weapon.GetWeight() + "\n所持数：";
    }
    public void AttackWeapon(Weapon weapon, Unit Attacker, Unit target)
    {
        string messege = "射程外";
        Vector3 dir = target.transform.position - Attacker.transform.position;
        if (dir.sqrMagnitude <= weapon.GetRange() * weapon.GetRange())
        {
            if (dir.sqrMagnitude <= weapon.GetEffectiveRange() * weapon.GetEffectiveRange())
            {
                messege = "有効射程内";
            }
            else
            {
                messege = "射程内";
            }
        }
        guideText.text = "武器名：" + weapon.GetName() + "\n武器種：" + weapon.GetWeaponType() + "\n攻撃力：" + weapon.GetPower() + "×" + weapon.GetShotNumber()
            + "\n有効射程距離：" + weapon.GetEffectiveRange() + "\n最大射程距離：" + weapon.GetRange() + "\n" + messege;
    }
}
