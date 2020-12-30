using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField]
    int armorPoint = 0;
    public int ArmorPoint { get; private set; }
    [SerializeField]
    int defense = 0;
    public int Defense { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        Bullet hitBullet = other.GetComponent<Bullet>();
        if (hitBullet != null)
        {
            //Debug.Log("アーマーヒット" + hitBullet.Power + "残：" + ArmorPoint);
            ArmorPoint--;
            hitBullet.HitBullet(Defense);
            if (ArmorPoint < 0)
            {
                //Debug.Log("アーマーブレイク");
                EffectManager.PlayEffect(EffectID.BreakParts, transform.position);
                gameObject.SetActive(false);
            }
        }
    }

    public void SetData(int armorPoint, int defense)
    {
        ArmorPoint = armorPoint;
        Defense = defense;
    }
}
