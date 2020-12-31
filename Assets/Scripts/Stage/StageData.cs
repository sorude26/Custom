using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    City1,
    Forest1,
    Wasteland,
    Factory1,
}

public enum StageID
{
    Stage1,
    Stage2,
    Stage3,
}
public class StageData : MonoBehaviour
{
    public StageType Type { get; set; }
    public float Level { get; private set; }
    public Map.MapType MapTypeData { get; private set; }
    public int PlayerNumbers { get; private set; }
    public int EnemyNumber { get; private set; }
    public Map.MapType StageDataGet(int x, int z)
    {
        if(Type == StageType.Factory1)
        {
            MapTypeData = Map.MapType.Asphalt;
        }
        return MapTypeData;
    }
    public float StageLevelData(int x,int z)
    {
        Level = 0;
        if (x >= 10 && z >= 0 && z <= 9)
        {
            Level = 2.5f;
        }
        if (z >= 10)
        {
            Level = 4.6f;
        }
        if (x == 9 && z >= 1 && z <= 2)
        {
            Level = 1.0f;
        }
        if (x <= 1 && z == 8)
        {
            Level = 1.5f;
        }
        if (x <= 1 && z == 9)
        {
            Level = 3.0f;
        }
        if (x >= 11 && x <= 12 && z == 9)
        {
            Level = 3.5f;
        }
        return Level;
    }
}
