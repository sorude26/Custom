using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Rifle,
    MachineGun,
    Shotgun,
    MShotGun,
    Melee,
}

public class Weapon : MonoBehaviour
{

    [SerializeField]
    GameObject thisBullet = null;//弾

    [SerializeField]
    Transform muzzle = null;//銃口

    [SerializeField]
    WeaponType weaponType = WeaponType.Rifle;//武器種
    public WeaponType Type { get; protected set; }
    public float Range { get; private set; }//最大射程
    [SerializeField] float range = 0;
    public float EffectiveRange { get; private set; }//有効射程
    [SerializeField] float effectiveRange = 0;
    public int Power { get; private set; }//威力
    [SerializeField] int power = 0;
    public float BulletSpeed { get; private set; }//弾速
    [SerializeField] float bulletSpeed = 0;
    public int TotalShotNumber { get; private set; }//総射撃数
    [SerializeField] int totalShotNumber = 0;
    private int shotNumber;//発射弾数
    private float intervalTime;//発射間隔
    [SerializeField] float diffusivity = 0.02f;//拡散率
    public float Diffusivity { get; private set; }
    public int NumberOfBullets { get; private set; }//装弾数
    [SerializeField] int weight = 0;
    public int Weight { get; private set; }//重量
    [SerializeField] protected string weaponName = "A";
    [SerializeField] protected GameObject blade = null;//攻撃判定
    protected bool weaponTrigger = false;//攻撃トリガー
    private bool shotStart = false;//攻撃開始フラグ
    public bool AttackNow { get; private set; }
    private float attackTimer = 0;
    protected Unit owner;
    protected void Start()
    {
        Type = weaponType;
        Weight = weight;
        Range = range;
        EffectiveRange = effectiveRange;
        Power = power;
        BulletSpeed = bulletSpeed;
        TotalShotNumber = totalShotNumber;
        Diffusivity = diffusivity;
        if (blade)
        {
            blade.SetActive(false);
        }
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
                    owner.ShotCameraShake(20);
                    break;
                case WeaponType.MachineGun:
                    MachineGunShot();
                    break;
                case WeaponType.Shotgun:
                    ShotgunShot();                    
                    break;
                case WeaponType.MShotGun:
                    MachineShotGunShot();
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
        EffectManager.PlayEffect(EffectID.MuzzleFlash, muzzle.position);        
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
                owner.ShotCameraShake(5);
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
        owner.ShotCameraShake(20);
        weaponTrigger = false;
    }
    private void MachineShotGunShot()
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
                for (int i = 0; i < totalShotNumber; i++)
                {
                    BulletShot();
                }
                shotNumber++;
                intervalTime = 0.15f;
                owner.ShotCameraShake(5);
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
    public void PartsDelete()
    {
        Destroy(this);
    }
    public string GetName() { return weaponName; }
    public int GetWeight() { return weight; }
    public float GetRange() { return range; }
    public float GetEffectiveRange() { return effectiveRange; }
    public int GetPower() { return power; }
    public int GetShotNumber() { return totalShotNumber; }
    public WeaponType GetWeaponType() { return weaponType; }
    public void SetOwner(Unit owner)
    {
        this.owner = owner;
    }
    public void BladeAttackStart()
    {
        if (blade)
        {
            blade.SetActive(true);
        }
    }
    public void BladeAttackEnd()
    {
        if (blade)
        {
            blade.SetActive(false);
        }
    }
}
