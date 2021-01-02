using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandType
{
    ChoiceUnit,
    ChoicePartsHead,
    ChoicePartsBody,
    ChoicePartsLArm,
    ChoicePartsRArm,
    ChoicePartsLeg,
    ChoiceWeapon,
}
public class Command : MonoBehaviour
{
    [SerializeField]
    CommandType commandType = CommandType.ChoiceUnit;
    [SerializeField]
    int commandNumber = 0;
    [SerializeField]
    GameObject gard;
    BaseStage baseStage;
    private void Start()
    {
        baseStage = BaseStage.Instance;
    }
    public void OnClickThis()
    {
        switch (commandType)
        {
            case CommandType.ChoiceUnit:
                baseStage.SetUpUnitChange(commandNumber);
                break;
            case CommandType.ChoicePartsHead:
                break;
            case CommandType.ChoicePartsBody:
                break;
            case CommandType.ChoicePartsLArm:
                break;
            case CommandType.ChoicePartsRArm:
                break;
            case CommandType.ChoicePartsLeg:
                break;
            case CommandType.ChoiceWeapon:
                break;
            default:
                break;
        }
    }
}
