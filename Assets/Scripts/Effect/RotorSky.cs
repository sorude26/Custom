using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotorSky : MonoBehaviour
{
    [SerializeField]
    float anglePerFrame = 0.1f;    // 1フレームに何度回すか[unit : deg]
    float rot = 0.0f;
    float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.1f)
        {
            rot += anglePerFrame;
            if (rot >= 360.0f)
            {    // 0～360°の範囲におさめたい
                rot -= 360.0f;
            }
            RenderSettings.skybox.SetFloat("_Rotation", rot);    // 回す
            timer = 0;
        }
    }
}
