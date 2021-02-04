using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public void OnClickStart()
    {
        GameManager.Instance.StartChange(3);
    }
}
