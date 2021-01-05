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

    }

    public void LRArmData(PartsLArm arm)
    {

    }

    public void LegData(PartsLeg leg)
    {

    }

    public void WeaponData(Weapon weapon)
    {

    }
}
