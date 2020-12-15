using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera MainCamera { get; private set; }
    public Transform MainCameraTrans { get; private set; }

    private int cameraPos = 0;

    private void Awake()
    {
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
            cameraPos++;
            CameraPositionChange(cameraPos);
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
                transform.position = new Vector3(Map.Instans.maxX * 5.0f, Map.Instans.maxX * Map.Instans.maxZ, Map.Instans.maxZ * 5.0f);
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
}
