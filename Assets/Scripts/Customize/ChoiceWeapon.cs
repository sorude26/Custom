﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceWeapon : MonoBehaviour
{
    [SerializeField]
    GameObject choiceView;
    [SerializeField]
    GameObject weaponL;
    [SerializeField]
    GameObject weaponR;
    BaseStage baseStage;
    bool view = false;
    bool open = false;
    private void Start()
    {
        baseStage = BaseStage.Instance;
        choiceView.SetActive(false);
        weaponL.SetActive(false);
        weaponR.SetActive(false);
    }
    private void Update()
    {
        if (baseStage.viewOpen && !view)
        {
            open = false;
            choiceView.SetActive(false);
            weaponL.SetActive(false);
            weaponR.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        if (view)
        {
            view = false;
            baseStage.viewOpen = false;
            baseStage.SwithGard = true;
        }
    }
    public void OnClickView()
    {
        if (!open)
        {
            choiceView.SetActive(true);
            open = true;
            view = true;
            baseStage.viewOpen = true;
        }
        else
        {
            open = false;
            choiceView.SetActive(false);
            weaponL.SetActive(false);
            weaponR.SetActive(false);
        }
    }
    public void OnClickWeaponL()
    {
        weaponL.SetActive(true);
        weaponR.SetActive(false);
        baseStage.SwithGard = true;
        baseStage.GuideWeaponL();
    }
    public void OnClickWeaponR()
    {
        weaponL.SetActive(false);
        weaponR.SetActive(true);
        baseStage.SwithGard = true;
        baseStage.GuideWeaponR();
    }
}
