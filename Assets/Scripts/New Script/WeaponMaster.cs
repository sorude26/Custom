using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMaster : PartsMaster
{
    /// <summary> 武器攻撃力 </summary>
    [SerializeField] int power = 5;
    /// <summary> 武器攻撃力 </summary>
    public int Power { get => power; }
    /// <summary> 最大射程 </summary>
    [SerializeField] float range = 4;
    /// <summary> 最大射程 </summary>
    public float Range { get => range; }
    /// <summary> 武器種 </summary>
    [SerializeField] WeaponType weaponType = WeaponType.Rifle;
    /// <summary> 武器種 </summary>
    public WeaponType Type { get => weaponType; }
    /// <summary> 総攻撃回数 </summary>
    [SerializeField] int maxAttackNumber = 1;
    /// <summary> 総攻撃回数 </summary>
    public int MaxAttackNumber { get => maxAttackNumber; }
}
