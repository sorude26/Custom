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
    [SerializeField] MoveType moveType;
    public MoveType UnitMoveType { get; private set; }
    /// <summary> 移動力 </summary>
    [SerializeField] int movePower = 10;
    public int MovePower { get; private set; }
    [SerializeField] float moveSpeed = 20;
    public float MoveSpeed { get; private set; }
    [SerializeField] GameObject partsHigh;
    /// <summary> 昇降力 </summary>
    [SerializeField] float liftingForce;
    /// <summary> 回避力 </summary>
    [SerializeField] int avoidance;
    public float LiftingForce { get; protected set; }
    [SerializeField] GameObject legJointL1;
    [SerializeField] GameObject legJointL2;
    [SerializeField] GameObject legJointR1;
    [SerializeField] GameObject legJointR2;
    float posY;
    float posYtransform;
    int y =1;
    void Start()
    {
        StartSet();
        MovePower = movePower;
        UnitMoveType = moveType;
        MoveSpeed = moveSpeed;
        LiftingForce = liftingForce;
        posY = partsHigh.transform.localPosition.y;
    }
    private void Update()
    {
        if (partsBreak)
        {
            MovePower = 3;
            MoveSpeed = 15;
            partsBreak = false;
        }
        /*
        if (posYtransform < 0)
        {
            y = 1;
        }
        else if (posYtransform >= 0.05f)
        {
            y = -1;
        }
        posYtransform += y * 0.1f * Time.deltaTime;
        partsHigh.transform.localPosition = new Vector3(0, posY + posYtransform, 0);
        */
    }
    public Transform GetPartsHigh()
    {
        return partsHigh.transform;
    }

    public MoveType GetMoveType() { return moveType; }
    public int GetMovePower() { return movePower; }
    public int GetAvoidance() { return avoidance; }
    public float GetLiftingForce() { return liftingForce; }
    public GameObject GetLegJointL1() { return legJointL1; }
    public GameObject GetLegJointL2() { return legJointL2; }
    public GameObject GetLegJointR1() { return legJointR1; }
    public GameObject GetLegJointR2() { return legJointR2; }
}
