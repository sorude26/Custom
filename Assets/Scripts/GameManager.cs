using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static int[] HeadID { get; set; } = new int[5];
    public static int[] BodyID { get; set; } = new int[5];
    public static int[] LArmID { get; set; } = new int[5];
    public static int[] WeaponLID { get; set; } = new int[5];
    public static int[] RArmID { get; set; } = new int[5];
    public static int[] WeaponRID { get; set; } = new int[5];
    public static int[] LegID { get; set; } = new int[5];

    int count = 0;
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
            default:
                break;
        }
    }
}
