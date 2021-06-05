﻿using System.Collections;
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
    int bodySize = 0;
    int headSize = 0;
    int lArmSize = 0;
    int rArmSize = 0;
    int legSize = 0;
    protected virtual void StartSet()
    {
        if (body)
        {
            bodySize = body.GetPartsSize();
        }
        if (head)
        {
            headSize = head.GetPartsSize();
        }
        if (lArm)
        {
            lArmSize = lArm.GetPartsSize();
        }
        if (rArm)
        {
            rArmSize = rArm.GetPartsSize();
        }
        if (leg)
        {
            legSize = leg.GetPartsSize();
        }
    }
    /// <summary>
    /// 現在の総パーツ耐久値を返す
    /// </summary>
    /// <returns></returns>
    public int CurrentHP()
    {
        int hp = 0;
        UnitParts[] allParts = { body, head, lArm, rArm, leg };
        foreach (var parts in allParts)
        {
            if (parts)
            {
                hp += parts.CurrentPartsHp;
            }
        }
        return hp;
    }

    /// <summary>
    /// 命中弾をランダムなパーツに割り振り、ダメージ計算を行わせる
    /// </summary>
    /// <param name="hitDamage"></param>
    public void HitCheckShot(int hitDamage)
    {
        int hitPos = 0;
        UnitParts[] allParts = { body, head, lArm, rArm, leg };
        foreach (var parts in allParts)
        {
            if (parts)
            {
                if (parts.CurrentPartsHp > 0)
                {
                    continue;
                }
                hitPos += parts.GetPartsSize();
            }
        }
        int r = Random.Range(0, hitPos);
        if (bodySize > r)
        {
            body.Damage(hitDamage);
        }
        else
        {
            r -= bodySize;
        }
        if (head)
        {
            if (head.CurrentPartsHp > 0 && head.GetPartsSize() > r)
            {
                head.Damage(hitDamage);
            }
            else
            {
                r -= head.GetPartsSize();
            }
        }
        if (lArm)
        {
            if (lArm.CurrentPartsHp > 0 && lArmSize > r)
            {
                lArm.Damage(hitDamage);
            }
            else
            {
                r -= lArmSize;
            }
        }
        if (rArm)
        {
            if (rArm.CurrentPartsHp > 0 && rArmSize > r)
            {
                rArm.Damage(hitDamage);
            }
        }
        if (leg)
        {
            leg.Damage(hitDamage);
        }
    }

}
