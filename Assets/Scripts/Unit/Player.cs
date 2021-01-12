using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [SerializeField]
    int unitID = 0;
    public GameObject mark;
    public List<Unit> TargetEnemies { get; private set; } = new List<Unit>();
    public Unit TargetEnemy { get; set; }
    void Update()
    {
        if (!silhouetteOn && !DestroyBody)
        {
            //UnitCreate(1, 1, 1, 5, 1, 5, 0);
            UnitCreate(GameManager.HeadID[unitID], GameManager.BodyID[unitID], GameManager.LArmID[unitID],
                GameManager.WeaponLID[unitID], GameManager.RArmID[unitID], GameManager.WeaponRID[unitID], GameManager.LegID[unitID]);
        }
        if (silhouetteOn)
        {
            if (moveMood)
            {
                UnitMove();
            }
            if (gameStage.MoveFinish && !moveMood)//移動終了で位置を保存
            {
                MoveFinishSet();
                gameStage.MoveFinish = false;
                gameStage.turnCountTimer = 2;
            }
            UnitAngleControl();
            if (DestroyBody)
            {
                gameStage.PlayerDestroyCount++;
                silhouetteOn = false;
            }
        }
        PartsMotion();
        DeadMotion();
        MoveMotion();
    }

    private void LateUpdate()
    {
        PartsUpdate();
        AttackSystem();
    }
    /// <summary>
    /// 移動可能箇所表示
    /// </summary>
    /// <param name="moveList"></param>
    public void UnitMoveList2(List<Map.MapDate> moveList)
    {
        foreach (Map.MapDate map in moveList)
        {
            if (map.movePoint > 0)
            {
                GameObject instance = Instantiate(mark);
                instance.transform.position = new Vector3(map.PosX * gameMap.mapScale, map.Level, map.PosZ * gameMap.mapScale);
            }
        }
    }

    public void SearchTarget()
    {
        TargetEnemies.Clear();
        TargetEnemies.Add(null);
        if (TargetEnemy)
        {
            if (!TargetEnemy.DestroyBody)
            {
                Vector3 dir = TargetEnemy.transform.position - transform.position;
                if (dir.sqrMagnitude <= DetectionRange * DetectionRange)
                {
                    TargetEnemies.Add(TargetEnemy);
                }
                else
                {
                    TargetEnemy = null;
                }
            }
            else
            {
                TargetEnemy = null;
            }
        }
        foreach (Enemy enemy in unitManager.GetEnemies())
        {
            if (enemy != TargetEnemy)
            {
                if (!enemy.DestroyBody)
                {
                    Vector3 dir = enemy.transform.position - transform.position;
                    if (dir.sqrMagnitude <= DetectionRange * DetectionRange)
                    {
                        TargetEnemies.Add(enemy);
                    }
                }
            }
        }
        TargetEnemy = null;
    }
    public List<Unit> GetEnemies()
    {
        return TargetEnemies;
    }

    public Unit GetTarget(int i)
    {
        return TargetEnemies[i];
    }
    public void Attack()
    {
        attackTrigger = true;
    }
}
