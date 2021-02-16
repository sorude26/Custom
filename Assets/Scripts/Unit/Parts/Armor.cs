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
    [SerializeField]
    GameObject damage;
    public int Defense { get; private set; }
    private void Start()
    {
        if (armorPoint > 0)
        {
            SetData(armorPoint, defense);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Bullet hitBullet = other.GetComponent<Bullet>();
        if (hitBullet != null)
        {
            GameObject hit = Instantiate(damage);
            hit.transform.position = transform.position * 1.02f;
            hitBullet.HitBullet(Defense);
            ArmorDamage();
        }
    }
    public void ArmorDamage()
    {
        ArmorPoint--;
        if (ArmorPoint < 0)
        {
            SoundManager.Instance.PlaySE(SEType.Break);
            EffectManager.PlayEffect(EffectID.Smoke, transform.position);
            gameObject.SetActive(false);
        }
    }
    public void SetData(int armorPoint, int defense)
    {
        ArmorPoint = armorPoint;
        Defense = defense;
    }
}
