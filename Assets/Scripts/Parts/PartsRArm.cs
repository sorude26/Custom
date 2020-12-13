using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsRArm : UnitParts
{
    public Weapon HaveWeapon { get; set; }

    [SerializeField]
    GameObject armParts;
    [SerializeField]
    Transform grip;
    void Start()
    {
        StartSet();
    }
    private void Update()
    {
        if (partsBreak)
        {
            gameObject.SetActive(false);
        }
    }
    public Transform GetGrip()
    {
        return grip;
    }
    public GameObject ArmParts() { return armParts; }
}
