using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingUI : MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    private float blinkingSpeed = 1.0f;
    [SerializeField]
    private float maxClearScale = 0.9f;
    [SerializeField]
    private float minClearScale = 0.1f;
    private int p = 1;
    private float clearScale = 0;
    private float timer = 0;
    void Update()
    {
        if (clearScale <= minClearScale)
        {
            p = 1;
        }
        else if (clearScale >= maxClearScale)
        {
            p = -1;
        }
        clearScale += p * blinkingSpeed * Time.deltaTime;        
        timer += Time.deltaTime;
        if (timer >= 0.05f)
        {
            if (clearScale < 0)
            {
                clearScale = 0;
            }
            else if (clearScale > 1)
            {
                clearScale = 1;
            }
            image.color = new Color(1, 1, 1, clearScale);
            timer = 0;
        }
    }
}
