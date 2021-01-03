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
                EffectManager.PlayEffect(EffectID.Hit, blade.transform.position);
                hitParts.Damage(Power);
            }
        }
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if (obstacle)
        {
            EffectManager.PlayEffect(EffectID.Hit, blade.transform.position);
            obstacle.ObstacleHit(Power);
        }
    }
    
}
