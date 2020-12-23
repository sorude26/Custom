using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public GameObject mark;

    void Update()
    {
        if (!silhouetteOn && !DestroyBody)
        {
            UnitCreate(0, 0, 0, 1, 0, 1, 0);
        }
        if (silhouetteOn)
        {
            if (moveMood)
            {
                UnitMove();
            }
            if (gameStage.MoveFinish && !moveMood)//移動終了で位置を保存
            {
                MoveFinishSet();
                gameStage.MoveFinish = false;
            }
            UnitAngleControl();
            if (DestroyBody)
            {
                gameObject.SetActive(false);
                gameStage.PlayerDestroyCount++;
                silhouetteOn = false;
            }
        }

    }

    private void LateUpdate()
    {
        PartsUpdate();
    }
    /// <summary>
    /// 移動可能箇所表示
    /// </summary>
    /// <param name="moveList"></param>
    public void UnitMoveList(List<List<Map.MapDate>> moveList)
    {
        for (int i = 0; i < gameMap.maxX; i++)
        {
            for (int j = 0; j < gameMap.maxZ; j++)
            {
                if (moveList[i][j].movePoint > 0)
                {
                    GameObject instance = Instantiate(mark);
                    instance.transform.position = new Vector3(i * gameMap.mapScale, moveList[i][j].Level, j * gameMap.mapScale);
                }
            }
        }
    }
    public void UnitMoveList2(List<Map.MapDate> moveList)
    {
        //gameStage.SetUnitPos();
        foreach (Map.MapDate map in moveList)
        {
            if (map.movePoint > 0)
            {
                GameObject instance = Instantiate(mark);
                instance.transform.position = new Vector3(map.PosX * gameMap.mapScale, map.Level, map.PosZ * gameMap.mapScale);
            }
        }
    }
}
