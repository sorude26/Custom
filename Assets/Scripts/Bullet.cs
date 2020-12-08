using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Power { get; private set; }//威力
    public float Speed { get; private set; }//弾速
    public float Range { get; private set; }//最大射程
    public float EffectiveRange { get; private set; }//有効射程
    private bool overRange = false;//有効射程外か
    Vector3 moveDir = Vector3.zero;//移動方向
    Vector3 startPos;//発射位置
    private float diffusivity = 0.02f;//拡散率
    private void Start()
    {
        startPos = transform.position;
    }
    /// <summary>
    /// 弾生成時の設定
    /// </summary>
    /// <param name="weapon">発射武器</param>
    /// <param name="pos">発射位置</param>
    /// <param name="angle">発射方向</param>
    public void StartMove(Weapon weapon,Vector3 pos,Vector3 angle)
    {
        transform.localPosition = pos;
        moveDir = angle;
        moveDir.x += Random.Range(-diffusivity, diffusivity);
        moveDir.y += Random.Range(-diffusivity, diffusivity);
        moveDir.z += Random.Range(-diffusivity, diffusivity);
        Power = weapon.Power;
        Speed = weapon.BulletSpeed;
        Range = weapon.Range;
        EffectiveRange = weapon.EffectiveRange;
    }

    void Update()
    {
        transform.localPosition += moveDir * Speed * Time.deltaTime;//移動処理

        Vector3 dir = startPos - transform.localPosition;//移動量計算
        if (dir.sqrMagnitude >= EffectiveRange * EffectiveRange && !overRange)//有効射程内か
        {
            overRange = true;
            Power /= 5;
        }
        if (dir.sqrMagnitude >= Range * Range || Power <= 0)//射程範囲外又は、威力低下で消去
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit hitUnit = other.GetComponent<Unit>();
        if (hitUnit != null)//命中処理
        {
            if (Power > 0)
            {
                Debug.Log("命中");
                hitUnit.Damage(Power);
            }
            Power -= hitUnit.Defense;
        }

        UnitParts hitParts = other.GetComponent<UnitParts>();
        if (hitParts != null)
        {
            hitParts.Damage(Power);
            Power -= hitParts.Defense;
        }
    }
}
