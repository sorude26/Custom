using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField]
    int armorPoint = 0;
    [SerializeField]
    int defense = 0;
    private void OnTriggerEnter(Collider other)
    {
        Bullet hitBullet = other.GetComponent<Bullet>();
        if (hitBullet != null)
        {
            armorPoint--;
            hitBullet.HitBullet(defense);
            if (armorPoint < 0)
            {
                EffectManager.PlayEffect(EffectID.BreakParts, transform.position);
                gameObject.SetActive(false);
            }
        }
    }
}
