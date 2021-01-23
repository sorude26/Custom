using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public class PlayerUnitData
    {
        public int HeadID { get; set; }
        public int BodyID { get; set; } 
        public int LArmID { get; set; } 
        public int WeaponLID { get; set; }
        public int RArmID { get; set; }
        public int WeaponRID { get; set; }
        public int LegID { get; set; }
    }
    public static PlayerUnitData[] unitDatas = new PlayerUnitData[5];
    public static PlayerUnitData[] sortieUnit = new PlayerUnitData[5];
    public StageID StageCode { get; private set; }
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
                SceneManager.LoadScene("Title");
                break;
            default:
                break;
        }
    }

    public void SetStageID(StageID ID)
    {
        StageCode = ID;
    }
    public bool GetStageFlag(StageID ID)
    {
        return false;
    }
}
