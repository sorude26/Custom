using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : Bullet
{
    [SerializeField]
    GameObject thisBullet = null;//弾
    private bool explosion = false;
    private void Update()
    {
        transform.localPosition += moveDir * Speed * Time.deltaTime;//移動処理

        Vector3 dir = startPos - transform.localPosition;//移動量計算
        if (dir.sqrMagnitude >= Range * Range || explosion)//射程範囲外で消去
        {
            Destroy(gameObject);
        }
        if (startPower != Power && !explosion)
        {
            Explosion();
            explosion = true;
        }
    }

    private void Explosion()
    {
        EffectManager.PlayEffect(EffectID.Explosion, transform.position);
        for (int j = 0; j < 10; j ++)
        {
            for (int i = 0; i < 10; i ++)
            {                
                GameObject bullet = Instantiate(thisBullet);
                Bullet shotBullet = bullet.GetComponent<Bullet>();
                shotBullet.Shot(startPower, transform.position, transform.forward * -1);
            }
        }
    }
}
