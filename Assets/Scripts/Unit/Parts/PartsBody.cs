using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Human,
    Helicopter,
    Tank,
}
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
    [SerializeField]
    GameObject bodyHand;
    [SerializeField]
    Transform cameraPos = null;
    public int MovePower { get; private set; }
    
    [SerializeField] public UnitType unitType = UnitType.Human;
    void Start()
    {
        StartSet();
        UnitOutput = unitOutput;
        LiftingForce = liftingForce;
        MovePower = movePower;
        posY = transform.localPosition.y;
    }
    float posY;
    float posYtransform;
    int y = 1;
    
    private void Update()
    {
        if (unitType == UnitType.Helicopter)
        {
            if (partsBreak)
            {
                posYtransform -= 2.5f * Time.deltaTime;
                transform.localPosition = new Vector3(0, posY + posYtransform, 0);
                transform.Rotate(new Vector3(0, 0.5f, 0.02f));
            }
            else
            {
                if (posYtransform < 0)
                {
                    y = 1;
                }
                else if (posYtransform >= 0.2f)
                {
                    y = -1;
                }
                posYtransform += y * 0.2f * Time.deltaTime;
                transform.localPosition = new Vector3(0, posY + posYtransform, 0);
            }
        }
        
    }
    public override void Damage(int damage)
    {
        if (unitType == UnitType.Tank)
        {
            if(Owner.Head.CurrentPartsHp <= 0)
            {
                CurrentPartsHp = 0;
                PartsBreak();
                return;
            }
        }
        base.Damage(damage);
    }
    public Transform GetHeadPos() { return headParts; }
    public Transform GetRArmPos() { return rArmParts; }
    public Transform GetLArmPos() { return lArmParts; }
    public Transform GetBodyCentrer() { return bodyCenter; }
    public GameObject GetBodyHand() { return bodyHand; }
    public int GetUnitOutput() { return unitOutput; }
    public float GetLiftingForce() { return liftingForce; }
    public int GetMovePower() { return movePower; }
    public Transform GetCameraPos() { return cameraPos; }
    public void SetCameraPos(Transform pos) { cameraPos = pos; }
    public int GetUnitOverOutput() { return 0; }
}
