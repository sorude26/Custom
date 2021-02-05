using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSeneUI : MonoBehaviour
{
    [SerializeField]
    GameObject massegeBox;
    [SerializeField]
    Text allMoney;
    private int sceneNumber;
    private void Start()
    {
        allMoney.text = GameManager.allMoney + "";
        massegeBox.SetActive(false);
        if (GameManager.allMoney >= 10000)
        {
            GameManager.Instance.StartChange(0);
        }
    }
    public void OnClickStageSelect()
    {
        massegeBox.SetActive(true);
        sceneNumber = 2;
    }
    public void OnClickCustomize()
    {
        massegeBox.SetActive(true);
        sceneNumber = 1;
    }
    public void OnClickSceneChange()
    {
        GameManager.Instance.StartChange(sceneNumber);
    }
    public void OnClickCancel()
    {
        massegeBox.SetActive(false);
    }
}
