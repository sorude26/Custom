using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Instans { get; private set; }

    /// <summary>
    /// 地形タイプ
    /// </summary>
    public enum MapType
    {
        Normal,//通常
        NonAggressive,//侵入不可
        Asphalt,//舗装
        Wasteland,//荒地
    }

    /// <summary>
    /// 地形データ
    /// </summary>
    public class MapDate
    {
        public int PosX { get; private set; }//X座標
        public int PosZ { get; private set; }//Z座標
        public float Level { get; private set; }//高さ
        public MapType MapType { get; private set; }//地形種

        public int movePoint = 0;//移動計算用データ
        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="mapType"></param>
        /// <param name="posX"></param>
        /// <param name="posZ"></param>
        /// <param name="level"></param>
        public MapDate(MapType mapType, int posX, int posZ, float level)
        {
            MapType = mapType;
            PosX = posX;
            PosZ = posZ;
            Level = level;
            movePoint = 0;
        }
    }

    public List<List<MapDate>> MapDates { get; private set; } = new List<List<MapDate>>();//マップデータ

    public List<List<MapDate>> MoveList { get; private set; } //移動マップデータ
    public Stage gameStage;//ステージデータ
    public readonly int maxX = 10;//マップ最大値
    public readonly int maxZ = 10;
    public readonly int mapScale = 10;//拡大率

    private void Awake()
    {
        MapCreate(maxX, maxZ);
        MoveList = new List<List<MapDate>>(MapDates);
        Instans = this;
    }
    void Start()
    {
        gameStage = Stage.StageDate;
    }

    /// <summary>
    /// マップ生成
    /// </summary>
    /// <param name="x">X軸</param>
    /// <param name="z">Z軸</param>
    public void MapCreate(int x, int z)
    {
        for (int i = 0; i < x; i++)
        {
            List<MapDate> mapX = new List<MapDate>();
            for (int j = 0; j < z; j++)
            {
                float y = 0;
                MapType type = MapType.Normal;
               if (i > 1 && i < 8 && j > 1 && j < 8)
                {
                    y = 2.0f;
                    //type = MapType.Wasteland;
                }
                
                MapDate mapZ = new MapDate(type, i, j, y);
                mapX.Add(mapZ);
            }
            MapDates.Add(mapX);
        }
    }

    /// <summary>
    /// 地形タイプごとの移動力補正を返す
    /// </summary>
    /// <param name="mapType">地形タイプ</param>
    /// <returns></returns>
    public int MovePoint(MapType mapType)
    {
        int point;
        switch (mapType)
        {
            case MapType.Normal:
                point = 1;
                break;
            case MapType.NonAggressive:
                point = 0;
                break;
            case MapType.Asphalt:
                point = 1;
                break;
            case MapType.Wasteland:
                point = 2;
                break;
            default:
                point = 0;//０は移動不可
                break;
        }
        return point;
    }


    /// <summary>
    /// ユニットの移動範囲を書き込む
    /// </summary>
    /// <param name="moveUnit">移動ユニット</param>
    public void StartSearch(Unit moveUnit)
    {
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxZ; j++)
            {
                MoveList[i][j].movePoint = 0;
            }
        }
        MoveList[moveUnit.CurrentPosX][moveUnit.CurrentPosZ].movePoint = moveUnit.GetMovePower();
        SearchCross(moveUnit.CurrentPosX, moveUnit.CurrentPosZ, moveUnit.GetMovePower(), moveUnit.GetLiftingForce());
    }

    /// <summary>
    /// 十字範囲の移動可能箇所を調べる
    /// </summary>
    /// <param name="x">X軸現在地</param>
    /// <param name="z">Z軸現在地</param>
    /// <param name="movePower">移動力</param>
    /// <param name="liftingForce">昇降力</param>
    void SearchCross(int x, int z, int movePower, float liftingForce)
    {
        if (0 <= x && x < maxX && 0 <= z && z < maxZ)
        {
            SearchPos(x, z - 1, movePower, MoveList[x][z].Level, liftingForce);
            SearchPos(x, z + 1, movePower, MoveList[x][z].Level, liftingForce);
            SearchPos(x - 1, z, movePower, MoveList[x][z].Level, liftingForce);
            SearchPos(x + 1, z, movePower, MoveList[x][z].Level, liftingForce);
        }
    }

    /// <summary>
    /// 対象座標の確認
    /// </summary>
    /// <param name="x">対象X軸</param>
    /// <param name="z">対象Z軸</param>
    /// <param name="movePower">移動力</param>
    /// <param name="currentLevel">現在地高度</param>
    /// <param name="liftingForce">昇降力</param>
    void SearchPos(int x, int z, int movePower, float currentLevel, float liftingForce)
    {
        if (x < 0 || x >= maxX || z < 0 || z >= maxZ)//調査対象がマップ範囲内であるか確認
        {
            return;
        }
        if (MovePoint(MoveList[x][z].MapType) == 0)//侵入可能か確認、０は侵入不可又は未設定
        {
            return;
        }
        if (MoveList[x][z].Level >= currentLevel)//高低差確認
        {
            if (MoveList[x][z].Level - currentLevel > liftingForce)
            {
                return;
            }
        }
        else
        {
            if (currentLevel - MoveList[x][z].Level > liftingForce)
            {
                return;
            }
        }

        if (movePower <= MoveList[x][z].movePoint)//確認済か確認
        {
            return;
        }
        for (int i = 0; i < gameStage.stageUnits.Count; i++)//ユニットがいるか確認
        {
            if (x == gameStage.stageUnitsPos[i][0] && z == gameStage.stageUnitsPos[i][1])
            {
                if (!gameStage.stageUnits[i].DestroyBody)
                {
                    return;
                }
            }
        }
        movePower = movePower - MovePoint(MoveList[x][z].MapType);//移動力変動

        if (movePower > 0)//移動可能箇所に足跡入力、再度検索
        {
            MoveList[x][z].movePoint = movePower;
            SearchCross(x, z, movePower, liftingForce);
        }
    }
}
