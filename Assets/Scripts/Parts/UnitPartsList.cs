using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPartsList : MonoBehaviour
{
    public static UnitPartsList Instanse { get; private set; }
    [SerializeField]
    List<PartsHead> HeadList;
    [SerializeField]
    List<PartsBody> BodyList;
    [SerializeField]
    List<PartsLArm> LArmsList;
    [SerializeField]
    List<PartsRArm> RArmsList;
    [SerializeField]
    List<PartsLeg> LegList;

    private void Awake()
    {
        Instanse = this;
    }

    public PartsHead GetPartsHead(int i) { return HeadList[i]; }
    public PartsBody GetPartsBody(int i) { return BodyList[i]; }
    public PartsLArm GetPartsLArm(int i) { return LArmsList[i]; }
    public PartsRArm GetPartsRArm(int i) { return RArmsList[i]; }
    public PartsLeg GetPartsLeg(int i) { return LegList[i]; }
}
