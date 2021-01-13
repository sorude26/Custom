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
    private bool search = false;
    [SerializeField]
    GameObject attackBottons;
    [SerializeField]
    GameObject attackCommand;
    [SerializeField]
    PartsGuide guide;
    [SerializeField]
    GameObject gameOverView;
    private bool sceneChange = false;
    private bool nControl = false;
    void Start()
    {
        stageData = Stage.Instance;
       
        messageWindow.SetActive(false);
        attackCommand.SetActive(false);
        attackBottons.SetActive(false);
        gameOverView.SetActive(false);
    }

    private void Update()
    {
        if (choiceTarget && !nControl)
        {
            if (stageData.PlayerUnit.GetEnemies().Count >= 2)
            {
                if (count == 0)
                {
                    count = 1;
                    CameraControl.Instans.UnitCamera(stageData.PlayerUnit.GetTarget(count));
                    TargetCursor.instance.SetCursor(stageData.PlayerUnit.GetTarget(count));
                    guide.AttackWeapon(stageData.PlayerAttackWeapon, stageData.PlayerUnit, stageData.PlayerUnit.GetTarget(count));
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
        if (stageData.viewGameOver && !nControl)
        {
            ViewGameOver();
            nControl = true;
        }
    }
    public void ViewGameOver()
    {
        messageWindow.SetActive(false);
        attackCommand.SetActive(false);
        attackBottons.SetActive(false);
        gameOverView.SetActive(true);
    }
    public void OnClickMove()
    {
        if (stageData.turnCountTimer <= 0 && !nControl)
        {
            stageData.MoveStart();
            messageWindow.SetActive(true);
            CameraControl.Instans.UnitCameraMove(stageData.PlayerUnit);
        }
    }
    public void OnClickMoveCancel()
    {
        stageData.UnitMoveReturn();
    }
    public void OnClickAttack()
    {
        if (stageData.turnCountTimer <= 0 && !nControl)
        {
            stageData.PlayerUnit.MoveFinishSet();
            attackBottons.SetActive(true);
            guide.Clear();
            messageWindow.SetActive(false);
        }
    }

    public void OnClickDecide()
    {
        stageData.UnitMoveFinish();
        choiceTarget = false;
        search = false;
        guide.Clear();
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
        if (stageData.turnCountTimer <= 0 && !nControl)
        {
            if (stageData.PlayerUnit.RArm.CurrentPartsHp > 0)
            {
                if (!search)
                {
                    count = 0;
                    search = true;
                    choiceTarget = true;
                    attackCommand.SetActive(true);
                    stageData.PlayerUnit.SearchTarget();
                    stageData.SetPlayerAttackWeapon(stageData.PlayerUnit.RArmWeapon);
                }
                else
                {
                    stageData.SetPlayerAttackWeapon(stageData.PlayerUnit.RArmWeapon);
                    guide.AttackWeapon(stageData.PlayerUnit.RArmWeapon, stageData.PlayerUnit, stageData.PlayerUnit.GetTarget(count));
                }
            }
        }
    }
    public void OnClickLeftWeapon()
    {
        if (stageData.turnCountTimer <= 0 && !nControl)
        {
            if (stageData.PlayerUnit.LArm.CurrentPartsHp > 0)
            {
                if (!search)
                {
                    count = 0;
                    search = true;
                    choiceTarget = true;
                    attackCommand.SetActive(true);
                    stageData.PlayerUnit.SearchTarget();
                    stageData.SetPlayerAttackWeapon(stageData.PlayerUnit.LArmWeapon);
                }
                else
                {
                    stageData.SetPlayerAttackWeapon(stageData.PlayerUnit.LArmWeapon);
                    guide.AttackWeapon(stageData.PlayerUnit.LArmWeapon, stageData.PlayerUnit, stageData.PlayerUnit.GetTarget(count));
                }
            }
        }
    }

    public void OnClickAttackStart()
    {
        if (stageData.turnCountTimer <= 0 && !nControl)
        {
            CameraControl.Instans.UnitCamera(stageData.PlayerUnit);
            stageData.PlayerUnit.TargetEnemy = stageData.PlayerUnit.TargetEnemies[count];
            stageData.AttackStart();
            messageWindow.SetActive(false);
            attackCommand.SetActive(false);
            attackBottons.SetActive(false);
            choiceTarget = false;
            search = false;
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
        guide.AttackWeapon(stageData.PlayerAttackWeapon, stageData.PlayerUnit, stageData.PlayerUnit.GetTarget(count));
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
        guide.AttackWeapon(stageData.PlayerAttackWeapon, stageData.PlayerUnit, stageData.PlayerUnit.GetTarget(count));
    }
    public void OnClickRetry()
    {
        if (!sceneChange)
        {
            GameManager.Instance.SceneChange(0);
            sceneChange = true;
        }
    }

    public void OnClickReturn()
    {
        if (!sceneChange)
        {
            GameManager.Instance.SceneChange(1);
            sceneChange = true;
        }
    }
}
