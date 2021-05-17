using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
    int cursorPosX;
    int cursorPosZ;
    bool cursorControl;
    void Start()
    {
        
    }

    void Update()
    {
        if (!cursorControl)
        {
            return;
        }
        if (Input.GetButtonDown("Horizontal"))
        {
            float h = Input.GetAxisRaw("Horizontal");
            if (h > 0)
            {
                cursorPosX++;
            }
            else if(h < 0)
            {
                cursorPosX--;
            }
        }
        if (Input.GetButtonDown("Vertical"))
        {
            float v = Input.GetAxisRaw("Vertical");
            if (v > 0)
            {
                cursorPosZ++;
            }
            else if(v < 0)
            {
                cursorPosZ--;
            }
        }
    }
}
