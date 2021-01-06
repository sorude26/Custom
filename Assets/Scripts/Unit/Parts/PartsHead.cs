using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsHead : UnitParts
{
    [SerializeField]
    float detectionRange;
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
        }
    }
    public float GetDetectionRange() { return detectionRange; }
}
