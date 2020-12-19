using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

/// <summary>
/// 通常向き4方向
/// </summary>
public enum UnitAngle
{
    Up,
    Down,
    Left,
    Right,
}
public class Unit : MonoBehaviour
{
    public class Target
    {
        public Unit TargetUnit { get; set; }
        public int TargetPoint { get; set; }
        public int PosX { get; set; }
        public int PosZ { get; set; }
        public Target(Unit target, int point, int x, int z)
        {
            TargetUnit = target;
            TargetPoint = point;
            PosX = x;
            PosZ = z;
        }
    }

    [SerializeField]
    public UnitAngle unitAngle = UnitAngle.Down;//初期方向
    protected UnitAngle currentAngle;//現在の方向
    [SerializeField]
    protected int movePower = 10;//仮
    [SerializeField]
    protected float liftingForce = 2.0f;//仮
    [SerializeField]
    protected int maxHp = 100;//仮
    [SerializeField] protected int startPosX;//初期位置
    [SerializeField] protected int startPosZ;//初期位置

    public int CurrentHp { get; protected set; }
    public int CurrentPosX { get; protected set; } = 0;//現在位置
    public int CurrentPosZ { get; protected set; } = 0;//現在位置
    [SerializeField] float detectionRange;
    public float DetectionRange { get; protected set; }//索敵範囲
    public float CurrentPosY { get; set; }

    protected UnitManager unitManager;
    protected Map gameMap;
    protected Stage gameStage;
    protected UnitPartsList partsList;

    protected List<int[]> unitMoveList;
    //――――――――移動描写関連――――――――
    [SerializeField]
    protected float moveSpeed = 1;
    protected bool moveMood = false;
    public bool MoveNow { get; protected set; } = false;
    protected int moveCount = -1;
    protected float movePosX;
    protected float movePosZ;
    protected float moveTargetPosX;
    protected float moveTargetPosZ;
    protected float moveLevel;
    protected float moveTargetLevel;
    //―――――――――――――――――――――――
    [SerializeField]
    protected Weapon haveWeapon;
    public int Defense { get; protected set; } = 20;

    public PartsHead Head { get; protected set; }
    public PartsBody Body { get; protected set; } = null;
    public PartsLArm LArm { get; protected set; }
    public PartsRArm RArm { get; protected set; }
    public PartsLeg Leg { get; protected set; }
    public Weapon LArmWeapon { get; protected set; }
    public Weapon RArmWeapon { get; protected set; }
    protected bool silhouetteOn = false;
    public bool DestroyBody { get; protected set; } = false;

    public bool ActionTurn { get; set; }
    protected void Awake()
    {
        CurrentHp = maxHp;
        CurrentPosX = startPosX;
        CurrentPosZ = startPosZ;
    }
    protected void Start()
    {
        gameMap = Map.Instans;
        gameStage = Stage.StageDate;
        unitManager = UnitManager.Instance;
        partsList = UnitPartsList.Instance;
        DetectionRange = detectionRange;
        //CurrentPosY = gameMap.MapDates[CurrentPosX][CurrentPosZ].Level;
        CurrentPosY = gameMap.MapDates2[CurrentPosX + (gameMap.maxX * CurrentPosZ)].Level;
        transform.position = new Vector3(CurrentPosX * gameMap.mapScale, CurrentPosY, CurrentPosZ * gameMap.mapScale);
        StartUnitAngle();
    }

    /// <summary>
    /// 移動力渡し
    /// </summary>
    /// <returns>移動力</returns>
    public int GetMovePower() { return movePower; }
    /// <summary>
    /// 昇降力渡し
    /// </summary>
    /// <returns>昇降力</returns>
    public float GetLiftingForce() { return liftingForce; }

    public int GetMaxHp() { return maxHp; }
    
    /// <summary>
    /// 撃破処理
    /// </summary>
    protected void Dead()
    {
        EffectManager.PlayEffect(EffectID.HyperExplosion, transform.position);
        DestroyBody = true;
        gameStage.SetUnitPos();
        gameObject.SetActive(false);
    }
    /// <summary>
    /// ユニット瞬間移動
    /// </summary>
    /// <param name="posX">目標地点X軸</param>
    /// <param name="posZ">目標地点Z軸</param>
    public void UnitMove(int posX, int posZ)
    {
        CurrentPosX = posX;
        CurrentPosZ = posZ;
        //CurrentPosY = gameMap.MapDates[CurrentPosX][CurrentPosZ].Level;
        CurrentPosY = gameMap.MapDates2[CurrentPosX + (gameMap.maxX * CurrentPosZ)].Level;
        transform.position = new Vector3(CurrentPosX * gameMap.mapScale, CurrentPosY, CurrentPosZ * gameMap.mapScale);
    }

