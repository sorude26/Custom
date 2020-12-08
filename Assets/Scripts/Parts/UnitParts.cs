﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitParts : MonoBehaviour
{
    [SerializeField]
    protected string partsName;
    public string PartsName { get; protected set; }

    [SerializeField]
    protected int partsHp;
    public int CurrentPartsHp { get; protected set; }

    [SerializeField]
    protected int defense;
    public int Defense { get; protected set; }

    [SerializeField]
    protected int weight;
    public int Weight { get; protected set; }//重量
    public bool PartsDestroy { get; protected set; }
    protected void StartSet()
    {
        PartsName = partsName;
        CurrentPartsHp = partsHp;
        Defense = defense;
        Weight = weight;
    }

    public void Damage(int damage)
    {
        CurrentPartsHp -= damage;
    }
}
