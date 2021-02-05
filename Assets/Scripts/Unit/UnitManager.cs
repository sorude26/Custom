using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [SerializeField]
    Player[] playerList = null;
    [SerializeField]
    Enemy[] enemyList = null;
    Obstacle[] obstaclesList = null;
    public static List<int[]> UnitDetaList { get; private set; }//パーツデータ反映用
    private void Awake()
    {
        Instance = this;        
        obstaclesList = FindObjectsOfType<Obstacle>();
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

    public Obstacle[] GetObstacles()
    {
        return obstaclesList;
    }
}
