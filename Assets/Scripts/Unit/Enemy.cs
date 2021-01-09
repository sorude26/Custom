using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAI
{
    Attacker,
    Sniper,
    Guardian,
    Commander,
}
public class Enemy : Unit
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


    private void Update()
    {
        if (!silhouetteOn && !DestroyBody)
        {
            UnitCreate(1, 3, 3, 0, 3, 4, 1);
        }
        if (silhouetteOn)
        {
            if (ActionNow)
            {
                if (enemyAI == EnemyAI.Attacker)
                {
                    ActionTypeAttacker();
                }
                if (moveMood)
                {
                    UnitMove();
                }
                if (move && !moveMood && !attack)//移動終了で位置を保存
                {
                    MoveFinishSet();
                    attack = true;
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
        if (attack)//攻撃指示実行後ターゲット含めリセットし行動終了
        {
            if (Target != null && !attackMode)//ターゲットが存在する場合に攻撃
            {
                gameStage.panelP.SetUnit(Target.TargetUnit);
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
            if (!attackTrigger)
            {
                Target = null;
                search = false;
                move = false;
                attack = false;
                ActionNow = false;
                gameStage.EnemyAction = true;
                gameStage.turnCountTimer = 2;
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
                                if (LArm.CurrentPartsHp >0)
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
                                else if(RArm.CurrentPartsHp > 0)
                                {
                                    weapon1 = RArmWeapon;                                    
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
}
