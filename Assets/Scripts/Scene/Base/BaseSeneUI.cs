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
    private bool moneyMax;
    private void Start()
    {
        allMoney.text = GameManager.allMoney + "";
        massegeBox.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.allMoney >= 30000 && !moneyMax)
        {
            if (SceneChangeControl.Instance.GetFadeInEnd())
            {
                GameManager.Instance.StartChange(7);
                moneyMax = true;
            }
        }
    }
    public void OnClickStageSelect()
    {
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
        massegeBox.SetActive(true);
        sceneNumber = 2;
    }
    public void OnClickCustomize()
    {
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
        massegeBox.SetActive(true);
        sceneNumber = 1;
    }
    public void OnClickSceneChange()
    {
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
        GameManager.Instance.StartChange(sceneNumber);
    }
    public void OnClickCancel()
    {
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
        massegeBox.SetActive(false);
    }
}
