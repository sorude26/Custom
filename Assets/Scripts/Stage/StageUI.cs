using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    Stage stageData;
    
    [SerializeField]
    GameObject messageWindow;
    private bool message;
    [SerializeField]
    GameObject attackBottons;
    [SerializeField]
    List<GameObject> attackCommands;
    void Start()
    {
        stageData = Stage.Instance;
        messageWindow.SetActive(false);
    }

    void Update()
    {
        if (!stageData.MoveNow && stageData.PlayerMoveMode && !message)
        {
            message = true;
            messageWindow.SetActive(true);
        }
    }

    public void OnClickMove()
    {
        stageData.MoveStart();
    }

    public void OnClickAttack()
    {

        stageData.AttackStart();
        message = false;
        messageWindow.SetActive(false);

    }

    public void OnClickDecide()
    {        
        stageData.UnitMoveFinish();
        message = false;
        messageWindow.SetActive(false);
    }

    public void OnClickEnemyMove()
    {
        stageData.enemyUnit.StatAction();
    }

    public void OnClickRightWeapon()
    {
        if (stageData.PlayerUnit.RArm.CurrentPartsHp > 0)
        {
            stageData.SetPlayerAttackWeapon(stageData.PlayerUnit.RArmWeapon); 
        }
    }
    public void OnClickLeftWeapon()
    {
        if (stageData.PlayerUnit.LArm.CurrentPartsHp > 0)
        {
            stageData.SetPlayerAttackWeapon(stageData.PlayerUnit.LArmWeapon);
        }
    }
}
