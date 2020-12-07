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
    void Start()
    {
        StartSet();
        UnitOutput = unitOutput;
        LiftingForce = liftingForce;
    }
}
