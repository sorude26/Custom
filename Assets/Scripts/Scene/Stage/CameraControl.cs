﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera MainCamera { get; private set; }
    public Transform MainCameraTrans { get; private set; }

    private int cameraPos = 0;
    public static CameraControl Instance { get; private set; }
    private Unit target = null;
    private int posX = -15;
    private int posZ = -15;
    private void Awake()
    {
        Instance = this;
        MainCamera = GetComponentInChildren<Camera>();
        MainCameraTrans = MainCamera.transform;
    }

    private void Start()
    {
        // transform.position = new Vector3(-10, 20, -30);
        transform.position = new Vector3(-10, 90, -30);
        Vector3 lockOnPos = new Vector3(Map.Instance.maxX * 5.0f, 0, Map.Instance.maxZ * 5.0f);
        Vector3 targetDir = lockOnPos - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDir);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (target != null)
            {
                CameraPositionChange2(cameraPos);
            }
            else
            {
                CameraPositionChange(cameraPos);
            }
            cameraPos++;
        }
    }
    public void CameraPositionChange(int Pos)
    {
        switch (Pos)
        {
            case 0:
                transform.position = new Vector3(-10, 20, -30);
                Vector3 lockOnPos = new Vector3(Map.Instance.maxX * 5.0f, 0, Map.Instance.maxZ * 5.0f);
                Vector3 targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                break;
            case 1:
                transform.position = new Vector3(Map.Instance.maxX * 10.0f, 20, Map.Instance.maxZ * 10.0f + 15);
                lockOnPos = new Vector3(Map.Instance.maxX * 5.0f, 0, Map.Instance.maxZ * 5.0f);
                targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                break;
            case 2:
                transform.position = new Vector3(-10, 20, Map.Instance.maxZ * 10.0f + 15);
                lockOnPos = new Vector3(Map.Instance.maxX * 5.0f, 0, Map.Instance.maxZ * 5.0f);
                targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                break;
            case 3:
                transform.position = new Vector3(Map.Instance.maxX * 10.0f, 22, -30);
                lockOnPos = new Vector3(Map.Instance.maxX * 5.0f, 0, Map.Instance.maxZ * 5.0f);
                targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                break;
            case 4:
                transform.position = new Vector3(Map.Instance.maxX * 5.0f, (Map.Instance.maxX + Map.Instance.maxZ)*5, Map.Instance.maxZ * 5.0f);
                lockOnPos = new Vector3(Map.Instance.maxX * 5.0f, 0, Map.Instance.maxZ * 5.0f);
                targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                break;
            default:
                transform.position = new Vector3(-10, 20, -30);
                lockOnPos = new Vector3(Map.Instance.maxX * 5.0f, 0, Map.Instance.maxZ * 5.0f);
                targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                cameraPos = 0;
                break;
        }
    }
    public void CameraPositionChange2(int Pos)
    {
        switch (Pos)
        {
            case 0:
                posX = 15; posZ = -15;
                UnitCamera(target);
                break;
            case 1:
                posX = 15; posZ = 15;
                UnitCamera(target);
                break;
            case 2:
                posX = -15; posZ = 15;
                UnitCamera(target);
                break;
            default:
                posX = -15; posZ = -15;
                UnitCamera(target);
                cameraPos = -1;
                break;
        }
    }
    public void UnitCamera(Unit unit)
    {
        target = unit;
        transform.position = new Vector3(unit.CurrentPosX * 10 + posX, 10 + unit.CurrentPosY, unit.CurrentPosZ * 10 + posZ);
        Vector3 targetDir = unit.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDir);
    }

    public void UnitCameraMove(Unit unit)
    {
        transform.position = new Vector3(unit.CurrentPosX * 10 + posX - unit.GetMovePower() * 0.5f, unit.GetMovePower()*2 + unit.CurrentPosY, unit.CurrentPosZ * 10 + posZ -unit.GetMovePower() * 0.5f);
        Vector3 targetDir = unit.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDir);
    }

    public void AttackCamera(Unit unit)
    {
        transform.position = unit.Body.GetCameraPos().position; 
        Vector3 targetDir = unit.Body.GetBodyCentrer().position - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDir);
    }
    public void PointCamera(int x, int z)
    {
        int p = x + (z * Map.Instance.maxX);
        transform.position = new Vector3(x * 10 + posX, 10 + Map.Instance.MapDates2[p].Level, z * 10 + posZ);
        Vector3 targetDir = new Vector3(x * 10, Map.Instance.MapDates2[p].Level, z * 10) - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDir);
    }
}
