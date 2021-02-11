using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 5.0f;
    float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.02f)
        {
            transform.Rotate(new Vector3(0, rotateSpeed, 0));
            timer = 0;
        }
    }
}
