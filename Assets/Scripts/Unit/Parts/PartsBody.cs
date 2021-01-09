using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsBody : UnitParts
{
    [SerializeField]
    int unitOutput;
    public int UnitOutput { get; protected set; }
    [SerializeField]
    float liftingForce;
    public float LiftingForce { get; protected set; }
    [SerializeField]
    Transform headParts;
    [SerializeField]
    Transform rArmParts;
    [SerializeField]
    Transform lArmParts;
    [SerializeField]
    Transform bodyCenter;
    [SerializeField]
    int movePower = 10;
    public int MovePower { get; private set; }
    void Start()
    {
        StartSet();
        UnitOutput = unitOutput;
        LiftingForce = liftingForce;
    }

    public Transform GetHeadPos() { return headParts; }
    public Transform GetRArmPos() { return rArmParts; }
    public Transform GetLArmPos() { return lArmParts; }
    public Transform GetBodyCentrer() { return bodyCenter; }
    public int GetUnitOutput() { return unitOutput; }
    public float GetLiftingForce() { return liftingForce; }
}
