using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPartsMaster : PartsMaster
{
    /// <summary> パーツ耐久値 </summary>
    [SerializeField] protected int partsHp;
    /// <summary> パーツ耐久値 </summary>
    public int MaxPartsHp { get => partsHp; }
    /// <summary> パーツ装甲値 </summary>
    [SerializeField] protected int defense;
    /// <summary> パーツ装甲値 </summary>
    public int Defense { get => defense; }
    /// <summary> 現在のパーツ耐久値 </summary>
    public int CurrentPartsHp { get; protected set; }

    /// <summary>
    /// パーツの初期化
    /// </summary>
    protected virtual void StartSet()
    {
        CurrentPartsHp = partsHp;
    }
}
