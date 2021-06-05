using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMaster : MonoBehaviour
{
    public int HitAccuracy { get; protected set; }

    protected PartsBody body = null;
    protected PartsHead head = null;
    protected PartsLArm lArm = null;
    protected PartsRArm rArm = null;
    protected PartsLeg leg = null;
    protected virtual void StartSet()
    {

    }
    /// <summary>
    /// 現在の総パーツ耐久値を返す
    /// </summary>
    /// <returns></returns>
    public int GetCurrentHP()
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
    /// 現在の回避率を返す
    /// </summary>
    /// <returns></returns>
    public int GetAvoidance()
    {
        int avoidance = body.GetUnitOverOutput();
        if (leg)
        {
            if (leg.CurrentPartsHp > 0)
            {
                avoidance += leg.GetAvoidance();
            }
        }
        if (head)
        {
            if (head.CurrentPartsHp > 0)
            {
                avoidance += head.GetAvoidance();
            }
        }
        return avoidance;
    }
    /// <summary>
    /// 平均装甲値を返す
    /// </summary>
    /// <returns></returns>
    public int GetAmorPoint()
    {
        int count = 0; 
        int armor = 0;
        UnitParts[] allParts = { body, head, lArm, rArm, leg };
        foreach (var parts in allParts)
        {
            if (parts)
            {
                if (parts.CurrentPartsHp > 0)
                {
                    continue;
                }
                armor += parts.GetArmorPoint();
                count++;
            }
        }
        return armor / count;
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
        int prb = 0;
        foreach (var parts in allParts)
        {
            if (parts)
            {
                if (parts.CurrentPartsHp > 0)
                {
                    continue;
                }
                prb += parts.GetPartsSize();
                if (prb > r)
                {
                    parts.Damage(hitDamage);
                    break;
                }
            }
        }
    }

}
