﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatMove : MonoBehaviour
{
    private float posY = 0;
    [SerializeField]
    private float currentPosY = 6;
    private int y = 1;
    
    void Update()
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
    }
}