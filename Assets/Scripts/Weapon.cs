using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponType
{
    Rifle,
    MachineGun,
    Shotgun,
}

public class Weapon : MonoBehaviour
{

    [SerializeField]
    GameObject thisBullet;//弾

    [SerializeField]
    Transform muzzle;//銃口

    [SerializeField]
    WeaponType weaponType = WeaponType.Rifle;//武器種
    public float Range { get; private set; }//最大射程
    [SerializeField] float range;
    public float EffectiveRange { get; private set; }//有効射程
    [SerializeField] float effectiveRange;
    public int Power { get; private set; }//威力
    [SerializeField] int power;
    public float BulletSpeed { get; private set; }//弾速
    [SerializeField] float bulletSpeed;
    public int TotalShotNumber { get; private set; }//総射撃数
    [SerializeField] int totalShotNumber;
    private int shotNumber;//発射弾数
    private float intervalTime;//発射間隔
    public int NumberOfBullets { get; private set; }//装弾数
    public int Weight { get; private set; }//重量

    private bool weaponTrigger = false;//攻撃トリガー
    private bool shotStart = false;//攻撃開始フラグ

    private void Start()
    {
        Range = range;
        EffectiveRange = effectiveRange;
        Power = power;
        BulletSpeed = bulletSpeed;
        TotalShotNumber = totalShotNumber;
    }
    private void Update()
    {
        if (weaponTrigger)
        {
            switch (weaponType)
            {
                case WeaponType.Rifle:
                    BulletShot();
                    weaponTrigger = false;
                    break;
                case WeaponType.MachineGun:
                    MachineGunShot();
                    break;
                case WeaponType.Shotgun:
                    weaponTrigger = false;
                    break;
                default:
                    weaponTrigger = false;
                    break;
            }
        }
    }

    public void Shot()
    {
        weaponTrigger = true;
    }
    
    private void BulletShot()
    {
        GameObject bullet = Instantiate(thisBullet);
        Bullet shotBullet = bullet.GetComponent<Bullet>();
        shotBullet.StartMove(this, muzzle.position, transform.forward);
    }

    private void MachineGunShot()
    {
        if (!shotStart)
        {
            shotNumber = 0;
            shotStart = true;
            intervalTime = 0.1f;
        }
        else
        {
            intervalTime -= Time.deltaTime;
        }
        if (intervalTime <= 0)
        {
            if (shotNumber < TotalShotNumber)
            {
                BulletShot();
                shotNumber++;
                intervalTime = 0.1f;
            }
            else
            {
                shotStart = false;
                weaponTrigger = false;
            }
        }
    }
    public void TransFormParts(Vector3 partsPos)
    {
        transform.position = partsPos;
    }
}
