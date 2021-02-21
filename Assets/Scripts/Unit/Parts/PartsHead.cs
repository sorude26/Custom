using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsHead : UnitParts
{
    [SerializeField]
    float detectionRange;
    [SerializeField]
    Transform cameraPos;
    [SerializeField]
    GameObject headObject;
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

    public Transform GetCameraPos() { return cameraPos; }
    
}
