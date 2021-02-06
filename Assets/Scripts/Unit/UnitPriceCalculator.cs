using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPriceCalculator : MonoBehaviour
{
    UnitPartsList unitPartsList;
    private void Start()
    {
        unitPartsList = UnitPartsList.Instance;
    }
    public int GetPraice(int unitNumber)
    {
        int price = 0;
        GameManager.PlayerUnitData unitData = GameManager.UnitDatas[unitNumber];
        price += unitPartsList.GetHeadObject(unitData.HeadID).GetComponent<PartsHead>().GetPrice();
        price += unitPartsList.GetBodyObject(unitData.BodyID).GetComponent<PartsBody>().GetPrice();
        price += unitPartsList.GetLArmObject(unitData.LArmID).GetComponent<PartsLArm>().GetPrice();
        price += unitPartsList.GetRArmObject(unitData.RArmID).GetComponent<PartsRArm>().GetPrice();
        price += unitPartsList.GetLegObject(unitData.LegID).GetComponent<PartsLeg>().GetPrice();
        price += unitPartsList.GetWeaponObject(unitData.WeaponLID).GetComponent<Weapon>().GetPrice();
        price += unitPartsList.GetWeaponObject(unitData.WeaponRID).GetComponent<Weapon>().GetPrice();
        return price;
    }
}
