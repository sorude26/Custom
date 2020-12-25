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
            body.fillAmount = (float)unit.Body.CurrentPartsHp / unit.Body.MaxPartsHp;
            rArm.fillAmount = (float)unit.RArm.CurrentPartsHp / unit.RArm.MaxPartsHp;
            lArm.fillAmount = (float)unit.LArm.CurrentPartsHp / unit.LArm.MaxPartsHp;
            leg.fillAmount = (float)unit.Leg.CurrentPartsHp / unit.Leg.MaxPartsHp;
        }
    }

    public void SetUnit(Unit unit) 
    {
        this.unit = unit;
    }
}
