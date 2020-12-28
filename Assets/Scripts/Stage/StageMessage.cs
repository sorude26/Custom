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
    Image messageBack;
    private float clearScale = 1;
    void Start()
    {
        foreach (GameObject message in battleMessage)
        {
            message.SetActive(false);
        }
    }

    void Update()
    {
        if (clearScale > 0)
        {
            clearScale -= 0.5f * Time.deltaTime;
        }
        if (clearScale <= 0)
        {
            clearScale = 0;
        }
        messageBack.color = new Color(1,1,1,clearScale);
    }

    public void ViewMessage(int i)
    {
        battleMessage[i].SetActive(true);
        clearScale = 1;
    }
}
