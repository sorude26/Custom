using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public GameObject mark;

    void Update()
    {
        if (moveMood)
        {
            UnitMove();
        }
        if (gameStage.MoveFinish && !moveMood)//移動終了で位置を保存
        {
            Vector3 thisPos = transform.position;
            //Debug.Log(thisPos.x + "," + thisPos.z);
            if (CurrentPosX != (int)Math.Round(thisPos.x) / gameMap.mapScale || CurrentPosZ != (int)Math.Round(thisPos.z) / gameMap.mapScale)
            {
                CurrentPosX = (int)Math.Round(thisPos.x) / gameMap.mapScale;
                CurrentPosZ = (int)Math.Round(thisPos.z) / gameMap.mapScale;
                CurrentPosY = gameMap.MapDates[CurrentPosX][CurrentPosZ].Level;
                gameStage.MoveFinish = false;
            }
            //Debug.Log(CurrentPosX +"," + CurrentPosZ);
            gameStage.SetUnitPos();
        }
        UnitAngleControl();
        
    }

    private void LateUpdate()
    {
        if (silhouetteOn)
        {
            if (CurrentHp != Body.CurrentPartsHp + Head.CurrentPartsHp + LArm.CurrentPartsHp + RArm.CurrentPartsHp + Leg.CurrentPartsHp)
            {
                CurrentHp = Body.CurrentPartsHp + Head.CurrentPartsHp + LArm.CurrentPartsHp + RArm.CurrentPartsHp + Leg.CurrentPartsHp;
            }
            if (Body.CurrentPartsHp <= 0)
            {
                Dead();
            }
        }
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
}
