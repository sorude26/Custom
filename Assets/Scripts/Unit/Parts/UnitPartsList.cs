using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPartsList : MonoBehaviour
{
    public static UnitPartsList Instance { get; private set; }
    [SerializeField]
    List<GameObject> HeadObjectList;
    [SerializeField]
    List<GameObject> BodyObjectList;
    [SerializeField]
    List<GameObject> LArmObjectList;
    [SerializeField]
    List<GameObject> RArmObjectList;
    [SerializeField]
    List<GameObject> LegObjectList;
    [SerializeField]
    List<GameObject> weaponObjectList;

    private void Awake()
    {
        Instance = this;
    }

    public int GetPartsHeadCount() { return HeadObjectList.Count; }
    public int GetPartsBodyCount() { return BodyObjectList.Count; }
    public int GetPartsLArmCount() { return LArmObjectList.Count; }
    public int GetPartsRArmCount() { return RArmObjectList.Count; }
    public int GetPartsLegCount() { return LegObjectList.Count; }
    public int GetWeaponCount() { return weaponObjectList.Count; }
    public GameObject GetHeadObject(int i) { return HeadObjectList[i]; }
    public GameObject GetBodyObject(int i) { return BodyObjectList[i]; }
    public GameObject GetLArmObject(int i) { return LArmObjectList[i]; }
    public GameObject GetRArmObject(int i) { return RArmObjectList[i]; }
    public GameObject GetLegObject(int i) { return LegObjectList[i]; }
    public GameObject GetWeaponObject(int i) { return weaponObjectList[i]; }

}
