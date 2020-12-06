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

    private Target targetUnit = null;

    private void Update()
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
                Vector3 thisPos = transform.position;
                if (CurrentPosX != (int)thisPos.x / gameMap.mapScale || CurrentPosZ != (int)thisPos.z / gameMap.mapScale)
                {
                    CurrentPosX = (int)thisPos.x / gameMap.mapScale;
                    CurrentPosZ = (int)thisPos.z / gameMap.mapScale;
                    CurrentPosY = gameMap.MapDates[CurrentPosX][CurrentPosZ].Level;
                }
                gameStage.SetUnitPos();
                attack = true;
            }
            UnitAngleControl();
            if (attack)//攻撃指示実行後ターゲット含めリセットし行動終了
            {
                if (targetUnit != null)//ターゲットが存在する場合に攻撃
                {
                    TargetShot(targetUnit.TargetUnit);
                }
                targetUnit = null;
                search = false;
                move = false;
                attack = false;
                ActionNow = false;
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
                            if (target.CurrentHp > 0)
                            {
                                int point = 0;
                                Vector3 dir = target.transform.position - new Vector3(i * gameMap.mapScale, gameMap.MoveList[i][j].Level, j * gameMap.mapScale);
                                if (dir.sqrMagnitude <= DetectionRange * DetectionRange)
                                {
                                    point += (movePower - gameMap.MoveList[i][j].movePoint) * 10;//移動量が多い場合に高得点
                                    point += (target.GetMaxHp() - target.CurrentHp) * 100;//ターゲットの耐久値の減少量が大きい場合に高得点
                                    point -= number;//ターゲットの登録順で得点に差
                                    if (targetUnit != null)//ターゲットが登録済みか判断し、登録済みのターゲットポイントと比較、高ポイントならば新規登録
                                    {
                                        if (point > targetUnit.TargetPoint)
                                        {
                                            targetUnit = new Target(target, point, i, j);
                                        }
                                    }
                                    else
                                    {
                                        targetUnit = new Target(target, point, i, j);
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
        if (targetUnit != null)//ターゲットが設定されているならば移動実施
        {
            if (!move)
            {
                UnitMove(gameMap.MoveList, targetUnit.PosX, targetUnit.PosZ);
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
