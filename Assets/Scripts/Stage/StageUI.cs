using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    Stage stageData;

    [SerializeField]
    GameObject messageWindow;
    private Weapon weapon;
    [SerializeField]
    GameObject attackBottons;
    [SerializeField]
    GameObject attackCommand;
    void Start()
    {
        stageData = Stage.Instance;
        messageWindow.SetActive(false);
        attackCommand.SetActive(false);
        attackBottons.SetActive(false);
    }


    public void OnClickMove()
    {
        if (stageData.turnCountTimer <= 0)
        {
            stageData.MoveStart();
            messageWindow.SetActive(true);
        }
    }
    public void OnClickMoveCancel()
    {
        stageData.UnitMoveReturn();
    }
    public void OnClickAttack()
    {
        if (stageData.turnCountTimer <= 0)
        {
            stageData.PlayerUnit.MoveFinishSet();
            attackBottons.SetActive(true);
            messageWindow.SetActive(false);
        }
    }

    public void OnClickDecide()
    {
        stageData.UnitMoveFinish();
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
            stageData.PlayerUnit.TargetEnemy = null;
            stageData.PlayerUnit.SearchTarget();
            stageData.SetPlayerAttackWeapon(stageData.PlayerUnit.RArmWeapon);
            attackCommand.SetActive(true);
        }

    }
    public void OnClickLeftWeapon()
    {
        if (stageData.PlayerUnit.LArm.CurrentPartsHp > 0)
        {
            stageData.PlayerUnit.TargetEnemy = null;
            stageData.PlayerUnit.SearchTarget();
            stageData.SetPlayerAttackWeapon(stageData.PlayerUnit.LArmWeapon);
            attackCommand.SetActive(true);
        }
    }

    public void OnClickAttackStart()
    {
        CameraControl.Instans.UnitCamera(stageData.PlayerUnit);
        //stageData.PlayerUnit.SearchTarget();
        stageData.PlayerUnit.TargetEnemy = stageData.PlayerUnit.TargetEnemies[0];
        stageData.AttackStart();
        messageWindow.SetActive(false);
        attackCommand.SetActive(false);
        attackBottons.SetActive(false);
    }
}
