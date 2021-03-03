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
    Mountain1,
}

public enum StageID
{
    Stage0,//デバック用仮ステージ
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
    Stage6,
    Stage7,
    Stage8,
    Stage9,
    Stage10,
    Stage11,
    Stage12,
}
public class StageData : MonoBehaviour
{
    public StageType Type { get; set; }
    public float Level { get; private set; }
    public Map.MapType MapTypeData { get; private set; }
    public Map.MapType StageDataGet(int x, int z)
    {
        MapTypeData = Map.MapType.Asphalt;
        switch (Type)
        {
            case StageType.City1:
                break;
            case StageType.Forest1:
                if (x <= 8 && 3 <= z && z <= 11 || x >= 5 && x <= 13 && z >= 14 && z <= 22
                    || 13 <= x && x <= 21 && z <= 8 || 15 <= x && x <= 23 && 15 <= z && z <= 23
                    || 10 <= x && x <= 18 && 9 <= z && z <= 17)
                {
                    MapTypeData = Map.MapType.Forest;
                }                
                break;
            case StageType.Wasteland:
                break;
            case StageType.Factory1:
                MapTypeData = Map.MapType.Asphalt;
                break;
            case StageType.Mountain1:
                MapTypeData = Map.MapType.Wasteland;
                break;
            default:
                break;
        }
        return MapTypeData;
    }
    public float StageLevelData(int x,int z)
    {
        Level = 0;
        switch (Type)
        {
            case StageType.City1:
                break;
            case StageType.Forest1:
                if (x <= 8 && 3 <= z && z <= 11 || x >= 5 && x <=13 && z >=14 && z <= 22 
                    || 13 <= x && x <= 21 && z <= 8 || 15 <= x && x <= 23 && 15 <= z && z <= 23 
                    || 10<= x && x <= 18 && 9<= z && z <= 17)
                {
                    Level = 0.5f;
                }
                if (1 <= x &&x <= 7 && 4 <= z && z <= 10 || x >= 6 && x <= 12 && z >= 15 && z <= 21 
                    || 14 <= x && x <= 20 && 1 <= z && z <= 7|| 16 <= x && x <= 22 && 16 <= z && z <= 22 
                    || 11 <= x && x <= 17 && 10 <= z && z <= 16)
                {
                    Level = 1.0f;
                }
                if (2 <= x && x <= 6 && 5 <= z && z <= 9 || x >= 7 && x <= 11 && z >= 16 && z <= 20
                    || 15 <= x && x <= 19 && 2 <= z && z <= 6 || 17 <= x && x <= 21 && 17 <= z && z <= 21
                    || 12 <= x && x <= 16 && 11 <= z && z <= 15)
                {
                    Level = 1.5f;
                }
                if (3 <= x && x <= 5 && 6 <= z && z <= 8 || x >= 8 && x <= 10 && z >= 17 && z <= 19
                    || 16 <= x && x <= 18 && 3 <= z && z <= 5 || 18 <= x && x <= 20 && 18 <= z && z <= 20
                    || 13 <= x && x <= 15 && 12 <= z && z <= 14)
                {
                    Level = 2.0f;
                }
                break;
            case StageType.Wasteland:
                break;
            case StageType.Factory1:
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
                if (x >= 15 && x <= 19 && z == 9)
                {
                    Level = 3.5f;
                }
                break;
            case StageType.Mountain1:
                if (x <= 9 && z <= 11)
                {
                    Level = 8;
                }
                if (x >= 10 && z <= 4 || z >= 18)
                {
                    Level = 10;
                }
                if (x >=12 && x <= 20 && z == 4 || x >= 12 && x <= 20 && z == 20
                    || x <= 9 && z == 17 || x == 21 && z >= 5 && z <= 19
                    || x <= 4 && z >= 8 && z <= 11)
                {
                    Level = 3;
                }
                if (x >= 1 && x <= 2 && z == 7 || x == 4 && z >= 9 && z <= 11
                    || x <= 9 && z == 17 || x >= 10 && z == 21 || x ==22 && z >= 5 && z <= 19)
                {
                    Level = 6;
                }
                if (x >= 4 && x <= 7 && z == 12 || x >= 1 && x <= 2 && z >= 8 && z <= 9)
                {
                    Level = 4;
                }
                if (x >= 10 && x <= 20 && z >= 5 && z <= 19)
                {
                    Level = 0;
                }
                if (x == 0 && z >= 8 && z <= 9 || x >= 8 && x <= 9 && z <= 7 
                    || x >= 5 && x <= 7 && z >= 9 && z <= 11 || x >= 23)
                {
                    Level = 12;
                }
                if (x >= 5 && x <= 6 && z >= 18 || x >= 20 && z >= 21)
                {
                    Level = 15;
                }
                if (x <= 16 && z == 0 || x >= 19 && z <= 1 || x >= 22 && z <= 2 || x >= 23 && z <= 6
                    || x >= 3 && x <= 7 && z >= 5 && z <= 7 || x <= 4 && z >= 18 || x <= 18 && z == 24)
                {
                    Level = 25;
                }
                if (x <= 3 && z <= 3 || x >= 1 && x <= 4 && z >= 21 || x >= 22 && z <= 1 || x >= 22 && z >= 22)
                {
                    Level = 25;
                }
                if (x >= 1 && x <= 2 && z >= 1 && z <= 2 || x >= 2 && x <= 4 && z >= 22 && z <= 23 || x >= 23 && z <= 2)
                {
                    Level = 30;
                }
                break;
            default:
                break;
        }
        return Level;
    }

