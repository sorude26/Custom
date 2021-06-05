using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ユニットの全パーツを保存するオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "UnitPartsData", menuName = "UnitPartsData")]
public class UnitPartsData : ScriptableObject
{
    [SerializeField] PartsHead[] HeadList;
    [SerializeField] PartsBody[] BodyList;
    [SerializeField] PartsLArm[] LArmList;
    [SerializeField] PartsRArm[] RArmList;
    [SerializeField] PartsLeg[] LegList;
    [SerializeField] Weapon[] WeaponList;
    public PartsHead GetHead(int ID) { return HeadList.ToList().Where(o => o.GetID() == ID ).FirstOrDefault(); }
    public PartsBody GetBody(int ID) { return BodyList.ToList().Where(o => o.GetID() == ID).FirstOrDefault(); }
    public PartsLArm GetLArm(int ID) { return LArmList.ToList().Where(o => o.GetID() == ID).FirstOrDefault(); }
    public PartsRArm GetRArm(int ID) { return RArmList.ToList().Where(o => o.GetID() == ID).FirstOrDefault(); }
    public PartsLeg GetLeg(int ID) { return LegList.ToList().Where(o => o.GetID() == ID).FirstOrDefault(); }
    public Weapon GetWeapon(int ID) { return WeaponList.ToList().Where(o => o.GetID() == ID).FirstOrDefault(); }
}
