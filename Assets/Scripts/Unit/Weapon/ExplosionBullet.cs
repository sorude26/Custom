using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : Bullet
{
    [SerializeField]
    GameObject thisBullet = null;//弾
    private bool explosion = false;
    private bool trigger = false;
    [SerializeField]
    int horizontalBulletCount = 12;
    private void Update()
    {
        transform.localPosition += moveDir * Speed * Time.deltaTime;//移動処理
        Vector3 dir = startPos - transform.localPosition;//移動量計算
        
        if (dir.sqrMagnitude >= Range * Range || explosion)//射程範囲外で消去
        {
            Destroy(gameObject);
        }
        if (startPower != Power && !explosion || trigger)
        {
            Explosion();
            explosion = true;
        }
    }

    public void StartBom(int weapon, Vector3 pos)
    {
        transform.localPosition = pos;
        diffusivity = weapon;
        Power = weapon;
        startPower = weapon;
        trigger = true;
    }

    public void Explosion()
    {
        SoundManager.Instance.PlaySE(SEType.Explosion2);
        EffectManager.PlayEffect(EffectID.Explosion, transform.position);
        var angleStep = 360.0f / horizontalBulletCount;
        BomShot(0.0f, horizontalBulletCount); // 赤道方向に弾を発射
        for (var angle = angleStep; angle < 90.0f; angle += angleStep)
        {
            BomShot(angle, horizontalBulletCount); // 北緯angle°に弾を発射
            BomShot(-angle, horizontalBulletCount); // 南緯angle°に弾を発射
        }        
    }

    void BomShot(float angleV, int maxCount)
    {
        angleV *= Mathf.Deg2Rad; // 緯度を弧度法による角度に変換
        var cosAngleV = Mathf.Cos(angleV);
        var sinAngleV = Mathf.Sin(angleV);
        var count = (int)(cosAngleV * maxCount); // 緯度に合わせて弾数を減らす...緯線の長さは緯度のコサインに比例する
        for (var i = 0; i < count; i++)
        {
            var angleH = (2.0f * Mathf.PI * i) / count; // 弧度法による経度
            var cosAngleH = Mathf.Cos(angleH);
            var sinAngleH = Mathf.Sin(angleH);

            // 緯度、経度からVector3による方角を算出
            var x = sinAngleH * cosAngleV;
            var y = sinAngleV;
            var z = cosAngleH * cosAngleV;
            var direction = new Vector3(x, y, z); // この方角に弾を1発発射したい

            GameObject bullet = Instantiate(thisBullet);
            Bullet shotBullet = bullet.GetComponent<Bullet>();
            shotBullet.Shot(startPower/2, transform.position, direction);
        }
    }
}