    public int GetPlayerNumber(StageID ID)
    {
        int i = 0;
        switch (ID)
        {
            case StageID.Stage0:
                i = 3;
                break;
            case StageID.Stage1:
                i = 3;
                break;
            case StageID.Stage2:
                i = 5;
                break;
            case StageID.Stage3:
                i = 4;
                break;
            case StageID.Stage4:
                i = 5;
                break;
            case StageID.Stage5:
                i = 5;
                break;
            case StageID.Stage6:
                i = 3;
                break;
            case StageID.Stage7:
                i = 5;
                break;
            case StageID.Stage8:
                i = 4;
                break;
            case StageID.Stage9:
                break;
            case StageID.Stage10:
                break;
            case StageID.Stage11:
                break;
            case StageID.Stage12:
                break;
            default:
                break;
        }
        return i;
    }
    public int GetEnemyNumber(StageID ID)
    {
        int i = 0;
        switch (ID)
        {
            case StageID.Stage0:
                break;
            case StageID.Stage1:
                break;
            case StageID.Stage2:
                break;
            case StageID.Stage3:
                break;
            case StageID.Stage4:
                break;
            case StageID.Stage5:
                break;
            case StageID.Stage6:
                break;
            case StageID.Stage7:
                break;
            case StageID.Stage8:
                break;
            case StageID.Stage9:
                break;
            case StageID.Stage10:
                break;
            case StageID.Stage11:
                break;
            case StageID.Stage12:
                break;
            default:
                break;
        }
        return i;
    }

    public string GetStageName(StageID ID)
    {
        string name = "";
        switch (ID)
        {
            case StageID.Stage0:
                name = "訓練場";
                break;
            case StageID.Stage1:
                name = "平原";
                break;
            case StageID.Stage2:
                name = "都市";
                break;
            case StageID.Stage3:
                name = "森林地帯";
                break;
            case StageID.Stage4:
                name = "山岳基地";
                break;
            case StageID.Stage5:
                name = "平原";
                break;
            case StageID.Stage6:
                name = "平原";
                break;
            case StageID.Stage7:
                name = "都市";
                break;
            case StageID.Stage8:
                name = "都市";
                break;
            case StageID.Stage9:
                name = "森林地帯";
                break;
            case StageID.Stage10:
                name = "森林地帯";
                break;
            case StageID.Stage11:
                name = "山岳基地";
                break;
            case StageID.Stage12:
                name = "山岳基地";
                break;
            default:
                break;
        }
        return name;
    }

    public int GetReward(StageID ID)
    {
        int reward = 0;
        switch (ID)
        {
            case StageID.Stage0:
                reward = 500;
                break;
            case StageID.Stage1:
                reward = 1500;
                break;
            case StageID.Stage2:
                reward = 1500;
                break;
            case StageID.Stage3:
                reward = 2000;
                break;
            case StageID.Stage4:
                reward = 3000;
                break;
            case StageID.Stage5:
                reward = 2000;
                break;
            case StageID.Stage6:
                reward = 3000;
                break;
            case StageID.Stage7:
                reward = 3000;
                break;
            case StageID.Stage8:
                reward = 1000;
                break;
            case StageID.Stage9:
                break;
            case StageID.Stage10:
                break;
            case StageID.Stage11:
                break;
            case StageID.Stage12:
                break;
            default:
                break;
        }
        return reward;
    }
}
