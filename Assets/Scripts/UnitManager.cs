using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    Player[] playerList = null;
    Enemy[] enemyList = null;
    private void Awake()
    {
        Instance = this;
        playerList = FindObjectsOfType<Player>();
        enemyList = FindObjectsOfType<Enemy>();
    }
    public Player GetPlayer(int i)
    {
        if (i >= playerList.Length || i < 0)
        {
            return playerList[0];
        }
        return playerList[i];
    }
    public Player[] GetPlayerList()
    {
        return playerList;
    }

    public Enemy GetEnemy(int i)
    {
        if (i >= enemyList.Length || i < 0)
        {
            return enemyList[0];
        }
        return enemyList[i];
    }
    public Enemy[] GetEnemies()
    {
        return enemyList;
    }
}
