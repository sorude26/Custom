using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public GameObject mark;
    List<Enemy> targetEnemies = new List<Enemy>();
    void Update()
    {
        if (!silhouetteOn && !DestroyBody)
        {
            UnitCreate(1, 1, 1, 2, 1, 3, 0);
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
                gameObject.SetActive(false);
                gameStage.PlayerDestroyCount++;
                silhouetteOn = false;
            }
        }

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
        targetEnemies.Clear();
        foreach (Enemy enemy in unitManager.GetEnemies())
        {
            Vector3 dir = enemy.transform.position - transform.position;
            if (dir.sqrMagnitude <= DetectionRange * DetectionRange)
            {
                targetEnemies.Add(enemy);
            }
        }
    }
    public List<Enemy> GetEnemies()
    {
        return targetEnemies;
    }
}
