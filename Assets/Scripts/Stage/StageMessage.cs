using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMessage : MonoBehaviour
{
    [SerializeField]
    List<GameObject> battleMessage;
    //１．戦闘開始、２．自軍ターン、３．敵軍ターン、４．戦闘終了、５．勝利条件達成、６．敗北
    [SerializeField]
    GameObject messageBack;
    private float clearScale = 1;
    private int viewTarget = 0;
    private float viweTime = 0;
    private bool viwe = false;
    void Start()
    {
        viwe = true;
        viweTime = 0.1f;
        foreach (GameObject message in battleMessage)
        {
            message.SetActive(false);
        }
        battleMessage[0].SetActive(true);
    }

    void Update()
    {
        if (viwe)
        {
            if (viweTime <= 0)
            {
                if (clearScale > 0)
                {
                    clearScale -= 1.0f * Time.deltaTime;
                }
                if (clearScale <= 0)
                {
                    clearScale = 0;
                    battleMessage[viewTarget].SetActive(false);
                    messageBack.SetActive(false);
                    viwe = false;
                    return;
                }
                messageBack.GetComponent<Image>().color = new Color(1, 1, 1, clearScale);
                battleMessage[viewTarget].GetComponent<Image>().color = new Color(1, 1, 1, clearScale);
            }
            else
            {
                viweTime -= Time.deltaTime;
            }
        }
        
    }

    public void ViewMessage(int i)
    {
        viewTarget = i;
        messageBack.SetActive(true);
        battleMessage[i].SetActive(true);
        clearScale = 1;
        viweTime = 1;
        viwe = true;
    }
}
