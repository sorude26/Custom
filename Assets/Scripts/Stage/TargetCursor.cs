using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    public static TargetCursor instance;
    public Unit TargetUnit { get; private set; }
    private float posY = 0;
    private float currentPosY = 6;
    private int y = 1;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (posY < 0)
        {
            y = 1;
        }
        else if (posY >= 0.5f)
        {
            y = -1;
        }
        posY += y * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, posY + currentPosY, transform.position.z);
        transform.LookAt(Camera.main.transform);
    }
    public void SetCursor(Unit target)
    {
        TargetUnit = target;
        currentPosY = 6 + TargetUnit.CurrentPosY;
        transform.position = new Vector3(TargetUnit.CurrentPosX * 10, currentPosY, TargetUnit.CurrentPosZ * 10);
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            Stage.StageDate.panelE.SetUnit(enemy);
        }
    }
}
