using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeType
{
    Axe,
    Knuckle,
}
public class MeleeWeapon : Weapon
{
    [SerializeField]
    public MeleeType meleeType = MeleeType.Axe;
    [SerializeField]
    GameObject damage;
    private void OnTriggerEnter(Collider other)
    {
        UnitParts hitParts = other.GetComponent<UnitParts>();
        if (hitParts != null)
        {
            if (hitParts.Owner != owner)
            {
                int power = Power + Random.Range(-PowerRange, PowerRange + 1);
                PartsLeg partsLeg = other.GetComponent<PartsLeg>();
                if (partsLeg)
                {
                    GameObject hit = Instantiate(damage);
                    DamageText damageText = hit.GetComponent<DamageText>();
                    damageText.ViewDamege(power / 3, transform.position);
                    EffectManager.PlayEffect(EffectID.Hit, blade.transform.position);
                    hitParts.Damage(power / 3);
                }
                else
                {
                    GameObject hit = Instantiate(damage);
                    DamageText damageText = hit.GetComponent<DamageText>();
                    damageText.ViewDamege(power, transform.position);
                    EffectManager.PlayEffect(EffectID.Hit, blade.transform.position);
                    hitParts.Damage(power);
                }
                SoundManager.Instance.PlaySE(SEType.Hit);
            }
        }
        else
        {
            Armor armor = other.GetComponent<Armor>();
            if (armor)
            {
                EffectManager.PlayEffect(EffectID.Hit, blade.transform.position);
                armor.ArmorDamage();
                GameObject hit = Instantiate(damage);
                DamageText damageText = hit.GetComponent<DamageText>();
                damageText.ViewDamege(1, transform.position);
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
