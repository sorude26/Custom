using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : MonoBehaviour
{
    [SerializeField]
    GameObject message;
    float messageTimer = 10f;
    void Start()
    {
        message.SetActive(false);
    }

    void Update()
    {
        if (messageTimer > 0)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0)
            {
                message.SetActive(true);
            }
        }
    }

    public void OnClickTitle()
    {
        SoundManager.Instance.PlaySE(SEType.ClickBotton);        
        message.SetActive(false);
        GameManager.Instance.StartChange(6);
    }
}
