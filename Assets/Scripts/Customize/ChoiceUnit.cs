using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceUnit : MonoBehaviour
{
    [SerializeField]
    GameObject ChoiceView;
    [SerializeField]
    BaseStage baseStage;
    bool view = false;
    private void Start()
    {
        ChoiceView.SetActive(false);
    }
    public void OnClickView()
    {
        if (!view)
        {
            ChoiceView.SetActive(true);
            view = true;
        }
    }
}
