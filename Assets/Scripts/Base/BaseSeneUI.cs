using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSeneUI : MonoBehaviour
{
    [SerializeField]
    GameObject massegeBox;
    private int sceneNumber;
    private void Start()
    {
        massegeBox.SetActive(false);
    }
    public void OnClickStageSelect()
    {
        massegeBox.SetActive(true);
        sceneNumber = 2;
    }
    public void OnClickCustomize()
    {
        massegeBox.SetActive(true);
        sceneNumber = 1;
    }
    public void OnClickSceneChange()
    {
        GameManager.Instance.SceneChange(sceneNumber);
    }
    public void OnClickCancel()
    {
        massegeBox.SetActive(false);
    }
}
