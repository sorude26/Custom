using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRArm : UnitParts
{
    public Weapon HaveWeapon { get; set; }

    [SerializeField]
    GameObject armParts;
    void Start()
    {
        StartSet();
    }

}
