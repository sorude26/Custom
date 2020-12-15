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
    [SerializeField] float diffusivity = 0.02f;//拡散率
    public float Diffusivity { get; private set; }
    public int NumberOfBullets { get; private set; }//装弾数
    public int Weight { get; private set; }//重量

    private bool weaponTrigger = false;//攻撃トリガー
    private bool shotStart = false;//攻撃開始フラグ
    public bool AttackNow { get; private set; }
    private float attackTimer = 0;
    private void Start()
    {
        Range = range;
        EffectiveRange = effectiveRange;
        Power = power;
        BulletSpeed = bulletSpeed;
        TotalShotNumber = totalShotNumber;
        Diffusivity = diffusivity;
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
                    ShotgunShot();                    
                    break;
                default:
                    weaponTrigger = false;
                    break;
            }
        }
        if (!weaponTrigger && attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                AttackNow = false;
            }
        }
    }

    public void Shot()
    {
        weaponTrigger = true;
        AttackNow = true;
        attackTimer = 0.5f;
    }
    
    private void BulletShot()
    {
        GameObject bullet = Instantiate(thisBullet);
        Bullet shotBullet = bullet.GetComponent<Bullet>();
        shotBullet.StartMove(this, muzzle.position, transform.forward * -1);
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
    private void ShotgunShot()
    {
        for (int i = 0; i < totalShotNumber; i++)
        {
            BulletShot();
        }
        weaponTrigger = false;
    }

    public void TransFormParts(Vector3 partsPos)
    {
        transform.position = partsPos;
    }
}
