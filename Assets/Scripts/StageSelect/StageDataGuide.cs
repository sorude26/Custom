using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDataGuide : MonoBehaviour
{
    public static StageDataGuide Instance { get; private set; } 
    [SerializeField]
    Text guideText;
    [SerializeField]
    StageData stageData;

    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WritingGuide(StageID ID)
    {
        stageData.GetData(ID);
    }
}
