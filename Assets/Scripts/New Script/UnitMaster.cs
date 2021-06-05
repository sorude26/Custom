using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMaster : MonoBehaviour
{
    public int BodyPartsHp { get; protected set; }
    public int Avoidance { get; protected set; }
    public int HitAccuracy { get; protected set; }

    protected PartsBody body;
    protected PartsHead head;
    protected PartsLArm lArm;
    protected PartsRArm rArm;
    protected PartsLeg leg;

    public int CurrentHP()
    {
        int hp = 0;
        if (body)
        {
            hp += body.CurrentPartsHp;
        }
        if (head)
        {
            hp += head.CurrentPartsHp;
        }
        if (lArm)
        {
            hp += lArm.CurrentPartsHp;
        }
        if (rArm)
        {
            hp += rArm.CurrentPartsHp;
        }
        if (leg)
        {
            hp += leg.CurrentPartsHp;
        }
        return hp;
    }

    public void HitCheck(int hitDamage)
    {
        int hitPos = 0;
        if (body && body.CurrentPartsHp > 0)
        {
            hitPos += body.GetPartsSize();
        }
        if (head && head.CurrentPartsHp > 0)
        {
            hitPos += head.GetPartsSize();
        }
        if (lArm && lArm.CurrentPartsHp > 0)
        {
            hitPos += lArm.GetPartsSize();
        }
        if (rArm && rArm.CurrentPartsHp > 0)
        {
            hitPos += rArm.GetPartsSize();
        }
        if (leg && leg.CurrentPartsHp > 0)
        {
            hitPos += leg.GetPartsSize();
        }
        int r = Random.Range(0, hitPos);
        if (r > body.GetPartsSize())
        {
            r -= body.GetPartsSize();
        }
        else
        {
            body.Damage(r);
        }
    }

}
