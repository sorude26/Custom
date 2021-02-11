using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMessage : MonoBehaviour
{
    [SerializeField]
    List<GameObject> battleMessage;
    //０．戦闘開始、１．自軍ターン、２．敵軍ターン、３．勝利条件達成、４．敗北、５．敵の殲滅、６．攻撃目標破壊、７．規定数撃破、８．目標地点への到達
    [SerializeField]
    GameObject messageBack;
    private float clearScale = 1;
    private int viewTarget = 0;
    private float viweTime = 0;
    private bool viwe = false;
    private bool startViwe = false;
    void Start()
    {
        viwe = true;
        viweTime = 2.0f;
        foreach (GameObject message in battleMessage)
        {
            message.SetActive(false);
        }
        messageBack.SetActive(true);
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
        if (startViwe)
        {
            if (clearScale < 1)
            {
                clearScale += 2.0f * Time.deltaTime;
            }
            if (clearScale >= 1)
            {
                clearScale = 1;
                viwe = true;
                startViwe = false;
            }
            messageBack.GetComponent<Image>().color = new Color(1, 1, 1, clearScale);
            battleMessage[viewTarget].GetComponent<Image>().color = new Color(1, 1, 1, clearScale);
        }
    }
    /// <summary>
    /// 指定されたメッセージを表示する、０．戦闘開始、１．自軍ターン、２．敵軍ターン、３．勝利条件達成、４．敗北、５．敵の殲滅、６．攻撃目標破壊、７．規定数撃破、８．目標地点への到達
    /// </summary>
    /// <param name="i">対象メッセージ番号</param>
    /// <param name="time">表示時間</param>
    public void ViewMessage(int i, float time)
    {
        foreach (GameObject message in battleMessage)
        {
            message.SetActive(false);
        }
        viewTarget = i;
        messageBack.SetActive(true);
        battleMessage[i].SetActive(true);
        clearScale = 0;
        messageBack.GetComponent<Image>().color = new Color(1, 1, 1, clearScale);
        battleMessage[viewTarget].GetComponent<Image>().color = new Color(1, 1, 1, clearScale);
        viweTime = time;
        viwe = false;
        startViwe = true;
    }
}
