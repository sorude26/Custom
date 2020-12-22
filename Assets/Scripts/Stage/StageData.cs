using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    City1,
    Forest1,
    Wasteland,
    Factory1;
}
public class StageData : MonoBehaviour
{
    public StageType Type { get; set; }
    public float Level { get;private set; }
    public Map.MapType MapTypeData { get; private set; }

    void StageDataGet(int x, int z)
    {
        if(Type == StageType.Factory1)
        {
            MapTypeData = Map.MapType.Asphalt;
            if (x <= 10 && x >= 14 && z >= 0 && z <= 9)
            {
                Level = 1;
            }
        }
        
    }
}
