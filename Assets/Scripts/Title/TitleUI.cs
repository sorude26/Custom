using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public void OnClickStart()
    {
        GameManager.Instance.SceneChange(3);
    }
}
