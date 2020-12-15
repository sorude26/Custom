using System.Collections;
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

    protected bool partsBreak = false;
    protected void StartSet()
    {
        PartsName = partsName;
        CurrentPartsHp = partsHp;
        Defense = defense;
        Weight = weight;
    }

    public void Damage(int damage)
    {
        if (CurrentPartsHp > 0)
        {
            CurrentPartsHp -= damage;
            Debug.Log(partsName + "にヒット" + damage + "ダメージ！残："+ CurrentPartsHp );
            if (CurrentPartsHp <= 0)
            {
                CurrentPartsHp = 0;
                PartsBreak();
            }
        }
    }

    public void TransFormParts(Vector3 partsPos)
    {
        transform.position = partsPos;
    }

    protected void PartsBreak()
    {
        EffectManager.PlayEffect(EffectID.Explosion, transform.position);
        partsBreak = true;
    }
}
