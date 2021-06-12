using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsMaster : MonoBehaviour
{
    /// <summary> パーツID </summary>
    [SerializeField] int partsID;
    /// <summary> パーツID </summary>
    public int PartsID { get => partsID; }
    /// <summary> パーツ名 </summary>
    [SerializeField] string partsName;
    /// <summary> パーツ名 </summary>
    public string PartsName { get => partsName; }
    /// <summary> 重量 </summary>
    [SerializeField] protected int weight = 10;
    /// <summary> 重量 </summary>
    public int Weight { get => weight; }
    /// <summary> パーツサイズ </summary>
    [SerializeField] int partsSize = 1;
    /// <summary> パーツサイズ </summary>
    public int PartsSize { get => partsSize; }

    /// <summary> 命中精度 </summary>
    [SerializeField] protected int hitAccuracy;
    /// <summary> 命中精度 </summary>
    public int HitAccuracy { get => hitAccuracy; }
}
