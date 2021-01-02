using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceUnit : MonoBehaviour
{
    [SerializeField]
    GameObject choiceView;
    [SerializeField]
    GameObject questionView;    
    BaseStage baseStage;
    bool view = false;
    bool open = false;
    private void Start()
    {
        baseStage = BaseStage.Instance;
        choiceView.SetActive(false);
        questionView.SetActive(false);
    }
    private void Update()
    {
        if (baseStage.viewOpen && !view)
        {
            open = false;
            choiceView.SetActive(false);
            questionView.SetActive(false);
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
            questionView.SetActive(false);
        }
    }
    public void QuestionViewOpen()
    {
        questionView.SetActive(true);
    }
}
