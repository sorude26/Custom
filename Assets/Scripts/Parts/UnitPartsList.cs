using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPartsList : MonoBehaviour
{
    public static UnitPartsList Instance { get; private set; }
    [SerializeField]
    List<PartsHead> HeadList;
    [SerializeField]
    List<GameObject> HeadObjectList;
    [SerializeField]
    List<PartsBody> BodyList;
    [SerializeField]
    List<GameObject> BodyObjectList;
    [SerializeField]
    List<PartsLArm> LArmsList;
    [SerializeField]
    List<GameObject> LArmObjectList;
    [SerializeField]
    List<PartsRArm> RArmsList;
    [SerializeField]
    List<GameObject> RArmObjectList;
    [SerializeField]
    List<PartsLeg> LegList;
    [SerializeField]
    List<GameObject> LegObjectList;
    [SerializeField]
    List<Weapon> weaponsList;
    [SerializeField]
    List<GameObject> weaponObjectList;

    private void Awake()
    {
        Instance = this;
    }

    public PartsHead GetPartsHead(int i) { return HeadList[i]; }
    public PartsBody GetPartsBody(int i) { return BodyList[i]; }
    public PartsLArm GetPartsLArm(int i) { return LArmsList[i]; }
    public PartsRArm GetPartsRArm(int i) { return RArmsList[i]; }
    public PartsLeg GetPartsLeg(int i) { return LegList[i]; }
    public Weapon GetWeapon(int i) { return weaponsList[i]; }
    public GameObject GetHeadObject(int i) { return HeadObjectList[i]; }
    public GameObject GetBodyObject(int i) { return BodyObjectList[i]; }
    public GameObject GetLArmObject(int i) { return LArmObjectList[i]; }
    public GameObject GetRArmObject(int i) { return RArmObjectList[i]; }
    public GameObject GetLegObject(int i) { return LegObjectList[i]; }
    public GameObject GetWeaponObject(int i) { return weaponObjectList[i]; }

}
