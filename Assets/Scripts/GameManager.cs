using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public struct PlayerUnitData
    {
        public int HeadID;
        public int BodyID;
        public int LArmID;
        public int WeaponLID;
        public int RArmID;
        public int WeaponRID;
        public int LegID;
        public bool Sortie;
    }
    public static PlayerUnitData[] UnitDatas { get; set; } = new PlayerUnitData[5];
    public static PlayerUnitData[] SortieUnit { get; private set; } = new PlayerUnitData[5];
    public static StageID StageCode { get; private set; }
    public static int allMoney = 0;
    public bool[] StageFlag { get; private set; }
    public struct ScoreData
    {
        public string StageName;
        public int StageReward;
        public int EnemyNumber;
        public int EnemyReward;
        public int TotalLoss;
    }
    public static ScoreData StageScoreData = new ScoreData();
    private void Awake()
    {
        Instance = this;
       
    }
    public void StartChange(int i)
    {
        SceneChangeControl.Instance.StartFade(i);
    }
    

    public void SetStageID(StageID ID)
    {
        StageCode = ID;
        StartChange(5);
    }
    public bool GetStageFlag(StageID ID)
    {
        return false;
    }
    /// <summary>
    /// 出撃機体設定
    /// </summary>
    /// <param name="sortieNumber"></param>
    /// <param name="setUnitNumber"></param>
    public void SetSortieUnit(int[] sortieData)
    {
        for (int i = 0; i < UnitDatas.Length; i++)
        {
            if (sortieData[i] < 0)
            {
                SortieUnit[i].Sortie = false;
            }
            else
            {                
                SortieUnit[i] = UnitDatas[sortieData[i]];
                SortieUnit[i].Sortie = true;
            }
        }
        StartChange(0);
    }
    public void ResetSortieUnit()
    {
        for (int i = 0; i < SortieUnit.Length; i++)
        {
            SortieUnit[i].Sortie = false;
        }
    }

    public void FullReset()
    {
        UnitDatas.ToList().ForEach(u =>
        {
            u.BodyID = 0;
            u.HeadID = 0;
            u.LArmID = 0;
            u.RArmID = 0;
            u.LegID = 0;
            u.WeaponLID = 0;
            u.WeaponRID = 0;
        });
        allMoney = 0;
    }
}
