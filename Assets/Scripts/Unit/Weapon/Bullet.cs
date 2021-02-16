using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public int Power { get; protected set; }//威力
    public float Speed { get; protected set; }//弾速
    public float Range { get; protected set; }//最大射程
    public float EffectiveRange { get; protected set; }//有効射程
    protected bool overRange = false;//有効射程外か
    protected Vector3 moveDir = Vector3.zero;//移動方向
    protected Vector3 startPos;//発射位置
    protected float diffusivity;//拡散率
    [SerializeField]
    protected GameObject damage;
    protected int startPower;

    protected void Start()
    {
        startPos = transform.position;
    }
    /// <summary>
    /// 弾生成時の設定
    /// </summary>
    /// <param name="weapon">発射武器</param>
    /// <param name="pos">発射位置</param>
    /// <param name="angle">発射方向</param>
    public void StartMove(Weapon weapon, Vector3 pos, Vector3 angle)
    {
        int power = Random.Range(-weapon.PowerRange, weapon.PowerRange + 1);
        transform.localPosition = pos;
        diffusivity = weapon.Diffusivity;
        moveDir = angle;
        moveDir.x += Random.Range(-diffusivity, diffusivity);
        moveDir.y += Random.Range(-diffusivity, diffusivity);
        moveDir.z += Random.Range(-diffusivity, diffusivity);
        Power = weapon.Power + power;
        startPower = weapon.Power + power;
        Speed = weapon.BulletSpeed;
        Range = weapon.Range;
        EffectiveRange = weapon.EffectiveRange;
    }

    public void Shot(int power, Vector3 pos, Vector3 angle)
    {
        transform.localPosition = pos;
        moveDir = angle;
        Power = power;
        startPower = power;
        Speed = 150;
        Range = 30;
        EffectiveRange = 10;
    }
    private void Update()
    {
        transform.localPosition += moveDir * Speed * Time.deltaTime;//移動処理

        Vector3 dir = startPos - transform.localPosition;//移動量計算
        if (dir.sqrMagnitude >= EffectiveRange * EffectiveRange && !overRange)//有効射程内か
        {
            overRange = true;
            Power /= 2;
        }
        if (dir.sqrMagnitude >= Range * Range || Power <= 0)//射程範囲外又は、威力低下で消去
        {
            Destroy(gameObject);
        }
    }

    public void HitBullet(int Defense)
    {

        EffectManager.PlayEffect(EffectID.Hit, transform.position);        
        Power -= Defense;
    }
    protected void OnTriggerEnter(Collider other)
    {
        UnitParts hitParts = other.GetComponent<UnitParts>();
        if (hitParts != null)
        {
            if (Power > 0)
            {
                GameObject hit = Instantiate(damage);
                DamageText damageText = hit.GetComponent<DamageText>();
                damageText.ViewDamege(Power, transform.position);
                EffectManager.PlayEffect(EffectID.Hit, transform.position);
                hitParts.Damage(Power);
                Power -= hitParts.Defense;
            }
        }
    }
}