    /// <summary>
    /// 検索範囲マップ移動経路検索・移動指示
    /// </summary>
    /// <param name="moveList">検索範囲マップ</param>
    /// <param name="targetX">開始地点X軸</param>
    /// <param name="targetZ">開始地点Z軸</param>
    public void UnitMove(List<List<Map.MapDate>> moveList, int targetX, int targetZ)
    {
        unitMoveList = new List<int[]>();
        int[] pos = { targetX, targetZ };
        unitMoveList.Add(pos); //目標データ保存
        SearchCross(targetX, targetZ, moveList[targetX][targetZ].movePoint, moveList);
    }
    public void UnitMove2(List<Map.MapDate> moveList, int targetX, int targetZ)
    {
        unitMoveList = new List<int[]>();
        int[] pos = { targetX, targetZ };
        unitMoveList.Add(pos); //目標データ保存
        int p = targetX + (targetZ * gameMap.maxX);
        SearchCross2(p, moveList[p].movePoint, moveList);
    }
    /// <summary>
    /// ユニット移動処理
    /// </summary>
    protected void UnitMove()
    {
        if (moveCount >= 0 && !MoveNow)//移動経路が残っているなら、初期地点・目標地点を設定、移動開始指示（一マスずつ移動させる）
        {
            Vector3 thisPos = transform.position;
            movePosX = thisPos.x;
            movePosZ = thisPos.z;
            moveLevel = thisPos.y;
            moveTargetPosX = unitMoveList[moveCount][0] * gameMap.mapScale;
            moveTargetPosZ = unitMoveList[moveCount][1] * gameMap.mapScale;
            //moveTargetLevel = gameMap.MapDates[unitMoveList[moveCount][0]][unitMoveList[moveCount][1]].Level;
            moveTargetLevel = gameMap.MapDates2[unitMoveList[moveCount][0]+(gameMap.maxX * unitMoveList[moveCount][1])].Level;
            MoveNow = true;
            StartUnitAngle();
        }

        if (movePosX != moveTargetPosX && MoveNow) //移動・昇降、方向変更処理
        {
            if (movePosX < moveTargetPosX)
            {
                if (unitAngle != UnitAngle.Right) { unitAngle = UnitAngle.Right; }

                if (moveTargetPosX - movePosX <= gameMap.mapScale / 2 && moveLevel != moveTargetLevel)//昇降処理の確認
                {
                    if (moveLevel > moveTargetLevel)
                    {
                        moveLevel -= 0.2f * moveSpeed * Time.deltaTime;
                        if (moveLevel < moveTargetLevel) { moveLevel = moveTargetLevel; }
                    }
                    else
                    {
                        moveLevel += 0.2f * moveSpeed * Time.deltaTime;
                        if (moveLevel > moveTargetLevel) { moveLevel = moveTargetLevel; }
                    }
                }
                else
                {
                    movePosX += 0.5f * moveSpeed * Time.deltaTime;
                    if (movePosX > moveTargetPosX) { movePosX = moveTargetPosX; }
                }
            }
            else
            {
                if (unitAngle != UnitAngle.Left) { unitAngle = UnitAngle.Left; }

                if (movePosX - moveTargetPosX <= gameMap.mapScale / 2 && moveLevel != moveTargetLevel)//昇降処理の確認
                {
                    if (moveLevel > moveTargetLevel)
                    {
                        moveLevel -= 0.2f * moveSpeed * Time.deltaTime;
                        if (moveLevel < moveTargetLevel) { moveLevel = moveTargetLevel; }
                    }
                    else
                    {
                        moveLevel += 0.2f * moveSpeed * Time.deltaTime;
                        if (moveLevel > moveTargetLevel) { moveLevel = moveTargetLevel; }
                    }
                }
                else
                {
                    movePosX -= 0.5f * moveSpeed * Time.deltaTime;
                    if (movePosX < moveTargetPosX) { movePosX = moveTargetPosX; }
                }
            }
        }
        if (movePosZ != moveTargetPosZ && MoveNow)
        {
            if (movePosZ < moveTargetPosZ)
            {
                if (unitAngle != UnitAngle.Up) { unitAngle = UnitAngle.Up; }

                if (moveTargetPosZ - movePosZ <= gameMap.mapScale / 2 && moveLevel != moveTargetLevel)//昇降処理の確認
                {
                    if (moveLevel > moveTargetLevel)
                    {
                        moveLevel -= 0.2f * moveSpeed * Time.deltaTime;
                        if (moveLevel < moveTargetLevel) { moveLevel = moveTargetLevel; }
                    }
                    else
                    {
                        moveLevel += 0.2f * moveSpeed * Time.deltaTime;
                        if (moveLevel > moveTargetLevel) { moveLevel = moveTargetLevel; }
                    }
                }
                else
                {
                    movePosZ += 0.5f * moveSpeed * Time.deltaTime;
                    if (movePosZ > moveTargetPosZ) { movePosZ = moveTargetPosZ; }
                }
            }
            else
            {
                if (unitAngle != UnitAngle.Down) { unitAngle = UnitAngle.Down; }

                if (movePosZ - moveTargetPosZ <= gameMap.mapScale / 2 && moveLevel != moveTargetLevel)//昇降処理の確認
                {
                    if (moveLevel > moveTargetLevel)
                    {
                        moveLevel -= 0.2f * moveSpeed * Time.deltaTime;
                        if (moveLevel < moveTargetLevel) { moveLevel = moveTargetLevel; }
                    }
                    else
                    {
                        moveLevel += 0.2f * moveSpeed * Time.deltaTime;
                        if (moveLevel > moveTargetLevel) { moveLevel = moveTargetLevel; }
                    }
                }
                else
                {
                    movePosZ -= 0.5f * moveSpeed * Time.deltaTime;
                    if (movePosZ < moveTargetPosZ) { movePosZ = moveTargetPosZ; }
                }
            }
        }

        transform.position = new Vector3(movePosX, moveLevel, movePosZ);

        if (movePosX == moveTargetPosX && movePosZ == moveTargetPosZ && MoveNow)//移動停止、移動経路確認
        {
            MoveNow = false;
            moveCount--;
            if (moveCount < 0)//移動経路を消化し終わったなら移動モードを終了
            {
                moveMood = false;
            }
        }

    }

