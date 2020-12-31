﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStage : MonoBehaviour
{
    UnitPartsList partsList;
    public PartsHead Head { get; protected set; } = null;
    public PartsBody Body { get; protected set; } = null;
    public PartsLArm LArm { get; protected set; } = null;
    public PartsRArm RArm { get; protected set; } = null;
    public PartsLeg Leg { get; protected set; } = null;
    public Weapon LArmWeapon { get; protected set; } = null;
    public Weapon RArmWeapon { get; protected set; } = null;
    bool silhouetteOn = false;
    public int SetUpUnit { get; set; } = 0;
    GameManager gM;
    int partsNumber = 0;
    private void Start()
    {
        partsList = UnitPartsList.Instance;
        gM = GameManager.Instance;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (partsList.GetPartsHeadCount() > partsNumber)
            {
                GameManager.HeadID[0] = partsNumber;
            }
            if (partsList.GetPartsBodyCount() > partsNumber)
            {
                GameManager.BodyID[0] = partsNumber;
            }
            if (partsList.GetPartsLArmCount() > partsNumber)
            {
                GameManager.LArmID[0] = partsNumber;
            }
            if (partsList.GetWeaponCount() > partsNumber)
            {
                GameManager.WeaponLID[0] = partsNumber;
            }
            if (partsList.GetPartsRArmCount() > partsNumber)
            {
                GameManager.RArmID[0] = partsNumber;
            }
            if (partsList.GetWeaponCount() > partsNumber)
            {
                GameManager.WeaponRID[0] = partsNumber;
            }
            if (partsList.GetPartsLegCount() > partsNumber)
            {
                GameManager.LegID[0] = partsNumber;
            }
            ResetBuild();
            partsNumber++;
        }
       // transform.Rotate(new Vector3(0, 0.1f, 0));
    }
    private void LateUpdate()
    {
        if (!silhouetteOn)
        {
            if (!Head)
            {
                BuildUnit(GameManager.HeadID[SetUpUnit], GameManager.BodyID[SetUpUnit], GameManager.LArmID[SetUpUnit],
                    GameManager.WeaponLID[SetUpUnit], GameManager.RArmID[SetUpUnit], GameManager.WeaponRID[SetUpUnit], GameManager.LegID[SetUpUnit]);
            }
        }
        if (!Head)
        {
            silhouetteOn = false;
        }
    }
    public void BuildUnit(int headID, int bodyID, int lArmID, int weaponLID, int rArmID, int weaponRID, int legID)
    {
        if (!silhouetteOn)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);//パーツ生成時に向きを合わせる
            GameObject leg = Instantiate(partsList.GetLegObject(legID));
            leg.transform.position = transform.position;
            leg.transform.parent = transform;
            Leg = leg.GetComponent<PartsLeg>();
            GameObject body = Instantiate(partsList.GetBodyObject(bodyID));
            body.transform.parent = Leg.transform;
            Body = body.GetComponent<PartsBody>();
            Body.TransFormParts(Leg.GetPartsHigh().position);
            GameObject head = Instantiate(partsList.GetHeadObject(headID));
            head.transform.parent = Body.transform;
            Head = head.GetComponent<PartsHead>();
            Head.TransFormParts(Body.GetHeadPos().position);
            GameObject lArm = Instantiate(partsList.GetLArmObject(lArmID));
            lArm.transform.parent = Body.transform;
            LArm = lArm.GetComponent<PartsLArm>();
            LArm.TransFormParts(Body.GetLArmPos().position);
            GameObject weaponL = Instantiate(partsList.GetWeaponObject(weaponLID));
            weaponL.transform.parent = LArm.ArmParts().transform;
            LArmWeapon = weaponL.GetComponent<Weapon>();
            LArmWeapon.TransFormParts(LArm.GetGrip().position);
            GameObject rArm = Instantiate(partsList.GetRArmObject(rArmID));
            rArm.transform.parent = Body.transform;
            RArm = rArm.GetComponent<PartsRArm>();
            RArm.TransFormParts(Body.GetRArmPos().position);
            GameObject weaponR = Instantiate(partsList.GetWeaponObject(weaponRID));
            weaponR.transform.parent = RArm.ArmParts().transform;
            RArmWeapon = weaponR.GetComponent<Weapon>();
            RArmWeapon.TransFormParts(RArm.GetGrip().position);
            silhouetteOn = true;
        }
    }

    public void ResetBuild()
    {
        if (RArmWeapon)
        {
            RArmWeapon.PartsDelete();
        }
        RArmWeapon = null;
        if (LArmWeapon)
        {
            LArmWeapon.PartsDelete();
        }
        LArmWeapon = null;
        if (RArm)
        {
            RArm.PartsDelete();
        }
        RArm = null;
        if (LArm)
        {
            LArm.PartsDelete();
        }
        LArm = null;
        if (Head)
        {
            Head.PartsDelete();
        }
        Head = null;
        if (Body)
        {
            Body.PartsDelete();
        }
        Body = null;
        if (Leg)
        {
            Leg.PartsDelete();
        }
        Leg = null;
        //  silhouetteOn = false;
        foreach (Transform n in transform)
        {
            Destroy(n.gameObject);
        }
    }
}
