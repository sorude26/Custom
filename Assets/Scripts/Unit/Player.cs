using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [SerializeField]
    int unitID = 0;
    public GameObject mark;
    public GameObject searchScale;
    private bool searchOn = false;
    public List<Unit> TargetEnemies { get; private set; } = new List<Unit>();
    public Unit TargetEnemy { get; set; }
    private float actionTimer = 0;
    private bool nBody = false;
    void Update()
    {
        if (!nBody)
        {
            if (!silhouetteOn && !DestroyBody)
            {
                searchScale.SetActive(false);
                //UnitCreate(1, 1, 1, 5, 1, 5, 0);
                //UnitCreate(GameManager.UnitDatas[unitID].HeadID, GameManager.UnitDatas[unitID].BodyID, GameManager.UnitDatas[unitID].LArmID,
                //     GameManager.UnitDatas[unitID].WeaponLID, GameManager.UnitDatas[unitID].RArmID, GameManager.UnitDatas[unitID].WeaponRID, GameManager.UnitDatas[unitID].LegID);
                if (GameManager.SortieUnit[unitID].Sortie)
                {
                    UnitCreate(GameManager.SortieUnit[unitID].HeadID, GameManager.SortieUnit[unitID].BodyID, GameManager.SortieUnit[unitID].LArmID,
                   GameManager.SortieUnit[unitID].WeaponLID, GameManager.SortieUnit[unitID].RArmID, GameManager.SortieUnit[unitID].WeaponRID, GameManager.SortieUnit[unitID].LegID);
                }
                else
                {
                    DestroyBody = true;
                    gameStage.PlayerDestroyCount++;
                    nBody = true;
                    return;
                }
            }
            if (silhouetteOn)
            {
                if (moveMood)
                {
                    UnitMove();
                }
                if (gameStage.MoveFinish && !moveMood && !attackMode && ActionTurn)//移動終了で位置を保存
                {
                    actionTimer += Time.deltaTime;
                    if (actionTimer > 0.5f)
                    {
                        MoveFinishSet();
                        gameStage.MoveFinish = false;
                        gameStage.turnCountTimer = 2;
                        actionTimer = 0;
                        ActionTurn = false;
                        gameStage.PlayerTurnSystem();
                        if (searchOn)
                        {
                            searchOn = false;
                            searchScale.SetActive(false);
                        }
                    }
                }
                UnitAngleControl();
                if (DestroyBody)
                {
                    gameStage.PlayerDestroyCount++;
                    gameStage.LossAdd(PartsTotalPlice);
                    silhouetteOn = false;
                }
                if (attackMode && searchOn)
                {
                    searchOn = false;
                    searchScale.SetActive(false);
                }
            }
            PartsMotion();
            DeadMotion();
            MoveMotion();
        }
    }

    private void LateUpdate()
    {
        if (!nBody)
        {
            PartsUpdate();
            AttackSystem();
        }
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

    public void SearchScale()
    {
        searchScale.SetActive(true);
        searchScale.transform.localScale = new Vector3(DetectionRange * 2, Body.GetBodyCentrer().position.y, DetectionRange * 2);
        searchOn = true;
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
