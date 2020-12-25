using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterPanel : MonoBehaviour
{
    [SerializeField] Image head = null;
    [SerializeField] Text headHp = null;
    [SerializeField] Image body = null;
    [SerializeField] Text bodyHp = null;
    [SerializeField] Image rArm = null;
    [SerializeField] Text rArmHp = null;
    [SerializeField] Image lArm = null;
    [SerializeField] Text lArmHp = null;
    [SerializeField] Image leg = null;
    [SerializeField] Text legHp = null;
    Unit unit = null;
    void Update()
    {
        if (unit != null)
        {
            head.fillAmount = (float)unit.Head.CurrentPartsHp / unit.Head.MaxPartsHp;
            headHp.text = "Head :" + unit.Head.CurrentPartsHp;
            body.fillAmount = (float)unit.Body.CurrentPartsHp / unit.Body.MaxPartsHp;
            bodyHp.text = "Body :" + unit.Body.CurrentPartsHp;
            rArm.fillAmount = (float)unit.RArm.CurrentPartsHp / unit.RArm.MaxPartsHp;
            rArmHp.text = "RArm:" + unit.RArm.CurrentPartsHp;
            lArm.fillAmount = (float)unit.LArm.CurrentPartsHp / unit.LArm.MaxPartsHp;
            lArmHp.text = "LArm:" + unit.LArm.CurrentPartsHp;
            leg.fillAmount = (float)unit.Leg.CurrentPartsHp / unit.Leg.MaxPartsHp;
            legHp.text = "Leg  :" + unit.Leg.CurrentPartsHp;
        }
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }
}
