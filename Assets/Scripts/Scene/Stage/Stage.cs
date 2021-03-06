﻿using System.Collections;
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
    private bool defeat;
    private bool setData;
    [SerializeField] int goalPositionX = 0;
    [SerializeField] int goalPositionZ = 0;
    public Player PlayerUnit { get; private set; }
    public Enemy EnemyUnit { get; private set; }
    [SerializeField]
    private Enemy targetEnemy = null;
    [SerializeField]
    private Enemy[] targetEnemies;
    private UnitManager unitManager;
    public List<Unit> stageUnits;
    public List<int[]> stageUnitsPos = new List<int[]>();
    public Map map;
    public static Stage Instance { get; private set; }
    public bool PlayerMoveMode { get; private set; } = false;
    public bool MoveFinish { get; set; } = false;
    public bool MoveNow { get; private set; }
    public bool PlayerTurn { get; private set; }
    public bool EnemyTurn { get; set; }
    public bool EnemyAction { get; set; }
    private bool victoryCheck;
    public float turnCountTimer = 2;
    public int PlayerUnitCount { get; private set; } = 0;
    public int PlayerDestroyCount { get; set; } = 0;
    public int EnemyUnitCount { get; private set; } = 0;
    public int EnemyDestroyCount { get; set; } = 0;
    bool start = false;
    [SerializeField]
    public ParameterPanel panelP;
    [SerializeField]
    public ParameterPanel panelE;
    [SerializeField]
    StageMessage stageMessage;
    public Weapon PlayerAttackWeapon { get; private set; } = null;
    public int StageReward { get; private set; } = 0;
    public int LossReward { get; private set; } = 0;
    private bool sceneChange =false;
    [SerializeField]
    GameObject actionButton;
    private void Awake()
    {
        Instance = this;        
    }

    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMType.Stage1);
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
        int i = 0;
        foreach (var item in GameManager.SortieUnit)
        {
            if (item.Sortie)
            {
                break;
            }
            i++;
        }
        PlayerUnit = unitManager.GetPlayer(i);
        PlayerTurn = false;
        EnemyTurn = false;
        TargetCursor.instance.SetCursor(PlayerUnit);
        PlayerAttackWeapon = PlayerUnit.LArmWeapon;
        turnCountTimer = 3;
        actionButton.SetActive(false);
    }

    void Update()
    {
        if (!start && turnCountTimer <= 0)
        {
            start = true;
            panelP.SetUnit(PlayerUnit);
            turnCountTimer = 2;
            switch (victory)
            {
                case VictoryConditions.AllDestroy:
                    stageMessage.ViewMessage(5, 2.0f);
                    break;
                case VictoryConditions.TargetNumberBreak:
                    stageMessage.ViewMessage(6, 2.0f);
                    CameraControl.Instance.UnitCamera(targetEnemies[0]);
                    break;
                case VictoryConditions.TargetBreak:
                    stageMessage.ViewMessage(6, 2.0f);                   
                    CameraControl.Instance.UnitCamera(targetEnemy);
                    break;
                case VictoryConditions.Survive:
                    break;
                case VictoryConditions.GoalPosition:
                    stageMessage.ViewMessage(8, 2.0f);
                    CameraControl.Instance.PointCamera(goalPositionX, goalPositionZ);
                    break;
                default:
                    break;
            }

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
        if (!PlayerTurn && !EnemyTurn && turnCountTimer <= 0 && start && !Victory && !defeat)
        {
            actionButton.SetActive(true);
            PlayerTurn = true;
            PlayerTurnSystem();
        }
        if (EnemyTurn && turnCountTimer <= 0 && !Victory && !defeat)
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
                    stageMessage.ViewMessage(1, 1.0f);
                }
                if (EnemyAction)
                {
                    if (unitManager.GetEnemy(EnemyUnitCount).Body.CurrentPartsHp > 0)
                    {
                        EnemyUnit = unitManager.GetEnemy(EnemyUnitCount);
                        CameraControl.Instance.UnitCamera(EnemyUnit);
                        EnemyUnit.StatAction();
                        EnemyAction = false;
                        panelE.SetUnit(EnemyUnit);                        
                    }
                    EnemyUnitCount++;
                    actionButton.SetActive(false);
                }
            }
        }
        VictoryConditionsCheck();
    }

    public void PlayerTurnSystem()
    {
        BattleEnd();
        if (Victory || defeat)
        {
            return;
        }
        victoryCheck = true;
        if (PlayerUnitCount > unitManager.GetPlayerList().Length)
        {
            PlayerUnitCount = 0;
            EnemyTurn = true;
            PlayerTurn = false;
            EnemyAction = true;
            turnCountTimer = 2;
            foreach (Unit unit in unitManager.GetPlayerList())
            {
                unit.MoveFinishSet();
            }
            stageMessage.ViewMessage(2, 1.0f);
            actionButton.SetActive(false);
            return;
        }
        if (!unitManager.GetPlayer(PlayerUnitCount).DestroyBody)
        {
            PlayerUnit.MoveFinishSet();
            PlayerUnit = unitManager.GetPlayer(PlayerUnitCount);
            TargetCursor.instance.SetCursor(PlayerUnit);
            CameraControl.Instance.UnitCamera(PlayerUnit);
            panelP.SetUnit(PlayerUnit);
            PlayerUnit.ActionTurn = true;
            PlayerUnitCount++;
            if (PlayerUnitCount > unitManager.GetPlayerList().Length)
            {
                PlayerUnitCount = 0;
                EnemyTurn = true;
                PlayerTurn = false;
                EnemyAction = true;
                turnCountTimer = 2;
                foreach (Unit unit in unitManager.GetPlayerList())
                {
                    unit.MoveFinishSet();
                }
                stageMessage.ViewMessage(2, 1.0f);
                actionButton.SetActive(false);
                return;
            }
        }
        else
        {
            PlayerUnitCount++;
            PlayerTurnSystem();
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
        if (!MoveFinish && !MoveNow)
        {
            PlayerUnit.UnitMove(PlayerUnit.CurrentPosX, PlayerUnit.CurrentPosZ);
            PlayerUnit.UnitMove2(map.MoveList2, PlayerUnit.CurrentPosX, PlayerUnit.CurrentPosZ);
        }
    }

    /// <summary>
    /// 全ユニットの位置を保存
    /// </summary>
    public void SetUnitPos()
    {
        turnCountTimer = 1;
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
                BattleStart();
                PlayerUnit.Attack();
                PlayerUnit.TargetShot(PlayerUnit.TargetEnemy, PlayerAttackWeapon);
                UnitMoveFinish();
                turnCountTimer = 2;
            }
        }
    }

    public void UnitMoveFinish()
    {
        if (!MoveNow && !MoveFinish && turnCountTimer <= 0)
        {
            MoveFinish = true;
            PlayerMoveMode = false;
            turnCountTimer = 1;
        }
    }
    public void BattleStart()
    {
        panelP.BattleMoveUp();
        panelE.BattleMoveDown();
    }
    public void BattleEnd()
    {
        panelP.BattleEnd();
        panelE.BattleEnd();
    }
    private float victoryTimer = 0;
    public bool viewGameOver;
    private void VictoryConditionsCheck()
    {
        if (!defeat && PlayerDestroyCount == unitManager.GetPlayerList().Length)
        {
            defeat = true;
            Debug.Log("敗北");
            stageMessage.ViewMessage(4, 5.0f);
        }
        if (defeat && !Victory)
        {
            if (!viewGameOver)
            {
                victoryTimer += 1.0f * Time.deltaTime;
                if (victoryTimer > 8.0f)
                {
                    viewGameOver = true;
                }
            }
        }
        if (Victory && !sceneChange)
        {
            if (!setData && victoryTimer > 8.0f)
            {
                GameManager.StageScoreData.StageName = map.data.GetStageName(GameManager.StageCode);
                GameManager.StageScoreData.StageReward = map.data.GetReward(GameManager.StageCode);
                GameManager.StageScoreData.EnemyNumber = EnemyDestroyCount;
                GameManager.StageScoreData.EnemyReward = StageReward;
                GameManager.StageScoreData.TotalLoss = LossReward;
                setData = true;
            }
            else if (setData)
            {
                GameManager.Instance.StartChange(4);
                sceneChange = true;
            }
            victoryTimer += 1.0f * Time.deltaTime;
        }
        if (!defeat && !Victory)
        {
            switch (victory)
            {
                case VictoryConditions.AllDestroy:
                    if (EnemyDestroyCount == unitManager.GetEnemies().Length)
                    {
                        Victory = true;
                        stageMessage.ViewMessage(3, 5.0f);
                    }
                    break;
                case VictoryConditions.TargetNumberBreak:
                    if (victoryCheck)
                    {
                        victoryCheck = false;
                        foreach (var unti in targetEnemies)
                        {
                            if (!unti.DestroyBody)
                            {
                                return;
                            }
                        }
                        Victory = true;
                        stageMessage.ViewMessage(3, 5.0f);
                    }
                    break;
                case VictoryConditions.TargetBreak:
                    if (targetEnemy.DestroyBody)
                    {
                        Victory = true;
                        stageMessage.ViewMessage(3, 5.0f);
                    }
                    break;
                case VictoryConditions.Survive:
                    break;
                case VictoryConditions.GoalPosition:
                    if (victoryCheck)
                    {
                        victoryCheck = false;
                        foreach (var unit in unitManager.GetPlayerList())
                        {
                            if (unit.CurrentPosX == goalPositionX && unit.CurrentPosZ == goalPositionZ)
                            {
                                Victory = true;
                                stageMessage.ViewMessage(3, 5.0f);
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            if (Victory)
            {
                SoundManager.Instance.PlayBGM(BGMType.Result);
            }
        }
    }

    public void SetPlayerAttackWeapon(Weapon attackWeapon)
    {
        PlayerAttackWeapon = attackWeapon;
    }

    public void RewardAdd(int money)
    {
        StageReward += money;
    }
    
    public void LossAdd(int money)
    {
        LossReward += money;
    }
}
