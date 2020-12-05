using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    Stage stageDate;
    private bool moveOrder;
    private bool attackOrder;

    [SerializeField]
    GameObject messageWindow;
    private bool message;
    void Start()
    {
        stageDate = Stage.StageDate;
        messageWindow.SetActive(false);
    }

    void Update()
    {
        if (!stageDate.MoveNow && stageDate.PlayerMoveMode && !message)
        {
            message = true;
            messageWindow.SetActive(true);
        }
    }

    public void OnClickMove()
    {
        stageDate.MoveStart();
    }

    public void OnClickAttack()
    {

        stageDate.AttackStart();
        message = false;
        messageWindow.SetActive(false);

    }

    public void OnClickDecide()
    {
        moveOrder = false;
        stageDate.UnitMoveFinish();
        message = false;
        messageWindow.SetActive(false);
    }
}
