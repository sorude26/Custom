using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    Walk,
    Caterpillar,
    Floating,
}
public class PartsLeg : UnitParts
{
    [SerializeField]
    MoveType moveType;
    public MoveType UnitMoveType { get; private set; }
    [SerializeField]
    int movePower = 10;
    public int MovePower { get; private set; }

    [SerializeField]
    Transform partsHigh;
    void Start()
    {
        StartSet();
        MovePower = movePower;
        UnitMoveType = moveType;
    }
    private void Update()
    {
        if (partsBreak)
        {
            MovePower = 2;
            partsBreak = false;
        }
    }
    public Transform GetPartsHigh()
    {
        return partsHigh;
    }
}
