using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera MainCamera { get; private set; }
    public Transform MainCameraTrans { get; private set; }

    private int cameraPos = 0;
    public static CameraControl Instans { get; private set; }
    private Unit target = null;
    private int posX = -15;
    private int posZ = -15;
    private void Awake()
    {
        Instans = this;
        MainCamera = GetComponentInChildren<Camera>();
        MainCameraTrans = MainCamera.transform;
    }

    private void Start()
    {
        transform.position = new Vector3(-10, 20, -30);
        Vector3 lockOnPos = new Vector3(Map.Instans.maxX * 5.0f, 0, Map.Instans.maxZ * 5.0f);
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
                Vector3 lockOnPos = new Vector3(Map.Instans.maxX * 5.0f, 0, Map.Instans.maxZ * 5.0f);
                Vector3 targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                break;
            case 1:
                transform.position = new Vector3(Map.Instans.maxX * 10.0f, 20, Map.Instans.maxZ * 10.0f + 15);
                lockOnPos = new Vector3(Map.Instans.maxX * 5.0f, 0, Map.Instans.maxZ * 5.0f);
                targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                break;
            case 2:
                transform.position = new Vector3(-10, 20, Map.Instans.maxZ * 10.0f + 15);
                lockOnPos = new Vector3(Map.Instans.maxX * 5.0f, 0, Map.Instans.maxZ * 5.0f);
                targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                break;
            case 3:
                transform.position = new Vector3(Map.Instans.maxX * 10.0f, 22, -30);
                lockOnPos = new Vector3(Map.Instans.maxX * 5.0f, 0, Map.Instans.maxZ * 5.0f);
                targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                break;
            case 4:
                transform.position = new Vector3(Map.Instans.maxX * 5.0f, (Map.Instans.maxX + Map.Instans.maxZ)*5, Map.Instans.maxZ * 5.0f);
                lockOnPos = new Vector3(Map.Instans.maxX * 5.0f, 0, Map.Instans.maxZ * 5.0f);
                targetDir = lockOnPos - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDir);
                break;
            default:
                transform.position = new Vector3(-10, 20, -30);
                lockOnPos = new Vector3(Map.Instans.maxX * 5.0f, 0, Map.Instans.maxZ * 5.0f);
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
        transform.position = new Vector3(unit.CurrentPosX * 10 + posX, 30 + unit.CurrentPosY, unit.CurrentPosZ * 10 + posZ);
        Vector3 targetDir = unit.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDir);
    }
}
