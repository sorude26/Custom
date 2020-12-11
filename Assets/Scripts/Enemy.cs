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
            UnitCreate(0,0,0,0,0,0,0);
        }
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
                Vector3 thisPos = transform.position;
                //Debug.Log(thisPos.x + "," + thisPos.z);
                if (CurrentPosX != (int)Math.Round(thisPos.x) / gameMap.mapScale || CurrentPosZ != (int)Math.Round(thisPos.z) / gameMap.mapScale)
                {
                    CurrentPosX = (int)Math.Round(thisPos.x) / gameMap.mapScale;
                    CurrentPosZ = (int)Math.Round(thisPos.z) / gameMap.mapScale;
                    CurrentPosY = gameMap.MapDates[CurrentPosX][CurrentPosZ].Level;
                    attack = true;
                }
                //Debug.Log(CurrentPosX +"," + CurrentPosZ);
                gameStage.SetUnitPos();
            }
            UnitAngleControl();
            if (attack)//攻撃指示実行後ターゲット含めリセットし行動終了
            {
                Debug.Log("待機");
                if (Target != null)//ターゲットが存在する場合に攻撃
                {
                    Debug.Log("攻撃");
                    LArmTargetShot(Target.TargetUnit);
                }
                Target = null;
                search = false;
                move = false;
                attack = false;
                ActionNow = false;
            }
        }
        
    }
    private void LateUpdate()
    {
        if (silhouetteOn)
        {
            CurrentHp = Body.CurrentPartsHp + Head.CurrentPartsHp + LArm.CurrentPartsHp + RArm.CurrentPartsHp + Leg.CurrentPartsHp;
            if (Body.CurrentPartsHp <= 0)
            {
                Dead();
            }
        }
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
                                    point += gameMap.MoveList[i][j].movePoint * 10;//移動量が少ない場合に高得点
                                    point += (target.GetMaxHp() - target.CurrentHp) * 100;//ターゲットの耐久値の減少量が大きい場合に高得点
                                    point -= number;//ターゲットの登録順で得点に差
                                    if (Target != null)//ターゲットが登録済みか判断し、登録済みのターゲットポイントと比較、高ポイントならば新規登録
                                    {
                                        if (point > Target.TargetPoint)
                                        {
                                            Target = new Target(target, point, i, j);
                                        }
                                    }
                                    else
                                    {
                                        Target = new Target(target, point, i, j);
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
            search = false;
            ActionNow = false;
        }
    }
}
