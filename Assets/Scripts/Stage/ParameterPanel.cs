using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterPanel : MonoBehaviour
{
    [SerializeField] Image head = null;
    [SerializeField] Text headHp = null;
    [SerializeField] Image body = null;
    [SerializeField] Text bodyHp = null;
    [SerializeField] Image rArm = null;
    [SerializeField] Text rArmHp = null;
    [SerializeField] Image lArm = null;
    [SerializeField] Text lArmHp = null;
    [SerializeField] Image leg = null;
    [SerializeField] Text legHp = null;
    Unit unit = null;
    [SerializeField] RectTransform rect;
    bool up;
    bool down;
    bool end;
    float posY = 0;
    float startPosX;
    float startPosY;
    private void Start()
    {
        startPosX = rect.transform.position.x;
        startPosY = rect.transform.position.y;
    }
    void Update()
    {
        if (unit != null && !unit.DestroyBody)
        {
            if (unit.Body.unitType == UnitType.Human)
            {
                head.fillAmount = (float)unit.Head.CurrentPartsHp / unit.Head.MaxPartsHp;
                headHp.text = "Head :" + unit.Head.CurrentPartsHp;
                body.fillAmount = (float)unit.Body.CurrentPartsHp / unit.Body.MaxPartsHp;
                bodyHp.text = "Body :" + unit.Body.CurrentPartsHp;
                rArm.fillAmount = (float)unit.RArm.CurrentPartsHp / unit.RArm.MaxPartsHp;
                rArmHp.text = "RArm:" + unit.RArm.CurrentPartsHp;
                lArm.fillAmount = (float)unit.LArm.CurrentPartsHp / unit.LArm.MaxPartsHp;
                lArmHp.text = "LArm:" + unit.LArm.CurrentPartsHp;
                leg.fillAmount = (float)unit.Leg.CurrentPartsHp / unit.Leg.MaxPartsHp;
                legHp.text = "Leg  :" + unit.Leg.CurrentPartsHp;
            }
            else if (unit.Body.unitType == UnitType.Helicopter)
            {
                head.fillAmount = 0;
                headHp.text = "";
                body.fillAmount = (float)unit.Body.CurrentPartsHp / unit.Body.MaxPartsHp;
                bodyHp.text = "Body :" + unit.Body.CurrentPartsHp;
                rArm.fillAmount = 0;
                rArmHp.text = "";
                lArm.fillAmount = 0;
                lArmHp.text = "";
                leg.fillAmount = 0;
                legHp.text = "";
            }
            else if (unit.Body.unitType == UnitType.Tank)
            {
                head.fillAmount = (float)unit.Head.CurrentPartsHp / unit.Head.MaxPartsHp;
                headHp.text = "Head :" + unit.Head.CurrentPartsHp;
                body.fillAmount = (float)unit.Body.CurrentPartsHp / unit.Body.MaxPartsHp;
                bodyHp.text = "Body :" + unit.Body.CurrentPartsHp;
                rArm.fillAmount = 0;
                rArmHp.text = "";
                lArm.fillAmount = 0;
                lArmHp.text = "";
                leg.fillAmount = 0;
                legHp.text = "";
            }
        }
        else
        {
            head.fillAmount = 0;
            headHp.text = "";
            body.fillAmount = 0;
            bodyHp.text = "";
            rArm.fillAmount = 0;
            rArmHp.text = "";
            lArm.fillAmount = 0;
            lArmHp.text = "";
            leg.fillAmount = 0;
            legHp.text = "";
        }
        if (up)
        {
            if (posY < 200 && !end)
            {
                posY += 800 * Time.deltaTime;
                if (posY >= 200)
                {
                    posY = 200;
                }
            }
            else if (end)
            {
               // posY -= 800 * Time.deltaTime;
               // if (posY <= 0)
               // {
                    posY = 0;
                    end = false;
                    up = false;
               // }
            }
            rect.transform.position = new Vector3(startPosX, startPosY + posY, 0);
        }
        if (down)
        {
            if (posY > -200 && !end)
            {
                posY -= 800 * Time.deltaTime;
                if (posY <= -200)
                {
                    posY = -200;
                }
            }
            else if (end)
            {
                //posY += 800 * Time.deltaTime;
               // if (posY >= 0)
               // {
                    posY = 0;
                    end = false;
                    down = false;
               // }
            }
            rect.transform.position = new Vector3(startPosX, startPosY + posY, 0);
        }
        if (!up && !down && end)
        {
            end = false;
        }
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }
    public void BattleMoveUp()
    {
        up = true;
    }
    public void BattleMoveDown()
    {
        down = true;
    }
    public void BattleEnd()
    {
        end = true;
    }
}
