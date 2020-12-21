using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StageType
{
    City1,
    Forest1,
    Wasteland,
}
public class StageData : MonoBehaviour
{
    public float Level { get;private set; }
    public Map.MapType MapTypeData { get; private set; }

    void StageDataGet()
    {

    }
}
