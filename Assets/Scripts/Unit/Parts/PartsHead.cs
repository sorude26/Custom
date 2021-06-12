using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsHead : UnitParts
{
    [SerializeField] float detectionRange;
    /// <summary> 回避力 </summary>
    [SerializeField] int avoidance;
    /// <summary> 命中精度 </summary>
    [SerializeField] int hitAccuracy;
    [SerializeField] Transform cameraPos;
    [SerializeField] GameObject headObject;
    public float DetectionRange { get; protected set; }//索敵範囲

    void Start()
    {
        StartSet();
        DetectionRange = detectionRange;
    }
    private void Update()
    {
        if (partsBreak)
        {
            DetectionRange = 20;
            partsBreak = false;
            if (Owner.Body.unitType != UnitType.Tank)
            {
                headObject.SetActive(false);
            }
        }
    }
    public float GetDetectionRange() { return detectionRange; }
    public int GetAvoidance() { return avoidance; }
    public int GetHitAccyracy() { return hitAccuracy; }
    public Transform GetCameraPos() { return cameraPos; }
    
}
