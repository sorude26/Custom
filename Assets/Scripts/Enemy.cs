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
    /*

    [SerializeField]
    EnemyAI enemyAI = EnemyAI.Attacker;
    public bool ActionNow { get; private set; } = false;

    private bool search = false;

    private List<Target> targetList;

    private void Update()
    {
        if (ActionNow)
        {
            if (!search)
            {
                gameMap.StartSearch(this);
                search = true;
            }
            if (enemyAI == EnemyAI.Attacker)
            {
                for (int i = 0; i < gameMap.maxX; i++)
                {
                    for (int j = 0; j < gameMap.maxZ; j++)
                    {
                        if (gameMap.MoveList[i][j].movePoint > 0)
                        {
                            foreach (Player target in unitManager.GetPlayerList())
                            {
                                Vector3 dir = target.transform.position - new Vector3(i * gameMap.mapScale, gameMap.MoveList[i][j].Level, j * gameMap.mapScale);
                                if (dir.sqrMagnitude <= DetectionRange * DetectionRange)
                                {
                                    int point = movePower - gameMap.MoveList[i][j].movePoint;
                                    point += (target.GetMaxHp() - target.CurrentHp) * 10;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    */
}
