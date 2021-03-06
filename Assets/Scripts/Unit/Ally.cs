﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllyAI
{
    Attacker,
    Sniper,
    Guardian,
    Commander,
    Object,
}
public class Ally : Unit
{

    [SerializeField]
    EnemyAI enemyAI = EnemyAI.Attacker;
    public bool ActionNow { get; private set; } = false;

    private bool search = false;
    private bool move = false;
    private bool attack = false;

    private Weapon weapon1 = null;
    private Weapon weapon2 = null;
    public Target Target { get; private set; } = null;

    [SerializeField]
    int enemyType = 0;


    void Update()
    {
        if (!silhouetteOn && !DestroyBody)
        {
            switch (enemyType)
            {
                case 0:
                    UnitCreate(5, 3, 10);
                    break;
                case 1:
                    UnitCreate(4, 9);
                    break;
                case 2:
                    UnitCreate(2, 2, 2, 3, 2, 6, 2);
                    break;
                case 3:
                    UnitCreate(1);
                    break;
                default:
                    break;
            }
        }
        if (silhouetteOn)
        {
            if (ActionNow)
            {
                if (enemyAI == EnemyAI.Attacker)
                {
                    ActionTypeAttacker();
                }
                else if (enemyAI == EnemyAI.Sniper)
                {
                    ActionTypeSniper();
                }
                if (moveMood)
                {
                    UnitMove();
                }
                if (move && !moveMood && !attack)//移動終了で位置を保存
                {
                    MoveFinishSet();
                    attack = true;
                    CameraControl.Instance.UnitCamera(this);
                }
                UnitAngleControl();
                AttackMood();
            }
            if (DestroyBody)
            {
                gameStage.EnemyDestroyCount++;
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
    public void StatAction()
    {
        ActionNow = true;
    }
    private void AttackMood()
    {
        if (attack && !moveMotionStart)//攻撃指示実行後ターゲット含めリセットし行動終了
        {
            if (Target != null && !attackMode)//ターゲットが存在する場合に攻撃
            {
                gameStage.panelP.SetUnit(Target.TargetUnit);
                gameStage.BattleStart();
                Vector3 dir = Target.TargetUnit.transform.position - transform.position;
                if (dir.sqrMagnitude <= weapon1.Range * weapon1.Range)
                {
                    if (weapon2 != null)
                    {
                        if (weapon2.Power * weapon2.TotalShotNumber > weapon1.Power * weapon1.TotalShotNumber && dir.sqrMagnitude <= weapon2.EffectiveRange * weapon2.EffectiveRange)
                        {
                            TargetCursor.instance.SetCursor(Target.TargetUnit);
                            attackTrigger = true;
                            TargetShot(Target.TargetUnit, weapon2);
                        }
                        else
                        {
                            TargetCursor.instance.SetCursor(Target.TargetUnit);
                            attackTrigger = true;
                            TargetShot(Target.TargetUnit, weapon1);
                        }
                    }
                    else
                    {
                        TargetCursor.instance.SetCursor(Target.TargetUnit);
                        attackTrigger = true;
                        TargetShot(Target.TargetUnit, weapon1);
                    }
                    Target = null;
                }
            }
            if (!attackTrigger && !attackMode)
            {
                Target = null;
                search = false;
                move = false;
                attack = false;
                ActionNow = false;
                gameStage.EnemyAction = true;
                gameStage.turnCountTimer = 2;
                gameStage.BattleEnd();
                CameraControl.Instance.UnitCameraMove(this);
            }
        }
    }
    private void ActionTypeAttacker()
    {
        if (!search)
        {
            gameMap.StartSearch2(this);
            foreach (Map.MapDate mapDate in gameMap.MoveList2)
            {
                if (mapDate.movePoint > 0)
                {
                    bool unitOn = false;
                    if (Body.unitType == UnitType.Helicopter)
                    {
                        foreach (Unit unit in gameStage.stageUnits)
                        {
                            if (mapDate.PosX == unit.CurrentPosX && mapDate.PosZ == unit.CurrentPosZ && !unit.DestroyBody)
                            {
                                unitOn = true;
                                break;
                            }
                        }
                    }
                    if (!unitOn)
                    {
                        int number = 0;
                        foreach (Player target in unitManager.GetPlayerList())//ユニットが移動後の索敵範囲にいるか検索
                        {
                            if (!target.DestroyBody)
                            {
                                float point = 0;
                                Vector3 dir = target.transform.position - new Vector3(mapDate.PosX * gameMap.mapScale, mapDate.Level, mapDate.PosZ * gameMap.mapScale);
                                if (dir.sqrMagnitude <= DetectionRange * DetectionRange)
                                {
                                    float distance = dir.sqrMagnitude;
                                    weapon1 = null;
                                    weapon2 = null;
                                    if (Body.unitType == UnitType.Human)
                                    {
                                        if (LArm.CurrentPartsHp > 0)
                                        {
                                            weapon1 = LArmWeapon;
                                            if (RArm.CurrentPartsHp > 0)
                                            {
                                                weapon2 = RArmWeapon;
                                            }
                                            if (weapon2 != null)
                                            {
                                                if (weapon1.EffectiveRange < weapon2.EffectiveRange)
                                                {
                                                    weapon1 = RArmWeapon;
                                                    weapon2 = LArmWeapon;
                                                }
                                            }
                                        }
                                        else if (RArm.CurrentPartsHp > 0)
                                        {
                                            weapon1 = RArmWeapon;
                                        }
                                    }
                                    else if (Body.unitType == UnitType.Helicopter || Body.unitType == UnitType.Tank)
                                    {
                                        weapon1 = LArmWeapon;
                                    }
                                    if (weapon1 != null)
                                    {
                                        if (dir.sqrMagnitude <= weapon1.EffectiveRange * weapon1.EffectiveRange)
                                        {
                                            point += 10000;
                                            point += mapDate.movePoint * 20;//移動量が少ない場合に高得点
                                        }
                                        else
                                        {
                                            point += (movePower - mapDate.movePoint) * 10;//移動量が大きい場合に高得点
                                        }
                                        point += (target.GetMaxHp() - target.CurrentHp) * 10;//ターゲットの耐久値の減少量が大きい場合に高得点
                                        point -= number;//ターゲットの登録順で得点に差
                                        point -= distance;//距離が短いほど高得点
                                        if (Target != null)//ターゲットが登録済みか判断し、登録済みのターゲットポイントと比較、高ポイントならば新規登録
                                        {
                                            if (point > Target.TargetPoint)
                                            {
                                                Target = new Target(target, point, mapDate.PosX, mapDate.PosZ);
                                            }
                                        }
                                        else
                                        {
                                            Target = new Target(target, point, mapDate.PosX, mapDate.PosZ);
                                        }
                                    }
                                }
                            }
                            number++;
                        }
                    }
                }
            }
            CameraControl.Instance.UnitCameraMove(this);
            search = true;
        }
        if (Target != null)//ターゲットが設定されているならば移動実施
        {
            if (!move)
            {
                UnitMove2(gameMap.MoveList2, Target.PosX, Target.PosZ);
                move = true;
            }
        }
        else
        {
            attack = true;
        }
    }
    private void ActionTypeSniper()
    {
        if (!search)
        {
            gameMap.StartSearch2(this);
            foreach (Map.MapDate mapDate in gameMap.MoveList2)
            {
                if (mapDate.movePoint > 0)
                {
                    bool unitOn = false;
                    if (Body.unitType == UnitType.Helicopter)
                    {
                        foreach (Unit unit in gameStage.stageUnits)
                        {
                            if (mapDate.PosX == unit.CurrentPosX && mapDate.PosZ == unit.CurrentPosZ && !unit.DestroyBody)
                            {
                                unitOn = true;
                                break;
                            }
                        }
                    }
                    if (!unitOn)
                    {
                        int number = 0;
                        foreach (Player target in unitManager.GetPlayerList())//ユニットが移動後の索敵範囲にいるか検索
                        {
                            if (!target.DestroyBody)
                            {
                                float point = 0;
                                Vector3 dir = target.transform.position - new Vector3(mapDate.PosX * gameMap.mapScale, mapDate.Level, mapDate.PosZ * gameMap.mapScale);
                                if (dir.sqrMagnitude <= DetectionRange * DetectionRange)
                                {
                                    float distance = dir.sqrMagnitude;
                                    weapon1 = null;
                                    weapon2 = null;
                                    if (Body.unitType == UnitType.Human)
                                    {
                                        if (LArm.CurrentPartsHp > 0)
                                        {
                                            weapon1 = LArmWeapon;
                                            if (RArm.CurrentPartsHp > 0)
                                            {
                                                weapon2 = RArmWeapon;
                                            }
                                            if (weapon2 != null)
                                            {
                                                if (weapon1.EffectiveRange < weapon2.EffectiveRange)
                                                {
                                                    weapon1 = RArmWeapon;
                                                    weapon2 = LArmWeapon;
                                                }
                                            }
                                        }
                                        else if (RArm.CurrentPartsHp > 0)
                                        {
                                            weapon1 = RArmWeapon;
                                        }
                                    }
                                    else if (Body.unitType == UnitType.Helicopter || Body.unitType == UnitType.Tank)
                                    {
                                        weapon1 = LArmWeapon;
                                    }
                                    if (weapon1 != null)
                                    {
                                        if (dir.sqrMagnitude <= weapon1.EffectiveRange * weapon1.EffectiveRange)
                                        {
                                            point += 20000;
                                        }
                                        point += mapDate.movePoint * 1000;//移動量が少ない場合に高得点
                                        point += (target.GetMaxHp() - target.CurrentHp) * 10;//ターゲットの耐久値の減少量が大きい場合に高得点
                                        point -= number;//ターゲットの登録順で得点に差
                                        point -= distance;//距離が短いほど高得点
                                        if (Target != null)//ターゲットが登録済みか判断し、登録済みのターゲットポイントと比較、高ポイントならば新規登録
                                        {
                                            if (point > Target.TargetPoint)
                                            {
                                                Target = new Target(target, point, mapDate.PosX, mapDate.PosZ);
                                            }
                                        }
                                        else
                                        {
                                            Target = new Target(target, point, mapDate.PosX, mapDate.PosZ);
                                        }
                                    }
                                }
                            }
                            number++;
                        }
                    }
                }
            }
            CameraControl.Instance.UnitCameraMove(this);
            search = true;
        }
        if (Target != null)//ターゲットが設定されているならば移動実施
        {
            if (!move)
            {
                UnitMove2(gameMap.MoveList2, Target.PosX, Target.PosZ);
                move = true;
            }
        }
        else
        {
            attack = true;
        }
    }
    private void ActionTypeGuardian()
    {
        if (!search)
        {


            int number = 0;
            foreach (Player target in unitManager.GetPlayerList())//ユニットが移動後の索敵範囲にいるか検索
            {
                if (!target.DestroyBody)
                {
                    float point = 0;
                    Vector3 dir = target.transform.position - transform.position;
                    if (dir.sqrMagnitude <= DetectionRange * DetectionRange)
                    {
                        float distance = dir.sqrMagnitude;
                        weapon1 = null;
                        weapon2 = null;
                        if (Body.unitType == UnitType.Human)
                        {
                            if (LArm.CurrentPartsHp > 0)
                            {
                                weapon1 = LArmWeapon;
                                if (RArm.CurrentPartsHp > 0)
                                {
                                    weapon2 = RArmWeapon;
                                }
                                if (weapon2 != null)
                                {
                                    if (weapon1.EffectiveRange < weapon2.EffectiveRange)
                                    {
                                        weapon1 = RArmWeapon;
                                        weapon2 = LArmWeapon;
                                    }
                                }
                            }
                            else if (RArm.CurrentPartsHp > 0)
                            {
                                weapon1 = RArmWeapon;
                            }
                        }
                        else if (Body.unitType == UnitType.Helicopter || Body.unitType == UnitType.Tank)
                        {
                            weapon1 = LArmWeapon;
                        }
                        if (weapon1 != null)
                        {
                            if (dir.sqrMagnitude <= weapon1.EffectiveRange * weapon1.EffectiveRange)
                            {
                                point += 10000;
                            }
                            point += (target.GetMaxHp() - target.CurrentHp) * 10;//ターゲットの耐久値の減少量が大きい場合に高得点
                            point -= distance;//距離が短いほど高得点
                            if (Target != null)//ターゲットが登録済みか判断し、登録済みのターゲットポイントと比較、高ポイントならば新規登録
                            {
                                if (point > Target.TargetPoint)
                                {
                                    Target = new Target(target, point, target.CurrentPosX, target.CurrentPosZ);
                                }
                            }
                            else
                            {
                                Target = new Target(target, point, target.CurrentPosX, target.CurrentPosZ);
                            }
                        }
                    }
                }
                number++;
            }
            CameraControl.Instance.UnitCameraMove(this);
            search = true;
        }
        attack = true;
    }
}
