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
                if (!search)
                {
                    gameMap.StartSearch(this);
                    for (int i = 0; i < gameMap.maxX; i++)
                    {
                        for (int j = 0; j < gameMap.maxZ; j++)
                        {
                            if (gameMap.MoveList[i][j].movePoint > 0)
                            {
                                int a = 0;
                                foreach (Player target in unitManager.GetPlayerList())
                                {
                                    int point = 0;
                                    Vector3 dir = target.transform.position - new Vector3(i * gameMap.mapScale, gameMap.MoveList[i][j].Level, j * gameMap.mapScale);
                                    if (dir.sqrMagnitude <= DetectionRange * DetectionRange)
                                    {
                                        point += (movePower - gameMap.MoveList[i][j].movePoint) * 10;
                                        point += (target.GetMaxHp() - target.CurrentHp) * 100;
                                        point -= a;
                                        if (a != 0)
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
                                    a++;
                                }
                            }
                        }
                    }
                    search = true;
                }
                if (targetUnit != null)
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
            if (attack)
            {
                if (targetUnit != null)
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
}
