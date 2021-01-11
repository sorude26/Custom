using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 5, 0));
    }
}
