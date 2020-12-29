using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanel : MonoBehaviour
{
    Stage stage;
    private void Start()
    {
        stage = Stage.Instance;
    }

    private void Update()
    {
        if (!stage.PlayerMoveMode)
        {
            Destroy(gameObject);
        }
    }

    public void OnClickMovePos()
    {
        if (!stage.MoveNow)
        {
            Vector3 thisPos = transform.position;
            //float posX = thisPos.x;
            //float posZ = thisPos.z;
            //Debug.Log("クリックされた" + posX + "," + posZ);
            int x = (int)Math.Round(thisPos.x) / 10;
            int z = (int)Math.Round(thisPos.z) / 10;
            stage.UnitMoveStart(x, z);
        }
    }
}
