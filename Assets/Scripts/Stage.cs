using System.Collections;
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
    public bool EnemyTurn { get; private set; }

    public int PlayUnitCount { get; private set; } = 0;
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
        SetUnitPos();
        PlayerUnit = unitManager.GetPlayer(0);
        PlayerTurn = true;
        EnemyTurn = false;
    }

    void Update()
    {

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
        if (!PlayerTurn)
        {
            PlayUnitCount++;
            if (PlayUnitCount >= unitManager.GetPlayerList().Length)
            {
                PlayerTurn = true;
                PlayUnitCount = 0;
                PlayerUnit = unitManager.GetPlayer(PlayUnitCount);
            }
            else
            {
                PlayerTurn = true;
                PlayerUnit = unitManager.GetPlayer(PlayUnitCount);
            }
        }

        if (EnemyTurn)
        {
            
        }
    }

    /// <summary>
    /// ユニット移動（仮
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void UnitMoveStart(int x, int y)
    {
        PlayerUnit.UnitMove(PlayerUnit.CurrentPosX, PlayerUnit.CurrentPosZ);
        PlayerUnit.UnitMove(map.MoveList, x, y);
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
        if (!MoveNow)
        {
            if (!PlayerMoveMode)
            {
                PlayerMoveMode = true;
                map.StartSearch(PlayerUnit);
                PlayerUnit.UnitMoveList(map.MoveList);
            }
        }
    }

    public void AttackStart()
    {
        if (!MoveNow)
        {
            PlayerUnit.LArmTargetShot(subUnit);
            UnitMoveFinish();
        }
        else
        { 
            MoveNow = false;
            MoveFinish = true;
            PlayerMoveMode = false;
            PlayerTurn = false;
            UnitMoveReturn();
            PlayerUnit.TargetShot(subUnit);
        }
    }

    public void UnitMoveFinish()
    {
        if (!MoveNow)
        {
            MoveFinish = true;
            PlayerMoveMode = false;
            PlayerTurn = false;
        }
    }

    public void UnitC()
    {
        PlayerUnit.UnitCreate(0, 0, 0, 0, 0, 0, 0);
        subUnit.UnitCreate(0, 0, 0, 0, 0, 0, 0);
    }
}
