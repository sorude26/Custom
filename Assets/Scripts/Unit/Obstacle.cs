using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Unit
{
    private void OnTriggerEnter(Collider other)
    {
        Bullet hitBullet = other.GetComponent<Bullet>();
        if (hitBullet != null)
        {
            CurrentHp -= hitBullet.Power;
            hitBullet.HitBullet(Defense);
            if (CurrentHp < 0)
            {
                Dead();
                gameObject.SetActive(false);
            }
        }
    }
}
