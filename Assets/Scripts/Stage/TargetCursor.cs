using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    public Unit TargetUnit { get; private set; }
    public void SetCursor(Unit target)
    {
        TargetUnit = target;
        transform.position = new Vector3(TargetUnit.CurrentPosX * 10, 10 + TargetUnit.CurrentPosY, TargetUnit.CurrentPosZ * 10);
    }
}
