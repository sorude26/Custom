using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public void OnClickStart()
    {
        SoundManager.Instance.PlaySE(SEType.ChoiceButton);
        GameManager.Instance.StartChange(3);
    }
}
