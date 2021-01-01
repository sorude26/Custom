using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    Stage stageData;

    [SerializeField]
    GameObject messageWindow;
    private int count = 0;
    private bool choiceTarget = false;
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

    private void Update()
    {
        if (choiceTarget)
        {
            if (stageData.PlayerUnit.GetEnemies().Count >= 2)
            {
                if (count == 0)
                {
                    count = 1;
                    CameraControl.Instans.UnitCamera(stageData.PlayerUnit.GetTarget(count));
                    TargetCursor.instance.SetCursor(stageData.PlayerUnit.GetTarget(count));
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    OnClickLeftArrow();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    OnClickRigftArrow();
                }
            }
            else
            {
                OnClickDecide();
            }
        }
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
        choiceTarget = false;
        messageWindow.SetActive(false);
        attackCommand.SetActive(false);
        attackBottons.SetActive(false);
    }

    public void OnClickEnemyMove()
    {
        stageData.enemyUnit.StatAction();
    }

    public void OnClickRightWeapon()
    {
        if (stageData.PlayerUnit.RArm.CurrentPartsHp > 0)
        {
            //stageData.PlayerUnit.TargetEnemy = null;
            stageData.PlayerUnit.SearchTarget();
            stageData.SetPlayerAttackWeapon(stageData.PlayerUnit.RArmWeapon);
            attackCommand.SetActive(true);
            choiceTarget = true;
            count = 0;
        }

    }
    public void OnClickLeftWeapon()
    {
        if (stageData.PlayerUnit.LArm.CurrentPartsHp > 0)
        {
            //stageData.PlayerUnit.TargetEnemy = null;
            stageData.PlayerUnit.SearchTarget();
            stageData.SetPlayerAttackWeapon(stageData.PlayerUnit.LArmWeapon);
            attackCommand.SetActive(true);
            choiceTarget = true;
            count = 0;
        }
    }

    public void OnClickAttackStart()
    {
        if (stageData.turnCountTimer <= 0)
        {
            CameraControl.Instans.UnitCamera(stageData.PlayerUnit);
            stageData.PlayerUnit.TargetEnemy = stageData.PlayerUnit.TargetEnemies[count];
            stageData.AttackStart();
            messageWindow.SetActive(false);
            attackCommand.SetActive(false);
            attackBottons.SetActive(false);
            choiceTarget = false;
        }
    }

    public void OnClickLeftArrow()
    {
        count--;
        if (count <= 0)
        {
            count = stageData.PlayerUnit.GetEnemies().Count - 1;
        }
        CameraControl.Instans.UnitCamera(stageData.PlayerUnit.GetTarget(count));
        TargetCursor.instance.SetCursor(stageData.PlayerUnit.GetTarget(count));
    }
    public void OnClickRigftArrow()
    {
        count++;
        if (count >= stageData.PlayerUnit.GetEnemies().Count)
        {
            count = 1;
        }
        CameraControl.Instans.UnitCamera(stageData.PlayerUnit.GetTarget(count));
        TargetCursor.instance.SetCursor(stageData.PlayerUnit.GetTarget(count));
    }
}
