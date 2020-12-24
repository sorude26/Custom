using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VictoryConditions
{
    AllDestroy,
    TargetNumberBreak,
    TargetBreak,
    Survive,
    GoalPosition,
}
public class Stage : MonoBehaviour
{
    [SerializeField]
    VictoryConditions victory = VictoryConditions.AllDestroy;
    public bool Victory { get; private set; } = false;
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
    public int PlayerUnitCount { get; private set; } = 0;
    public int PlayerDestroyCount { get; set; } = 0;
    public int EnemyUnitCount { get; private set; } = 0;
    public int EnemyDestroyCount { get; set; } = 0;
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
            if (!unitManager.GetPlayer(PlayerUnitCount).DestroyBody)
            {
                PlayerUnit.MoveFinishSet();
                PlayerUnit = unitManager.GetPlayer(PlayerUnitCount);
                TargetCursor.instance.SetCursor(PlayerUnit);
                PlayerTurn = true;
                CameraControl.Instans.UnitCamera(PlayerUnit);
            }
            PlayerUnitCount++;            
            if (PlayerUnitCount > unitManager.GetPlayerList().Length)
            {
                PlayerUnitCount = 0;
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
                    PlayerUnitCount = 0;
                    turnCountTimer = 2;
                }
                if (EnemyAction)
                {
                    if (unitManager.GetEnemy(EnemyUnitCount).Body.CurrentPartsHp > 0)
                    {
                        enemyUnit = unitManager.GetEnemy(EnemyUnitCount);
                        CameraControl.Instans.UnitCamera(enemyUnit);
                        enemyUnit.StatAction();
                        EnemyAction = false;
                    }
                    EnemyUnitCount++;
                }
            }
        }
        VictoryConditionsCheck();
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

    private void VictoryConditionsCheck()
    {
        switch (victory)
        {
            case VictoryConditions.AllDestroy:
                if (EnemyDestroyCount == unitManager.GetEnemies().Length && !Victory)
                {
                    Victory = true;
                    Debug.Log("勝利");
                }
                break;
            case VictoryConditions.TargetNumberBreak:
                break;
            case VictoryConditions.TargetBreak:
                break;
            case VictoryConditions.Survive:
                break;
            case VictoryConditions.GoalPosition:
                break;
            default:
                break;
        }
        if (!Victory && PlayerDestroyCount == unitManager.GetPlayerList().Length)
        {
            Victory = true;
            Debug.Log("敗北");
        }
    }




}
