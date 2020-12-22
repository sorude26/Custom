﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Player PlayerUnit { get; private set; }
    public Unit subUnit;
    public Enemy enemyUnit;
    private UnitManager unitManager;
    public List<Unit> stageUnits;
    public List<int[]> stageUnitsPos = new List<int[]>();
    public Map map;
    public static Stage StageDate { get; private set; }
    public bool PlayerMoveMode { get; private set; } = false;
    public bool MoveFinish { get; set; } = false;
    public bool MoveNow { get; private set; }

    public bool PlayerTurn { get; private set; }
    public bool EnemyTurn { get; set; }
    public bool EnemyAction { get; set; }

    public float turnCountTimer = 0;
    public int PlayUnitCount { get; private set; } = 0;
    public int EnemyUnitCount { get; private set; } = 0;

    bool start = false;
    private void Awake()
    {
        StageDate = this;
    }

    void Start()
    {
        unitManager = UnitManager.Instance;
        foreach (Enemy unit in unitManager.GetEnemies())
        {
            stageUnits.Add(unit);
        }
        foreach (Player unit in unitManager.GetPlayerList())
        {
            stageUnits.Add(unit);
        }
        foreach (Obstacle obstacle in unitManager.GetObstacles())
        {
            stageUnits.Add(obstacle);
        }
        SetUnitPos();
        PlayerUnit = unitManager.GetPlayer(0);
        PlayerTurn = false;
        EnemyTurn = false;
        TargetCursor.instance.SetCursor(PlayerUnit);
    }

    void Update()
    {
        if (!start && PlayerUnit.Body)
        {
            start = true;
        }
        if (turnCountTimer > 0)
        {
            turnCountTimer -= Time.deltaTime;
        }
        if (PlayerUnit.MoveNow)
        {
            if (!MoveNow)
            {
                MoveNow = true;
            }
        }
        else if (MoveNow)
        {
            MoveNow = false;
        }
        if (!PlayerTurn && !EnemyTurn && turnCountTimer <= 0 && start)
        {
            if (!unitManager.GetPlayer(PlayUnitCount).DestroyBody)
            {
                PlayerUnit.ActionTurn = true;
                PlayerTurn = true;
                PlayerUnit = unitManager.GetPlayer(PlayUnitCount);
            }
            PlayUnitCount++;
            if (PlayUnitCount > unitManager.GetPlayerList().Length)
            {
                PlayUnitCount = 0;
                EnemyTurn = true;
                EnemyAction = true;
                turnCountTimer = 2;
                foreach (Unit unit in unitManager.GetPlayerList())
                {
                    unit.MoveFinishSet();
                }
                TargetCursor.instance.SetCursor(enemyUnit);
            }            
        }
        if (EnemyTurn && turnCountTimer <= 0)
        {
            if (EnemyAction)
            {
                if (EnemyUnitCount >= unitManager.GetEnemies().Length)
                {
                    EnemyUnitCount = 0;
                    EnemyTurn = false;
                    EnemyAction = false;
                    PlayerTurn = false;
                    PlayUnitCount = 0;
                    turnCountTimer = 2;
                    /*
                    foreach (Player player in unitManager.GetPlayerList())
                    {
                        if (player.Body.CurrentPartsHp > 0)
                        {
                            PlayerUnit = player;
                            TargetCursor.instance.SetCursor(PlayerUnit);
                            break;
                        }
                        PlayUnitCount++;
                    }
                    */
                }
                if (EnemyAction)
                {
                    if (unitManager.GetEnemy(EnemyUnitCount).Body.CurrentPartsHp > 0)
                    {
                        enemyUnit = unitManager.GetEnemy(EnemyUnitCount);
                        enemyUnit.StatAction();
                        EnemyAction = false;
                    }
                    EnemyUnitCount++;
                }
            }
        }
    }

    /// <summary>
    /// ユニット移動（仮
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void UnitMoveStart(int x, int y)
    {
        if (!MoveFinish)
        {
            PlayerUnit.UnitMove(PlayerUnit.CurrentPosX, PlayerUnit.CurrentPosZ);
            PlayerUnit.UnitMove2(map.MoveList2, x, y);
        }
    }

    /// <summary>
    /// ユニット初期位置転送
    /// </summary>
    public void UnitMoveReturn()
    {
        PlayerUnit.UnitMove(PlayerUnit.CurrentPosX, PlayerUnit.CurrentPosZ);
    }

    /// <summary>
    /// 全ユニットの位置を保存
    /// </summary>
    public void SetUnitPos()
    {
        turnCountTimer = 2;
        stageUnitsPos.Clear();
        foreach (Unit unit in stageUnits)
        {
            SetUnitPos(unit);
        }
    }
    /// <summary>
    /// ユニットの位置を保存
    /// </summary>
    /// <param name="unit"></param>
    private void SetUnitPos(Unit unit)
    {
        int x = unit.CurrentPosX;
        int z = unit.CurrentPosZ;
        int[] xz = { x, z };
        stageUnitsPos.Add(xz);
    }

    public void MoveStart()
    {
        if (!MoveNow && !MoveFinish && !EnemyTurn && turnCountTimer <= 0)
        {
            if (!PlayerMoveMode)
            {
                PlayerMoveMode = true;
                map.StartSearch2(PlayerUnit);
                PlayerUnit.UnitMoveList2(map.MoveList2);
            }
        }
    }

    public void AttackStart()
    {
        if (!MoveFinish && !EnemyTurn && turnCountTimer <= 0)
        {
            if (!MoveNow)
            {
                PlayerUnit.RArmTargetShot(TargetCursor.instance.TargetUnit);
                UnitMoveFinish();
            }
            else
            {
                MoveNow = false;
                MoveFinish = true;
                PlayerMoveMode = false;
                PlayerTurn = false;
                UnitMoveReturn();
                PlayerUnit.TargetShot(TargetCursor.instance.TargetUnit);
            }
        }
    }

    public void UnitMoveFinish()
    {
        if (!MoveNow && !MoveFinish && turnCountTimer <= 0)
        {
            MoveFinish = true;
            PlayerMoveMode = false;
            PlayerTurn = false;
            PlayerUnit.ActionTurn = false;
        }
    }

    public void UnitC()
    {
    }

    private void Fupdete()
    {
        //個別ユニットターン開始
        //スイッチ表示フラグ待ち
        //移動Ｆｌａｇ
        //移動終了待ち
        //終了、キャンセルor待機or攻撃
        //攻撃Ｆｌａｇ
        //攻撃終了待ち
        //終了、待機
        //待機Ｆｌａｇ
        //方向設定、ターン終了
        //
        //次期ユニット検索、設定
        bool uiOn = true;
        if (true)
        {
            MoveStart();
        }
        if (true)
        {
            //移動終了
            uiOn = true;
        }
        if (true)
        {
            AttackStart();
        }
        if (true)
        {
            //攻撃終了
        }
        if (true)
        {
            //方向変更
            uiOn = true;
        }
        if (true)
        {
            //変更終了
            //Next
        }
    }

}
