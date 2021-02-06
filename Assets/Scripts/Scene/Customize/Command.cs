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
    private bool setGard = false;
    BaseStage baseStage;
    UnitPartsList partsList;
    [SerializeField]
    Text partsName;
    
    private void Start()
    {
        baseStage = BaseStage.Instance;
        partsList = UnitPartsList.Instance;
        switch (commandType)
        {
            case CommandType.ChoicePartsHead:
                partsName.text = partsList.GetHeadObject(commandNumber).GetComponent<PartsHead>().GetName();
                break;
            case CommandType.ChoicePartsBody:
                partsName.text = partsList.GetBodyObject(commandNumber).GetComponent<PartsBody>().GetName();
                break;
            case CommandType.ChoicePartsLArm:
                partsName.text = partsList.GetLArmObject(commandNumber).GetComponent<PartsLArm>().GetName();
                break;
            case CommandType.ChoicePartsRArm:
                partsName.text = partsList.GetRArmObject(commandNumber).GetComponent<PartsRArm>().GetName();
                break;
            case CommandType.ChoicePartsLeg:
                partsName.text = partsList.GetLegObject(commandNumber).GetComponent<PartsLeg>().GetName();
                break;
            case CommandType.ChoiceWeaponL:
                partsName.text = partsList.GetWeaponObject(commandNumber).GetComponent<Weapon>().GetName();
                break;
            case CommandType.ChoiceWeaponR:
                partsName.text = partsList.GetWeaponObject(commandNumber).GetComponent<Weapon>().GetName();
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (baseStage.SwithGard)
        {
            switch (commandType)
            {
                case CommandType.ChoiceUnit:
                    if (baseStage.SetUpUnit == commandNumber && !setGard)
                    {
                        gard.SetActive(true);
                        setGard = true;
                    }
                    if (baseStage.SetUpUnit != commandNumber && setGard)
                    {
                        gard.SetActive(false);
                        setGard = false;
                    }
                    break;
                case CommandType.ChoicePartsHead:
                    if (baseStage.HeadID == commandNumber && !setGard)
                    {
                        gard.SetActive(true);
                        setGard = true;
                    }
                    if (baseStage.HeadID != commandNumber && setGard)
                    {
                        gard.SetActive(false);
                        setGard = false;
                    }
                    break;
                case CommandType.ChoicePartsBody:
                    if (baseStage.BodyID == commandNumber && !setGard)
                    {
                        gard.SetActive(true);
                        setGard = true;
                    }
                    if (baseStage.BodyID != commandNumber && setGard)
                    {
                        gard.SetActive(false);
                        setGard = false;
                    }
                    break;
                case CommandType.ChoicePartsLArm:
                    if (baseStage.LArmID == commandNumber && !setGard)
                    {
                        gard.SetActive(true);
                        setGard = true;
                    }
                    if (baseStage.LArmID != commandNumber && setGard)
                    {
                        gard.SetActive(false);
                        setGard = false;
                    }
                    break;
                case CommandType.ChoicePartsRArm:
                    if (baseStage.RArmID == commandNumber && !setGard)
                    {
                        gard.SetActive(true);
                        setGard = true;
                    }
                    if (baseStage.RArmID != commandNumber && setGard)
                    {
                        gard.SetActive(false);
                        setGard = false;
                    }
                    break;
                case CommandType.ChoicePartsLeg:
                    if (baseStage.LegID == commandNumber && !setGard)
                    {
                        gard.SetActive(true);
                        setGard = true;
                    }
                    if (baseStage.LegID != commandNumber && setGard)
                    {
                        gard.SetActive(false);
                        setGard = false;
                    }
                    break;
                case CommandType.ChoiceWeaponL:
                    if (baseStage.WeaponLID == commandNumber && !setGard)
                    {
                        gard.SetActive(true);
                        setGard = true;
                    }
                    if (baseStage.WeaponLID != commandNumber && setGard)
                    {
                        gard.SetActive(false);
                        setGard = false;
                    }
                    break;
                case CommandType.ChoiceWeaponR:
                    if (baseStage.WeaponRID == commandNumber && !setGard)
                    {
                        gard.SetActive(true);
                        setGard = true;
                    }
                    if (baseStage.WeaponRID != commandNumber && setGard)
                    {
                        gard.SetActive(false);
                        setGard = false;
                    }
                    break;
                default:
                    break;
            }
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
                baseStage.SetUpWeaponL(commandNumber);
                break;
            case CommandType.ChoiceWeaponR:
                baseStage.SetUpWeaponR(commandNumber);
                break;
            default:
                break;
        }
        baseStage.SwithGard = true;
    }
    
}
