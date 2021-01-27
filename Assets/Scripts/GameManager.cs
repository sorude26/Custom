using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public struct PlayerUnitData
    {
        public int HeadID { get; set; }
        public int BodyID { get; set; }
        public int LArmID { get; set; }
        public int WeaponLID { get; set; }
        public int RArmID { get; set; }
        public int WeaponRID { get; set; }
        public int LegID { get; set; }
        public bool Sortie { get; set; }
    }
    public static PlayerUnitData[] UnitDatas { get; set; } = new PlayerUnitData[5];
    public static PlayerUnitData[] SortieUnit { get; private set; } = new PlayerUnitData[5];
    public static StageID StageCode { get; private set; }
    public bool[] StageFlag { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public void SceneChange(int i)
    {
        switch (i)
        {
            case 0:
                SceneManager.LoadScene("SampleScene");
                break;
            case 1:
                SceneManager.LoadScene("CustomizeScene");
                break;
            case 2:
                SceneManager.LoadScene("StageSelect");
                break;
            case 3:
                SceneManager.LoadScene("BaseScene");
                break;
            case 4:
                SceneManager.LoadScene("BattleResult");
                break;
            case 5:
                SceneManager.LoadScene("SortieScene");
                break;
            case 6:
                SceneManager.LoadScene("Title");
                break;
            default:
                break;
        }
    }

    public void SetStageID(StageID ID)
    {
        StageCode = ID;
        SceneChange(5);
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
                SortieUnit[i].Sortie = true;
                SortieUnit[i] = UnitDatas[sortieData[i]];
            }
        }
        SceneChange(0);
    }
}
