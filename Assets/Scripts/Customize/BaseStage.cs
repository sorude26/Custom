using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseStage : MonoBehaviour
{
    public static BaseStage Instance { get; private set; }
    UnitPartsList partsList;
    public PartsHead Head { get; private set; } = null;
    public int HeadID { get; private set; }
    public PartsBody Body { get; private set; } = null;
    public int BodyID { get; private set; }
    public PartsLArm LArm { get; private set; } = null;
    public int LArmID { get; private set; }
    public PartsRArm RArm { get; private set; } = null;
    public int RArmID { get; private set; }
    public PartsLeg Leg { get; private set; } = null;
    public int LegID { get; private set; }
    public Weapon LArmWeapon { get; private set; } = null;
    public int WeaponLID { get; private set; }
    public Weapon RArmWeapon { get; private set; } = null;
    public int WeaponRID { get; private set; }
    bool silhouetteOn = false;
    public int SetUpUnit { get; private set; } = 0;
    public bool viewOpen = false;
    int partsNumber = 0;
    [SerializeField]
    Text unitNumber;
    [SerializeField]
    PartsGuide guide;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        partsList = UnitPartsList.Instance;
        unitNumber.text = "機体番号：" + 1;
        FirstSet();
        guide.Clear();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (partsList.GetPartsHeadCount() > partsNumber)
            {
                GameManager.HeadID[SetUpUnit] = partsNumber;
            }
            if (partsList.GetPartsBodyCount() > partsNumber)
            {
                GameManager.BodyID[SetUpUnit] = partsNumber;
            }
            if (partsList.GetPartsLArmCount() > partsNumber)
            {
                GameManager.LArmID[SetUpUnit] = partsNumber;
            }
            if (partsList.GetWeaponCount() > partsNumber)
            {
                GameManager.WeaponLID[SetUpUnit] = partsNumber;
            }
            if (partsList.GetPartsRArmCount() > partsNumber)
            {
                GameManager.RArmID[SetUpUnit] = partsNumber;
            }
            if (partsList.GetWeaponCount() > partsNumber)
            {
                GameManager.WeaponRID[SetUpUnit] = partsNumber;
            }
            if (partsList.GetPartsLegCount() > partsNumber)
            {
                GameManager.LegID[SetUpUnit] = partsNumber;
            }
            ResetBuild();
            partsNumber++;
        }
        // transform.Rotate(new Vector3(0, 0.1f, 0));
    }
    private void LateUpdate()
    {
        if (!silhouetteOn)
        {
            if (!Head)
            {
                BuildUnit(HeadID, BodyID, LArmID, WeaponLID, RArmID, WeaponRID, LegID);
            }
        }
        if (!Head)
        {
            silhouetteOn = false;
        }
    }
    public void BuildUnit(int headID, int bodyID, int lArmID, int weaponLID, int rArmID, int weaponRID, int legID)
    {
        if (!silhouetteOn)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);//パーツ生成時に向きを合わせる
            GameObject leg = Instantiate(partsList.GetLegObject(legID));
            leg.transform.position = transform.position;
            leg.transform.parent = transform;
            Leg = leg.GetComponent<PartsLeg>();
            GameObject body = Instantiate(partsList.GetBodyObject(bodyID));
            body.transform.parent = Leg.transform;
            Body = body.GetComponent<PartsBody>();
            Body.TransFormParts(Leg.GetPartsHigh().position);
            GameObject head = Instantiate(partsList.GetHeadObject(headID));
            head.transform.parent = Body.transform;
            Head = head.GetComponent<PartsHead>();
            Head.TransFormParts(Body.GetHeadPos().position);
            GameObject lArm = Instantiate(partsList.GetLArmObject(lArmID));
            lArm.transform.parent = Body.transform;
            LArm = lArm.GetComponent<PartsLArm>();
            LArm.TransFormParts(Body.GetLArmPos().position);
            GameObject weaponL = Instantiate(partsList.GetWeaponObject(weaponLID));
            weaponL.transform.parent = LArm.ArmParts().transform;
            LArmWeapon = weaponL.GetComponent<Weapon>();
            LArmWeapon.TransFormParts(LArm.GetGrip().position);
            GameObject rArm = Instantiate(partsList.GetRArmObject(rArmID));
            rArm.transform.parent = Body.transform;
            RArm = rArm.GetComponent<PartsRArm>();
            RArm.TransFormParts(Body.GetRArmPos().position);
            GameObject weaponR = Instantiate(partsList.GetWeaponObject(weaponRID));
            weaponR.transform.parent = RArm.ArmParts().transform;
            RArmWeapon = weaponR.GetComponent<Weapon>();
            RArmWeapon.TransFormParts(RArm.GetGrip().position);
            silhouetteOn = true;
        }
    }
    private void FirstSet()
    {
        HeadID = GameManager.HeadID[SetUpUnit];
        BodyID = GameManager.BodyID[SetUpUnit];
        LArmID = GameManager.LArmID[SetUpUnit];
        WeaponLID = GameManager.WeaponLID[SetUpUnit];
        RArmID = GameManager.RArmID[SetUpUnit];
        WeaponRID = GameManager.WeaponRID[SetUpUnit];
        LegID = GameManager.LegID[SetUpUnit];
    }
    public void ResetBuild()
    {
        RArmWeapon = null;
        LArmWeapon = null;
        RArm = null;
        LArm = null;
        Head = null;
        Body = null;
        Leg = null;
        foreach (Transform n in transform)
        {
            Destroy(n.gameObject);
        }
    }
    public void SetUpUnitChange(int i)
    {
        if (SetUpUnit != i)
        {
            GameManager.HeadID[SetUpUnit] = HeadID;
            GameManager.BodyID[SetUpUnit] = BodyID;
            GameManager.LArmID[SetUpUnit] = LArmID;
            GameManager.WeaponLID[SetUpUnit] = WeaponLID;
            GameManager.RArmID[SetUpUnit] = RArmID;
            GameManager.WeaponRID[SetUpUnit] = WeaponRID;
            GameManager.LegID[SetUpUnit] = LegID;
            SetUpUnit = i;
            ResetBuild();
            int j = i + 1;
            unitNumber.text = "機体番号：" + j;
            FirstSet();
        }
    }
    public void SetUpPartsHead(int i)
    {
        if (HeadID != i)
        {
            HeadID = i;
            guide.HeadData(partsList.GetHeadObject(i).GetComponent<PartsHead>());
            ResetBuild();
        }
    }
    public void SetUpPartsBody(int i)
    {
        if (BodyID != i)
        {
            BodyID = i;
            guide.BodyData(partsList.GetBodyObject(i).GetComponent<PartsBody>());
            ResetBuild();
        }
    }
    public void SetUpPartsLArm(int i)
    {
        if (LArmID != i)
        {
            LArmID = i;
            guide.LRArmData(partsList.GetLArmObject(i).GetComponent<PartsLArm>());
            ResetBuild();
        }
    }
    public void SetUpPartsRArm(int i)
    {
        if (RArmID != i)
        {
            RArmID = i;
            guide.LRArmData(partsList.GetRArmObject(i).GetComponent<PartsRArm>());
            ResetBuild();
        }
    }
    public void SetUpPartsLeg(int i)
    {
        if (LegID != i)
        {
            LegID = i;
            guide.LegData(partsList.GetLegObject(i).GetComponent<PartsLeg>());
            ResetBuild();
        }
    }
    public void SetUpWeaponL(int i)
    {
        if (WeaponLID != i)
        {
            WeaponLID = i;
            guide.WeaponData(partsList.GetWeaponObject(i).GetComponent<Weapon>());
            ResetBuild();
        }
    }
    public void SetUpWeaponR(int i)
    {
        if (WeaponRID != i)
        {
            WeaponRID = i;
            guide.WeaponData(partsList.GetWeaponObject(i).GetComponent<Weapon>());
            ResetBuild();
        }
    }
}
