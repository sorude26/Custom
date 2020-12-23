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

    public Target Target { get; private set; } = null;


    private void Update()
    {
        if (!silhouetteOn)
        {
            UnitCreate(0, 0, 0, 2, 0, 2, 0);
        }
        if (silhouetteOn)
        {
            if (ActionNow)
            {
                if (enemyAI == EnemyAI.Attacker)
                {
                    ActionTypeAttacker2();
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
                if (attack)//攻撃指示実行後ターゲット含めリセットし行動終了
                {
                    // Debug.Log("待機");
                    if (Target != null && !attackMode)//ターゲットが存在する場合に攻撃
                    {
                        // Debug.Log("攻撃");
                        Vector3 dir = Target.TargetUnit.transform.position - transform.position;
                        if (dir.sqrMagnitude <= LArmWeapon.Range * LArmWeapon.Range)
                        {
                            TargetCursor.instance.SetCursor(Target.TargetUnit);
                            attackTrigger = true;
                            TargetShot(Target.TargetUnit, LArm);
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
            if (DestroyBody)
            {
                gameObject.SetActive(false);
                gameStage.EnemyDestroyCount++;
                silhouetteOn = false;
            }
        }


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

    private void ActionTypeAttacker()
    {
        if (!search)
        {
            gameMap.StartSearch(this);
            for (int i = 0; i < gameMap.maxX; i++)
            {
                for (int j = 0; j < gameMap.maxZ; j++)
                {
                    if (gameMap.MoveList[i][j].movePoint > 0)
                    {
                        int number = 0;
                        foreach (Player target in unitManager.GetPlayerList())//ユニットが移動後の索敵範囲にいるか検索
                        {
                            if (!target.DestroyBody)
                            {
                                int point = 0;
                                Vector3 dir = target.transform.position - new Vector3(i * gameMap.mapScale, gameMap.MoveList[i][j].Level, j * gameMap.mapScale);
                                if (dir.sqrMagnitude <= DetectionRange * DetectionRange)
                                {
                                    float distance = DetectionRange * DetectionRange - dir.sqrMagnitude;
                                    if (dir.sqrMagnitude <= LArmWeapon.EffectiveRange * LArmWeapon.EffectiveRange)
                                    {
                                        point += 2000;
                                        point += gameMap.MoveList[i][j].movePoint * 20;//移動量が少ない場合に高得点
                                    }
                                    else
                                    {
                                        point += (movePower - gameMap.MoveList[i][j].movePoint) * 10;//移動量が大きい場合に高得点
                                    }
                                    point += (target.GetMaxHp() - target.CurrentHp) * 30;//ターゲットの耐久値の減少量が大きい場合に高得点
                                    point -= number;//ターゲットの登録順で得点に差
                                    if (Target != null)//ターゲットが登録済みか判断し、登録済みのターゲットポイントと比較、高ポイントならば新規登録
                                    {
                                        if (Target.Distance > distance)
                                        {
                                            point += (movePower - gameMap.MoveList[i][j].movePoint) * 10;
                                        }
                                        if (point > Target.TargetPoint)
                                        {
                                            Target = new Target(target, point, i, j, distance);
                                        }
                                    }
                                    else
                                    {
                                        Target = new Target(target, point, i, j, distance);
                                    }
                                }
                            }
                            number++;
                        }
                    }
                }
            }
            search = true;
        }
        if (Target != null)//ターゲットが設定されているならば移動実施
        {
            if (!move)
            {
                UnitMove(gameMap.MoveList, Target.PosX, Target.PosZ);
                move = true;
            }
        }
        else
        {
            attack = true;
        }
    }
    private void ActionTypeAttacker2()
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
                                if (dir.sqrMagnitude <= LArmWeapon.EffectiveRange * LArmWeapon.EffectiveRange)
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
                                point -= distance;
                                if (Target != null)//ターゲットが登録済みか判断し、登録済みのターゲットポイントと比較、高ポイントならば新規登録
                                {
                                    if (point > Target.TargetPoint)
                                    {
                                        Target = new Target(target, point, mapDate.PosX, mapDate.PosZ, distance);
                                    }
                                }
                                else
                                {
                                    Target = new Target(target, point, mapDate.PosX, mapDate.PosZ, distance);
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