    /// <summary>
    /// 十字範囲の移動可能箇所を調べる
    /// </summary>
    /// <param name="x">X軸現在地</param>
    /// <param name="z">Z軸現在地</param>
    /// <param name="movePower">移動力</param>
    protected void SearchCross(int x, int z, int movePower, List<List<Map.MapDate>> moveList)
    {
        if (0 <= x && x < gameMap.maxX && 0 <= z && z < gameMap.maxZ)
        {
            MoveSearchPos(x, z - 1, movePower, moveList[x][z].Level, moveList, gameMap.MovePoint(gameMap.MapDates[x][z].MapType));
            MoveSearchPos(x, z + 1, movePower, moveList[x][z].Level, moveList, gameMap.MovePoint(gameMap.MapDates[x][z].MapType));
            MoveSearchPos(x - 1, z, movePower, moveList[x][z].Level, moveList, gameMap.MovePoint(gameMap.MapDates[x][z].MapType));
            MoveSearchPos(x + 1, z, movePower, moveList[x][z].Level, moveList, gameMap.MovePoint(gameMap.MapDates[x][z].MapType));
        }
    }
    protected void SearchCross2(int p, int movePower, List<Map.MapDate> moveList)
    {
        if (0 <= p && p < gameMap.maxX * gameMap.maxZ)
        {
            if (gameMap.MoveList2[p].PosZ > 0 && gameMap.MoveList2[p].PosZ < gameMap.maxZ)
            {
                MoveSearchPos2(p - gameMap.maxX, movePower, moveList[p].Level, moveList, gameMap.MovePoint(gameMap.MapDates2[p].MapType));
            }
            if (gameMap.MoveList2[p].PosZ >= 0 && gameMap.MoveList2[p].PosZ < gameMap.maxZ - 1)
            {
                MoveSearchPos2(p + gameMap.maxX, movePower, moveList[p].Level, moveList, gameMap.MovePoint(gameMap.MapDates2[p].MapType));
            }
            if (gameMap.MoveList2[p].PosX > 0 && gameMap.MoveList2[p].PosX < gameMap.maxX)
            {
                MoveSearchPos2(p - 1, movePower, moveList[p].Level, moveList, gameMap.MovePoint(gameMap.MapDates2[p].MapType));
            }
            if (gameMap.MoveList2[p].PosX >= 0 && gameMap.MoveList2[p].PosX < gameMap.maxX - 1)
            {
                MoveSearchPos2(p + 1, movePower, moveList[p].Level, moveList, gameMap.MovePoint(gameMap.MapDates2[p].MapType));
            }
        }
    }
    /// <summary>
    /// 対象座標の確認
    /// </summary>
    /// <param name="x">対象X軸</param>
    /// <param name="z">対象Z軸</param>
    /// <param name="movePower">移動力</param>
    /// <param name="currentLevel">現在地高度</param>
    /// <param name="moveList">移動範囲リスト</param>
    /// <param name="moveCost">移動前座標の移動コスト</param>
    protected void MoveSearchPos(int x, int z, int movePower, float currentLevel, List<List<Map.MapDate>> moveList, int moveCost)
    {
        if (x < 0 || z < 0 || x >= gameMap.maxX || z >= gameMap.maxZ) { return; }//マップ範囲内か確認
        if (movePower + moveCost != moveList[x][z].movePoint) { return; } //一つ前の座標か確認        
        if (moveMood) { return; }//検索終了か確認
        if (moveList[x][z].Level >= currentLevel) //高低差確認
        {
            if (moveList[x][z].Level - currentLevel > liftingForce) { return; }
        }
        else
        {
            if (currentLevel - moveList[x][z].Level > liftingForce) { return; }
        }

        movePower = moveList[x][z].movePoint;

        int[] pos = { x, z };
        unitMoveList.Add(pos); //移動順データ保存
        if (CurrentPosX == x && CurrentPosZ == z) //初期地点か確認
        {
            moveMood = true; //移動モード移行
            moveCount = unitMoveList.Count - 1;//移動経路数を入力
        }
        else
        {
            SearchCross(x, z, movePower, moveList);
        }
    }
    protected void MoveSearchPos2(int p, int movePower, float currentLevel, List<Map.MapDate> moveList, int moveCost)
    {
        if (moveMood) { return; }//検索終了か確認
        if (p < 0 || p >= gameMap.maxX * gameMap.maxZ) { return; }//マップ範囲内か確認
        if (movePower + moveCost != moveList[p].movePoint) { return; } //一つ前の座標か確認     
        if (moveList[p].Level >= currentLevel) //高低差確認
        {
            if (moveList[p].Level - currentLevel > liftingForce) { return; }
        }
        else
        {
            if (currentLevel - moveList[p].Level > liftingForce) { return; }
        }

        movePower = moveList[p].movePoint;

        int[] pos = { gameMap.MoveList2[p].PosX, gameMap.MoveList2[p].PosZ };
        unitMoveList.Add(pos); //移動順データ保存
        if (CurrentPosX == gameMap.MoveList2[p].PosX && CurrentPosZ == gameMap.MoveList2[p].PosZ) //初期地点か確認
        {
            moveMood = true; //移動モード移行
            moveCount = unitMoveList.Count - 1;//移動経路数を入力
        }
        else
        {
            SearchCross2(p, movePower, moveList);
        }
    }
    /// <summary>
    /// 向き変更
    /// </summary>
    protected void UnitAngleControl()
    {
        if (currentAngle != unitAngle)
        {
            currentAngle = unitAngle;
            switch (currentAngle)
            {
                case UnitAngle.Up:
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case UnitAngle.Down:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case UnitAngle.Left:
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case UnitAngle.Right:
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 向きリセットセット
    /// </summary>
    protected void StartUnitAngle()
    {
        currentAngle = unitAngle;
        switch (currentAngle)
        {
            case UnitAngle.Up:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case UnitAngle.Down:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case UnitAngle.Left:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case UnitAngle.Right:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            default:
                break;
        }
        if (silhouetteOn)//パーツ向きリセット
        {
            Head.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Body.transform.localRotation = Quaternion.Euler(0, 0, 0);
            LArm.transform.localRotation = Quaternion.Euler(0, 0, 0);
            LArm.ArmParts().transform.localRotation = Quaternion.Euler(0, 0, 0);
            RArm.transform.localRotation = Quaternion.Euler(0, 0, 0);
            RArm.ArmParts().transform.localRotation = Quaternion.Euler(0, 0, 0);
            Leg.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    protected bool attackMode = false;
    protected bool attackTrigger = false;
    protected bool attackNow = false;
    protected float attackTimer = 0;
    protected Weapon attackWeapon = null;
    protected void AttackSystem()
    {
        if (attackMode)
        {
            if (attackWeapon != null)
            {
                if (attackNow && !attackWeapon.AttackNow)
                {
                    attackNow = false;
                    attackMode = false;
                    attackTrigger = false;
                    //StartUnitAngle();
                }
                if (attackTrigger)
                {
                    attackTimer += Time.deltaTime;
                    if (attackTimer > 0.5 && !attackWeapon.AttackNow)
                    {
                        attackWeapon.Shot();
                        attackNow = true;
                        attackTrigger = false;
                        attackTimer = 0;
                    }
                }
            }
            else
            {
                attackTrigger = false;
                attackMode = false;
            }
        }
    }

    protected void MoveSystem()
    {
        if (moveMood)
        {
            UnitMove();
        }
    }
    public void MoveFinishSet()
    {
        if (!moveMood)//移動終了で位置を保存
        {
            Vector3 thisPos = transform.position;
            if (CurrentPosX != (int)Math.Round(thisPos.x) / gameMap.mapScale || CurrentPosZ != (int)Math.Round(thisPos.z) / gameMap.mapScale)
            {
                CurrentPosX = (int)Math.Round(thisPos.x) / gameMap.mapScale;
                CurrentPosZ = (int)Math.Round(thisPos.z) / gameMap.mapScale;
                //CurrentPosY = gameMap.MapDates[CurrentPosX][CurrentPosZ].Level;
                CurrentPosY = gameMap.MapDates2[CurrentPosX + (gameMap.maxX * CurrentPosZ)].Level;
            }
            gameStage.SetUnitPos();
        }
    }

    protected void TurnFinishSystem(UnitAngle angle)
    {
        unitAngle = angle;
        StartUnitAngle();
        ActionTurn = false;
    }
    /// <summary>
    /// ターゲットに攻撃
    /// </summary>
    /// <param name="targetUnit"></param>
    public void TargetShot(Unit targetUnit)
    {
        Vector3 targetPos = targetUnit.transform.position;
        Vector3 targetDir = targetPos - transform.position;
        targetDir.y = 0.0f;
        Quaternion p = Quaternion.Euler(0, 180, 0);
        Quaternion endRot = Quaternion.LookRotation(targetDir) * p;  //< 方向からローテーションに変換する
        transform.rotation = endRot;
        haveWeapon.Shot();
    }
    /// <summary>
    /// ターゲットに攻撃
    /// </summary>
    /// <param name="targetUnit"></param>
    public void TargetShot(Unit targetUnit, UnitParts attackArm)
    {
        //MoveFinishSet();
        Vector3 targetPos = targetUnit.Body.transform.position;
        Vector3 targetDir = targetPos - transform.position;
        targetDir.y = 0.0f;
        Quaternion p = Quaternion.Euler(0, 180, 0);
        Quaternion endRot = Quaternion.LookRotation(targetDir) * p;  //< 方向からローテーションに変換する
        transform.rotation = endRot;
        if (attackArm == LArm)
        {
            targetDir = targetPos - LArm.ArmParts().transform.position;
            endRot = Quaternion.LookRotation(targetDir) * p;
            LArm.ArmParts().transform.rotation = endRot;
            attackWeapon = LArmWeapon;
        }
        else if (attackArm == RArm)
        {
            targetDir = targetPos - RArm.ArmParts().transform.position;
            endRot = Quaternion.LookRotation(targetDir) * p;
            RArm.ArmParts().transform.rotation = endRot;
            attackWeapon = RArmWeapon;
        }
        else
        {
            attackWeapon = null;
        }
        attackTimer = 0;
        attackMode = true;
    }
    /// <summary>
    /// ターゲットに攻撃
    /// </summary>
    /// <param name="targetUnit"></param>
    public void LArmTargetShot(Unit targetUnit)
    {
        Vector3 targetPos = targetUnit.Body.transform.position;
        Vector3 targetDir = targetPos - transform.position;
        targetDir.y = 0.0f;
        Quaternion p = Quaternion.Euler(0, 180, 0);
        Quaternion endRot = Quaternion.LookRotation(targetDir) * p;  //< 方向からローテーションに変換する
        transform.rotation = endRot;
        targetDir = targetPos - LArm.ArmParts().transform.position;
        endRot = Quaternion.LookRotation(targetDir) * p;
        LArm.ArmParts().transform.rotation = endRot;
        LArmWeapon.Shot();
    }
    /// <summary>
    /// ターゲットに攻撃
    /// </summary>
    /// <param name="targetUnit"></param>
    public void RArmTargetShot(Unit targetUnit)
    {
        Vector3 targetPos = targetUnit.Body.transform.position;
        Vector3 targetDir = targetPos - transform.position;
        targetDir.y = 0.0f;
        Quaternion p = Quaternion.Euler(0, 180, 0);
        Quaternion endRot = Quaternion.LookRotation(targetDir) * p;  //< 方向からローテーションに変換する
        transform.rotation = endRot;
        targetDir = targetPos - RArm.ArmParts().transform.position;
        endRot = Quaternion.LookRotation(targetDir) * p;
        RArm.ArmParts().transform.rotation = endRot;
        RArmWeapon.Shot();
    }
    /// <summary>
    /// 指定した見た目のユニットを生成する
    /// </summary>
    /// <param name="headID">ヘッドパーツID</param>
    /// <param name="bodyID">ボディパーツID</param>
    /// <param name="lArmID">レフトアームパーツID</param>
    /// <param name="weaponLID">レフトアーム装備武器ID</param>
    /// <param name="rArmID">ライトアームパーツID</param>
    /// <param name="weaponRID">ライトアーム装備武器ID</param>
    /// <param name="legID">レッグパーツID</param>
    public void UnitCreate(int headID, int bodyID, int lArmID, int weaponLID, int rArmID, int weaponRID, int legID)
    {
        if (!silhouetteOn)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);//パーツ生成時に向きを合わせる
            GameObject leg = Instantiate(partsList.GetLegObject(legID));
            leg.transform.position = transform.position;
            leg.transform.parent = transform;
            Leg = leg.GetComponent<PartsLeg>();
            Leg.SetOwner(this);
            GameObject body = Instantiate(partsList.GetBodyObject(bodyID));
            body.transform.parent = transform;
            Body = body.GetComponent<PartsBody>();
            Body.SetOwner(this);
            Body.TransFormParts(Leg.GetPartsHigh().position);
            GameObject head = Instantiate(partsList.GetHeadObject(headID));
            head.transform.parent = transform;
            Head = head.GetComponent<PartsHead>();
            Head.SetOwner(this);
            Head.TransFormParts(Body.GetHeadPos().position);
            GameObject lArm = Instantiate(partsList.GetLArmObject(lArmID));
            lArm.transform.parent = transform;
            LArm = lArm.GetComponent<PartsLArm>();
            LArm.SetOwner(this);
            LArm.TransFormParts(Body.GetLArmPos().position);
            GameObject weaponL = Instantiate(partsList.GetWeaponObject(weaponLID));
            weaponL.transform.parent = LArm.ArmParts().transform;
            LArmWeapon = weaponL.GetComponent<Weapon>();
            LArmWeapon.TransFormParts(LArm.GetGrip().position);
            GameObject rArm = Instantiate(partsList.GetRArmObject(rArmID));
            rArm.transform.parent = transform;
            RArm = rArm.GetComponent<PartsRArm>();
            RArm.SetOwner(this);
            RArm.TransFormParts(Body.GetRArmPos().position);
            GameObject weaponR = Instantiate(partsList.GetWeaponObject(weaponRID));
            weaponR.transform.parent = RArm.ArmParts().transform;
            RArmWeapon = weaponR.GetComponent<Weapon>();
            RArmWeapon.TransFormParts(RArm.GetGrip().position);
            StartUnitAngle();//向きを戻す
            silhouetteOn = true;
        }
    }

    /// <summary>
    /// パーツデータを反映
    /// </summary>
    protected void PartsUpdate()
    {
        if (silhouetteOn)
        {
            if (movePower != Leg.MovePower)
            {
                movePower = Leg.MovePower;
            }
            if (moveSpeed != Leg.MoveSpeed)
            {
                moveSpeed = Leg.MoveSpeed;
            }
            if (CurrentHp != Body.CurrentPartsHp + Head.CurrentPartsHp + LArm.CurrentPartsHp + RArm.CurrentPartsHp + Leg.CurrentPartsHp)
            {
                CurrentHp = Body.CurrentPartsHp + Head.CurrentPartsHp + LArm.CurrentPartsHp + RArm.CurrentPartsHp + Leg.CurrentPartsHp;
            }
            if (liftingForce != Body.LiftingForce)
            {
                liftingForce = Body.LiftingForce;
            }
            if (DetectionRange != Head.DetectionRange)
            {
                DetectionRange = Head.DetectionRange;
            }
            if (Body.CurrentPartsHp <= 0)
            {
                Dead();
            }            
        }
    }

    public void OnClickThis()
    {
        TargetCursor.instance.SetCursor(this);
    }
}
