using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBuild : MonoBehaviour
{
    [SerializeField]
    GameObject choiceView;
    BaseStage baseStage;
    bool view = false;
    bool open = false;
    private void Start()
    {
        baseStage = BaseStage.Instance;
        choiceView.SetActive(false);
    }
    private void Update()
    {
        if (baseStage.viewOpen && !view)
        {
            open = false;
            choiceView.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        if (view)
        {
            view = false;
            baseStage.viewOpen = false;
        }
    }
    public void OnClickView()
    {
        if (!open)
        {
            choiceView.SetActive(true);
            open = true;
            view = true;
            baseStage.viewOpen = true;
        }
        else
        {
            open = false;
            choiceView.SetActive(false);
        }
    }

    public void CustomizeGarageOut()
    {
        baseStage.FinishBuild();
    }
}
