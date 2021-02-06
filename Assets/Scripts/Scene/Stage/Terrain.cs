using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)//地形に衝突した弾を破壊
    {
        Bullet hitBullet = other.GetComponent<Bullet>();
        if (hitBullet != null)
        {
            hitBullet.HitBullet(100);
        }
    }
}
