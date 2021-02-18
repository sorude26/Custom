using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShowMode
{
    All,
    Sortie,
}
public class UnitShowBase : MonoBehaviour
{
    [SerializeField]
    private ShowMode showMode = ShowMode.All;
    [SerializeField]
    Transform[] unitPos;
    UnitPartsList partsList = null;
    public PartsHead Head { get; private set; } = null;
    public int HeadID { get; private set; }
    public PartsBody Body { get; private set; } = null;
    public int BodyID { get; private set; }
    public PartsLArm LArm { get; private set; } = null;
    public int LArmID { get; private set; }
    public PartsRArm RArm { get; private set; } = null;
    public int RArmID { get; private set; }
    public PartsLeg Leg { get; private set; } = null;
    public int LegID { get; private set; }
    public Weapon LArmWeapon { get; private set; } = null;
    public int WeaponLID { get; private set; }
    public Weapon RArmWeapon { get; private set; } = null;
    private int count = 0;

    void Start()
    {
        partsList = UnitPartsList.Instance;
        switch (showMode)
        {
            case ShowMode.All:
                AllBuild();
                break;
            case ShowMode.Sortie:
                SortieBuild();
                break;
            default:
                break;
        }
    }

    private void AllBuild()
    {
        foreach (var unit in GameManager.UnitDatas)
        {
            BuildUnit(unit.HeadID, unit.BodyID, unit.LArmID, unit.WeaponLID, unit.RArmID, unit.WeaponRID, unit.LegID, unitPos[count]);
            count++;
        }
    }
    private void SortieBuild()
    {
        foreach (var unit in GameManager.SortieUnit)
        {
            if (unit.Sortie)
            {
                BuildUnit(unit.HeadID, unit.BodyID, unit.LArmID, unit.WeaponLID, unit.RArmID, unit.WeaponRID, unit.LegID, unitPos[count]);
                count++;
            }
        }
    }
    
    public void BuildUnit(int headID, int bodyID, int lArmID, int weaponLID, int rArmID, int weaponRID, int legID, Transform pos)
    {
        //pos.rotation = Quaternion.Euler(0, 0, 0);//パーツ生成時に向きを合わせる
        GameObject leg = Instantiate(partsList.GetLegObject(legID));
        leg.transform.position = pos.position;
        leg.transform.parent = pos;
        Leg = leg.GetComponent<PartsLeg>();
        GameObject body = Instantiate(partsList.GetBodyObject(bodyID));
        body.transform.parent = Leg.transform;
        Body = body.GetComponent<PartsBody>();
        Body.TransFormParts(Leg.GetPartsHigh().position);
        GameObject head = Instantiate(partsList.GetHeadObject(headID));
        head.transform.parent = Body.transform;
        Head = head.GetComponent<PartsHead>();
        Head.TransFormParts(Body.GetHeadPos().position);
        GameObject lArm = Instantiate(partsList.GetLArmObject(lArmID));
        lArm.transform.parent = Body.transform;
        LArm = lArm.GetComponent<PartsLArm>();
        LArm.TransFormParts(Body.GetLArmPos().position);
        GameObject weaponL = Instantiate(partsList.GetWeaponObject(weaponLID));
        weaponL.transform.parent = LArm.ArmParts().transform;
        LArmWeapon = weaponL.GetComponent<Weapon>();
        LArmWeapon.TransFormParts(LArm.GetGrip().position);
        GameObject rArm = Instantiate(partsList.GetRArmObject(rArmID));
        rArm.transform.parent = Body.transform;
        RArm = rArm.GetComponent<PartsRArm>();
        RArm.TransFormParts(Body.GetRArmPos().position);
        GameObject weaponR = Instantiate(partsList.GetWeaponObject(weaponRID));
        weaponR.transform.parent = RArm.ArmParts().transform;
        RArmWeapon = weaponR.GetComponent<Weapon>();
        RArmWeapon.TransFormParts(RArm.GetGrip().position);
    }
}
