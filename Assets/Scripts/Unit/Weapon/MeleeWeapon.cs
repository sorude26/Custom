using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    private void OnTriggerEnter(Collider other)
    {
        UnitParts hitParts = other.GetComponent<UnitParts>();
        if (hitParts != null)
        {
            if (hitParts.Owner != owner)
            {
                PartsLeg partsLeg = other.GetComponent<PartsLeg>();
                if (partsLeg)
                {
                    EffectManager.PlayEffect(EffectID.Hit, blade.transform.position);
                    hitParts.Damage(Power / 3);
                }
                else
                {
                    EffectManager.PlayEffect(EffectID.Hit, blade.transform.position);
                    hitParts.Damage(Power);
                }
            }
        }
        else
        {
            Armor armor = other.GetComponent<Armor>();
            if (armor)
            {
                EffectManager.PlayEffect(EffectID.Hit, blade.transform.position);
                armor.ArmorDamage();
            }
            else
            {
                Obstacle obstacle = other.GetComponent<Obstacle>();
                if (obstacle)
                {
                    EffectManager.PlayEffect(EffectID.Hit, blade.transform.position);
                    obstacle.ObstacleHit(Power);
                }
            }
        }
    }

}
