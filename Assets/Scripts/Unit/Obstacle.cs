﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Unit
{
    private void OnTriggerEnter(Collider other)
    {
        Bullet hitBullet = other.GetComponent<Bullet>();
        if (hitBullet != null)
        {
            ObstacleHit(hitBullet.Power);
            hitBullet.HitBullet(Defense);
        }
    }
    public void ObstacleHit(int power)
    {
        CurrentHp -= power;
        EffectManager.PlayEffect(EffectID.BreakParts, transform.position);
        if (CurrentHp < 0)
        {
            Dead();
            gameObject.SetActive(false);
        }
    }
}
