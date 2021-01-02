using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CommandType
{
    ChoiceUnit,
    ChoicePartsHead,
    ChoicePartsBody,
    ChoicePartsLArm,
    ChoicePartsRArm,
    ChoicePartsLeg,
    ChoiceWeaponL,
    ChoiceWeaponR,
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
    UnitPartsList partsList;
    [SerializeField]
    Text partsName;
    private void Start()
    {
        baseStage = BaseStage.Instance;
        switch (commandType)
        {
            case CommandType.ChoicePartsHead:
                partsList = UnitPartsList.Instance;
                partsName.text = partsList.GetHeadObject(commandNumber).GetComponent<PartsHead>().GetName();
                break;
            case CommandType.ChoicePartsBody:
                partsList = UnitPartsList.Instance;
                partsName.text = partsList.GetBodyObject(commandNumber).GetComponent<PartsBody>().GetName();
                break;
            case CommandType.ChoicePartsLArm:
                partsList = UnitPartsList.Instance;
                partsName.text = partsList.GetLArmObject(commandNumber).GetComponent<PartsLArm>().GetName();
                break;
            case CommandType.ChoicePartsRArm:
                partsList = UnitPartsList.Instance;
                partsName.text = partsList.GetRArmObject(commandNumber).GetComponent<PartsRArm>().GetName();
                break;
            case CommandType.ChoicePartsLeg:
                partsList = UnitPartsList.Instance;
                partsName.text = partsList.GetLegObject(commandNumber).GetComponent<PartsLeg>().GetName();
                break;
            case CommandType.ChoiceWeaponL:
            case CommandType.ChoiceWeaponR:
                partsList = UnitPartsList.Instance;
                partsName.text = partsList.GetWeaponObject(commandNumber).GetComponent<Weapon>().GetName();
                break;
            default:
                break;
        }
        
    }
    public void OnClickThis()
    {
        switch (commandType)
        {
            case CommandType.ChoiceUnit:
                baseStage.SetUpUnitChange(commandNumber);
                break;
            case CommandType.ChoicePartsHead:
                baseStage.SetUpPartsHead(commandNumber);
                break;
            case CommandType.ChoicePartsBody:
                baseStage.SetUpPartsBody(commandNumber);
                break;
            case CommandType.ChoicePartsLArm:
                baseStage.SetUpPartsLArm(commandNumber);
                break;
            case CommandType.ChoicePartsRArm:
                baseStage.SetUpPartsRArm(commandNumber);
                break;
            case CommandType.ChoicePartsLeg:
                baseStage.SetUpPartsLeg(commandNumber);
                break;
            case CommandType.ChoiceWeaponL:
                break;
            case CommandType.ChoiceWeaponR:
                break;
            default:
                break;
        }
    }
}
