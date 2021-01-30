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
    private bool gard = false;
    public bool SwithGard { get; set; }
    GameManager gameManager;
    [SerializeField]
    UnitPriceCalculator calculator;
    private int partsTotalPlice;
    int j = 1;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        partsList = UnitPartsList.Instance;
        unitNumber.text = "機体番号：" + j +"\n価格" + calculator.GetPraice(0);
        guide.Clear();
        SwithGard = true;
        FirstSet();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (partsList.GetPartsHeadCount() > partsNumber)
            {
                GameManager.UnitDatas[SetUpUnit].HeadID = partsNumber;
            }
            if (partsList.GetPartsBodyCount() > partsNumber)
            {
                GameManager.UnitDatas[SetUpUnit].BodyID = partsNumber;
            }
            if (partsList.GetPartsLArmCount() > partsNumber)
            {
                GameManager.UnitDatas[SetUpUnit].LArmID = partsNumber;
            }
            if (partsList.GetWeaponCount() > partsNumber)
            {
                GameManager.UnitDatas[SetUpUnit].WeaponLID = partsNumber;
            }
            if (partsList.GetPartsRArmCount() > partsNumber)
            {
                GameManager.UnitDatas[SetUpUnit].RArmID = partsNumber;
            }
            if (partsList.GetWeaponCount() > partsNumber)
            {
                GameManager.UnitDatas[SetUpUnit].WeaponRID = partsNumber;
            }
            if (partsList.GetPartsLegCount() > partsNumber)
            {
                GameManager.UnitDatas[SetUpUnit].LegID = partsNumber;
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
        if (SwithGard)
        {
            if (gard)
            {
                SwithGard = false;
                gard = false;
            }
            else
            {
                gard = true;
            }
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
            partsTotalPlice = Body.GetPrice() + Head.GetPrice() + LArm.GetPrice() + RArm.GetPrice() + Leg.GetPrice() + LArmWeapon.GetPrice() + RArmWeapon.GetPrice();
            unitNumber.text = "機体番号：" + j + "\n価格" + partsTotalPlice;
        }
    }
    private void FirstSet()
    {
        HeadID = GameManager.UnitDatas[SetUpUnit].HeadID;
        BodyID = GameManager.UnitDatas[SetUpUnit].BodyID;
        LArmID = GameManager.UnitDatas[SetUpUnit].LArmID;
        WeaponLID = GameManager.UnitDatas[SetUpUnit].WeaponLID;
        RArmID = GameManager.UnitDatas[SetUpUnit].RArmID;
        WeaponRID = GameManager.UnitDatas[SetUpUnit].WeaponRID;
        LegID = GameManager.UnitDatas[SetUpUnit].LegID;
    }
    public void FinishBuild()
    {
        GameManager.UnitDatas[SetUpUnit].HeadID = HeadID;
        GameManager.UnitDatas[SetUpUnit].BodyID = BodyID;
        GameManager.UnitDatas[SetUpUnit].LArmID = LArmID;
        GameManager.UnitDatas[SetUpUnit].WeaponLID = WeaponLID;
        GameManager.UnitDatas[SetUpUnit].RArmID = RArmID;
        GameManager.UnitDatas[SetUpUnit].WeaponRID = WeaponRID;
        GameManager.UnitDatas[SetUpUnit].LegID = LegID;
        GameManager.Instance.SceneChange(3);
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
            GameManager.UnitDatas[SetUpUnit].HeadID = HeadID;
            GameManager.UnitDatas[SetUpUnit].BodyID = BodyID;
            GameManager.UnitDatas[SetUpUnit].LArmID = LArmID;
            GameManager.UnitDatas[SetUpUnit].WeaponLID = WeaponLID;
            GameManager.UnitDatas[SetUpUnit].RArmID = RArmID;
            GameManager.UnitDatas[SetUpUnit].WeaponRID = WeaponRID;
            GameManager.UnitDatas[SetUpUnit].LegID = LegID;
            SetUpUnit = i;
            ResetBuild();
            j = i + 1;
            unitNumber.text = "機体番号：" + j + "\n価格" + calculator.GetPraice(i);
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
    public void GuideHead()
    {
        guide.HeadData(partsList.GetHeadObject(HeadID).GetComponent<PartsHead>());
    }
    public void GuideBody()
    {
        guide.BodyData(partsList.GetBodyObject(BodyID).GetComponent<PartsBody>());
    }
    public void GuideLArm()
    {
        guide.LRArmData(partsList.GetLArmObject(LArmID).GetComponent<PartsLArm>());
    }
    public void GuideRArm()
    {
        guide.LRArmData(partsList.GetRArmObject(RArmID).GetComponent<PartsRArm>());
    }
    public void GuideLeg()
    {
        guide.LegData(partsList.GetLegObject(LegID).GetComponent<PartsLeg>());
    }
    public void GuideWeaponL()
    {
        guide.WeaponData(partsList.GetWeaponObject(WeaponLID).GetComponent<Weapon>());
    }
    public void GuideWeaponR()
    {
        guide.WeaponData(partsList.GetWeaponObject(WeaponRID).GetComponent<Weapon>());
    }
}
