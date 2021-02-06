using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChangeControl : MonoBehaviour
{
    [SerializeField]
    FadeScene fade = null;
    private int sceneNumber;
    private bool secenChange = false;
    public static SceneChangeControl Instance { get; private set; }
    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        if (secenChange)
        {
            if (fade.IsFadeOutEnd())
            {
                SceneChange(sceneNumber);
                secenChange = false;
            }
        }
    }
    public void SceneChange(int i)
    {
        switch (i)
        {
            case 0:
                switch (GameManager.StageCode)
                {
                    case StageID.Stage0:
                        SceneManager.LoadScene("SampleScene");
                        break;
                    case StageID.Stage1:
                        SceneManager.LoadScene("Stage1");
                        break;
                    case StageID.Stage2:
                        SceneManager.LoadScene("Stage2");
                        break;
                    case StageID.Stage3:
                        break;
                    default:
                        SceneManager.LoadScene("SampleScene");
                        break;
                }               
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
            case 7:
                SceneManager.LoadScene("Ending");
                break;
            default:
                break;
        }
    }
    public void StartFade(int i)
    {
        sceneNumber = i;
        fade.StartFadeOut();
        secenChange = true;
    }
    public bool GetFadeInEnd()
    {
        return fade.IsFadeInEnd();
    }
}
