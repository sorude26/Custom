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
        public float TargetPoint { get; set; }
        public int PosX { get; set; }
        public int PosZ { get; set; }
        public Target(Unit target, float point, int x, int z)
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
    protected float deadTimer = 0;
    protected int bomCount = 0;
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
        gameStage = Stage.Instance;
        unitManager = UnitManager.Instance;
        partsList = UnitPartsList.Instance;
        DetectionRange = detectionRange;
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
        if (Body)
        {
            gameStage.panelP.SetUnit(this);
            EffectManager.PlayEffect(EffectID.HyperExplosion, Body.GetBodyCentrer().position);
        }
        else
        {
            EffectManager.PlayEffect(EffectID.HyperExplosion, transform.position);
        }
        DestroyBody = true;
        gameStage.SetUnitPos();
        if (silhouetteOn)
        {
            legL1Rotaion = Quaternion.Euler(-10, 0, 1);
            legL2Rotaion = Quaternion.Euler(-20, 0, 0);
            legR1Rotaion = Quaternion.Euler(-20, 0, -1);
            legR2Rotaion = Quaternion.Euler(-10, 0, 0);
            legRotaion = Quaternion.Euler(-90, 0, 0);
            legRSpeed = 0.1f;
            legL1RSpeed = 1.0f;
            legL2RSpeed = 1.0f;
            legR1RSpeed = 1.0f;
            legR2RSpeed = 1.0f;
        }
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
        CurrentPosY = gameMap.MapDates2[CurrentPosX + (gameMap.maxX * CurrentPosZ)].Level;
        transform.position = new Vector3(CurrentPosX * gameMap.mapScale, CurrentPosY, CurrentPosZ * gameMap.mapScale);
    }

    /// <summary>
    /// 検索範囲マップ移動経路検索・移動指示
    /// </summary>
    /// <param name="moveList">検索範囲マップ</param>
    /// <param name="targetX">開始地点X軸</param>
    /// <param name="targetZ">開始地点Z軸</param>
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
            moveTargetLevel = gameMap.MapDates2[unitMoveList[moveCount][0] + (gameMap.maxX * unitMoveList[moveCount][1])].Level;
            MoveNow = true;
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
            StartUnitAngle();
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
    /// 向きリセット
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
        ResetPartsAngle();
    }
    protected void ResetPartsAngle()
    {
        if (silhouetteOn)//パーツ向きリセット
        {
            if (Body.unitType == UnitType.Human)
            {
                Head.transform.localRotation = Quaternion.Euler(0, 0, 0);
                Body.transform.localRotation = Quaternion.Euler(0, 0, 0);
                LArm.transform.localRotation = Quaternion.Euler(0, 0, 0);
                LArm.ArmParts().transform.localRotation = Quaternion.Euler(0, 0, 0);
                RArm.transform.localRotation = Quaternion.Euler(0, 0, 0);
                RArm.ArmParts().transform.localRotation = Quaternion.Euler(0, 0, 0);
                Leg.transform.localRotation = Quaternion.Euler(0, 0, 0);
                legL1.transform.localRotation = Quaternion.Euler(0, 0, 0);
                legL2.transform.localRotation = Quaternion.Euler(0, 0, 0);
                legR1.transform.localRotation = Quaternion.Euler(0, 0, 0);
                legR2.transform.localRotation = Quaternion.Euler(0, 0, 0);
                headRotaion = Quaternion.Euler(0, 0, 0);
                headRSpeed = 1.0f;
                bodyRotaion = Quaternion.Euler(0, 0, 0);
                bodyRSpeed = 1.0f;
                lArmRotaion = Quaternion.Euler(0, 0, 0);
                lArmRSpeed = 1.0f;
                rArmRotaion = Quaternion.Euler(0, 0, 0);
                rArmRSpeed = 1.0f;
                legRotaion = Quaternion.Euler(0, 0, 0);
                legRSpeed = 1.0f;
                legL1Rotaion = Quaternion.Euler(0, 0, 0);
                legL2Rotaion = Quaternion.Euler(0, 0, 0);
                legR1Rotaion = Quaternion.Euler(0, 0, 0);
                legR2Rotaion = Quaternion.Euler(0, 0, 0);
            }
            else if (Body.unitType == UnitType.Helicopter)
            {
                Body.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
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
                if (attackNow && !attackWeapon.AttackNow && attackWeapon.Type != WeaponType.Melee)
                {
                    attackNow = false;
                    attackMode = false;
                    attackTrigger = false;
                    //StartUnitAngle();
                }
                if (attackTrigger)
                {
                    if (attackWeapon.Type != WeaponType.Melee)
                    {
                        attackTimer += Time.deltaTime;
                        if (attackTimer > 1.0 && !attackWeapon.AttackNow)
                        {
                            attackWeapon.Shot();
                            attackNow = true;
                            attackTrigger = false;
                            attackTimer = 0;

                        }
                    }
                    else
                    {
                        attackTimer += Time.deltaTime;
                        if (attackTimer > 0.8)
                        {
                            if (!attackNow)
                            {
                                if (attackWeapon == LArmWeapon)
                                {
                                    if (attackWeapon.GetComponent<MeleeWeapon>().meleeType == MeleeType.Axe)
                                    {
                                        AttackPattern(2);
                                    }
                                    else
                                    {
                                        AttackPattern(6);
                                    }
                                }
                                else if (attackWeapon == RArmWeapon)
                                {
                                    if (attackWeapon.GetComponent<MeleeWeapon>().meleeType == MeleeType.Axe)
                                    {
                                        AttackPattern(3);
                                    }
                                    else
                                    {
                                        AttackPattern(7);
                                    }
                                }
                                bodyRSpeed = 15.0f;
                                headRSpeed = 15.0f;
                                lArmRSpeed = 15.0f;
                                rArmRSpeed = 15.0f;
                                attackWeapon.BladeAttackStart();
                                attackNow = true;
                                attackTimer = 0;
                            }
                            if (attackTimer > 1.0)
                            {
                                attackWeapon.BladeAttackEnd();
                                bodyRotaion = Quaternion.Euler(0, 0, 0);
                                headRotaion = Quaternion.Euler(0, 0, 0);
                                lArmRotaion = Quaternion.Euler(0, 0, 0);
                                rArmRotaion = Quaternion.Euler(0, 0, 0);
                                LArm.ArmParts().transform.localRotation = Quaternion.Euler(0, 0, 0);
                                RArm.ArmParts().transform.localRotation = Quaternion.Euler(0, 0, 0);
                                legL1Rotaion = Quaternion.Euler(0, 0, 0);
                                legL2Rotaion = Quaternion.Euler(0, 0, 0);
                                legR1Rotaion = Quaternion.Euler(0, 0, 0);
                                legR2Rotaion = Quaternion.Euler(0, 0, 0);
                                targtPos = new Vector3(0, 0, 0);
                                attackNow = false;
                                attackMode = false;
                                attackTrigger = false;
                                attackTimer = 0;
                            }
                        }
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
    public void TargetShot(Unit targetUnit, Weapon attackWeapon)
    {
        ResetPartsAngle();
        if (targetUnit != null)
        {
            if (Body.unitType == UnitType.Human)
            {
                CameraControl.Instans.UnitCamera(this);
                Vector3 targetPos = targetUnit.Body.GetBodyCentrer().position;
                Vector3 targetDir = targetPos - transform.position;
                targetDir.y = 0.0f;
                Quaternion p = Quaternion.Euler(0, 180, 0);
                Quaternion endRot = Quaternion.LookRotation(targetDir) * p;  //< 方向からローテーションに変換する
                transform.rotation = endRot;
                if (attackWeapon.Type != WeaponType.Melee)
                {
                    if (attackWeapon == LArmWeapon)
                    {
                        Body.transform.localRotation = Quaternion.Euler(0, 20, 0);
                        bodyRotaion = Quaternion.Euler(0, 20, 0);
                        Head.transform.localRotation = Quaternion.Euler(0, -20, 0);
                        headRotaion = Quaternion.Euler(0, -20, 0);
                        LArm.transform.localRotation = Quaternion.Euler(30, 0, 0);
                        lArmRotaion = Quaternion.Euler(30, 0, 0);
                        targetDir = targetPos - LArm.ArmParts().transform.position;
                        endRot = Quaternion.LookRotation(targetDir) * p;
                        LArm.ArmParts().transform.rotation = endRot;
                        this.attackWeapon = LArmWeapon;
                    }
                    else if (attackWeapon == RArmWeapon)
                    {
                        Body.transform.localRotation = Quaternion.Euler(0, -20, 0);
                        bodyRotaion = Quaternion.Euler(0, -20, 0);
                        Head.transform.localRotation = Quaternion.Euler(0, 20, 0);
                        headRotaion = Quaternion.Euler(0, 20, 0);
                        RArm.transform.localRotation = Quaternion.Euler(30, 0, 0);
                        rArmRotaion = Quaternion.Euler(30, 0, 0);
                        targetDir = targetPos - RArm.ArmParts().transform.position;
                        endRot = Quaternion.LookRotation(targetDir) * p;
                        RArm.ArmParts().transform.rotation = endRot;
                        this.attackWeapon = RArmWeapon;
                    }
                    else
                    {
                        this.attackWeapon = null;
                    }
                }
                else
                {
                    if (attackWeapon == LArmWeapon)
                    {
                        if (attackWeapon.GetComponent<MeleeWeapon>().meleeType == MeleeType.Axe)
                        {
                            AttackPattern(0);
                        }
                        else
                        {
                            AttackPattern(4);
                        }
                        this.attackWeapon = LArmWeapon;
                    }
                    else if (attackWeapon == RArmWeapon)
                    {
                        if (attackWeapon.GetComponent<MeleeWeapon>().meleeType == MeleeType.Axe)
                        {
                            AttackPattern(1);
                        }
                        else
                        {
                            AttackPattern(5);
                        }
                        this.attackWeapon = RArmWeapon;
                    }
                    else
                    {
                        this.attackWeapon = null;
                    }

                    bodyRSpeed = 6.0f;
                    headRSpeed = 6.0f;
                    lArmRSpeed = 6.0f;
                    rArmRSpeed = 6.0f;
                    legL1RSpeed = 6.0f;
                    legL2RSpeed = 6.0f;
                    legR1RSpeed = 6.0f;
                    legR2RSpeed = 6.0f;
                    targtPos = new Vector3(0, 0, -6.5f);
                }
            }
            else if (Body.unitType == UnitType.Helicopter)
            {
                CameraControl.Instans.UnitCamera(this);
                Vector3 targetPos = targetUnit.Body.GetBodyCentrer().position;
                Vector3 targetDir = targetPos - transform.position;
                targetDir.y = 0.0f;
                Quaternion p = Quaternion.Euler(0, 180, 0);
                Quaternion endRot = Quaternion.LookRotation(targetDir) * p;  //< 方向からローテーションに変換する
                transform.rotation = endRot;
                targetDir = targetPos - Body.GetBodyHand().transform.position;
                endRot = Quaternion.LookRotation(targetDir) * p;
                Body.GetBodyHand().transform.rotation = endRot;
                this.attackWeapon = LArmWeapon;
            }
        }
        attackTimer = 0;
        attackMode = true;
    }

    protected void AttackPattern(int i)
    {
        switch (i)
        {
            case 0:
                bodyRotaion = Quaternion.Euler(5, -60, 0);
                headRotaion = Quaternion.Euler(0, 60, 0);
                lArmRotaion = Quaternion.Euler(130, 0, 0);
                rArmRotaion = Quaternion.Euler(20, 0, 0);
                LArm.ArmParts().transform.localRotation = Quaternion.Euler(-60, 0, 0);
                legL1Rotaion = Quaternion.Euler(-20, 0, -5);
                legL2Rotaion = Quaternion.Euler(-10, 0, 0);
                legR1Rotaion = Quaternion.Euler(40, 0, 5);
                legR2Rotaion = Quaternion.Euler(-20, 0, 0);
                break;
            case 1:
                bodyRotaion = Quaternion.Euler(5, 60, 0);
                headRotaion = Quaternion.Euler(0, -60, 0);
                lArmRotaion = Quaternion.Euler(20, 0, 0);
                rArmRotaion = Quaternion.Euler(130, 0, 0);
                RArm.ArmParts().transform.localRotation = Quaternion.Euler(-60, 0, 0);
                legL1Rotaion = Quaternion.Euler(40, 0, -5);
                legL2Rotaion = Quaternion.Euler(-20, 0, 0);
                legR1Rotaion = Quaternion.Euler(-20, 0, 5);
                legR2Rotaion = Quaternion.Euler(-10, 0, 0);
                break;
            case 2:
                bodyRotaion = Quaternion.Euler(-20, 50, 0);
                headRotaion = Quaternion.Euler(0, -40, 0);
                lArmRotaion = Quaternion.Euler(10, 0, 0);
                rArmRotaion = Quaternion.Euler(-20, 0, 0);
                break;
            case 3:
                bodyRotaion = Quaternion.Euler(-20, -50, 0);
                headRotaion = Quaternion.Euler(0, 40, 0);
                lArmRotaion = Quaternion.Euler(-20, 0, 0);
                rArmRotaion = Quaternion.Euler(10, 0, 0);
                break;
            case 4:
                bodyRotaion = Quaternion.Euler(0, -70, 0);
                headRotaion = Quaternion.Euler(0, 70, 0);
                lArmRotaion = Quaternion.Euler(60, 30, 0);
                rArmRotaion = Quaternion.Euler(40, 0, 0);
                LArm.ArmParts().transform.localRotation = Quaternion.Euler(-60, -60, 0);
                legL1Rotaion = Quaternion.Euler(-20, 0, -5);
                legL2Rotaion = Quaternion.Euler(-10, 0, 0);
                legR1Rotaion = Quaternion.Euler(40, 0, 5);
                legR2Rotaion = Quaternion.Euler(-20, 0, 0);
                break;
            case 5:
                bodyRotaion = Quaternion.Euler(0, 70, 0);
                headRotaion = Quaternion.Euler(0, -70, 0);
                lArmRotaion = Quaternion.Euler(40, 0, 0);
                rArmRotaion = Quaternion.Euler(60, 30, 0);
                RArm.ArmParts().transform.localRotation = Quaternion.Euler(-60, 60, 0);
                legL1Rotaion = Quaternion.Euler(40, 0, -5);
                legL2Rotaion = Quaternion.Euler(-20, 0, 0);
                legR1Rotaion = Quaternion.Euler(-20, 0, 5);
                legR2Rotaion = Quaternion.Euler(-10, 0, 0);
                break;
            case 6:
                bodyRotaion = Quaternion.Euler(-20, 50, 0);
                headRotaion = Quaternion.Euler(5, -50, 0);
                lArmRotaion = Quaternion.Euler(90, -30, 10);
                rArmRotaion = Quaternion.Euler(-40, 0, -50);
                LArm.ArmParts().transform.localRotation = Quaternion.Euler(-80, 60, 0);
                break;
            case 7:
                bodyRotaion = Quaternion.Euler(-20, -50, 0);
                headRotaion = Quaternion.Euler(5, 50, 0);
                lArmRotaion = Quaternion.Euler(-40, 0, 50);
                rArmRotaion = Quaternion.Euler(90, 30, -10);
                RArm.ArmParts().transform.localRotation = Quaternion.Euler(-80, 60, 0);
                break;
            default:
                break;
        }
    }

    private float blur = 0;
    private float v = 0.5f;
    public void ShotCameraShake(float i)
    {
        blur = UnityEngine.Random.Range(-v, v);
        if (attackWeapon == LArmWeapon)
        {
            LArm.transform.localRotation = Quaternion.Euler(30 + blur * i, 0 + blur * i, 0 + blur * i);
            lArmRotaion = Quaternion.Euler(30, 0, 0);
            rArmRSpeed = 1.0f;
        }
        else if (attackWeapon == RArmWeapon)
        {
            RArm.transform.localRotation = Quaternion.Euler(30 + blur * i, 0 + blur * i, 0 + blur * i);
            rArmRotaion = Quaternion.Euler(30, 0, 0);
            rArmRSpeed = 1.0f;
        }
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
            legL1 = Leg.GetLegJointL1();
            legL2 = Leg.GetLegJointL2();
            legR1 = Leg.GetLegJointR1();
            legR2 = Leg.GetLegJointR2();
            GameObject body = Instantiate(partsList.GetBodyObject(bodyID));
            body.transform.parent = Leg.GetPartsHigh();
            Body = body.GetComponent<PartsBody>();
            Body.SetOwner(this);
            Body.TransFormParts(Leg.GetPartsHigh().position);
            GameObject head = Instantiate(partsList.GetHeadObject(headID));
            head.transform.parent = Body.transform;
            Head = head.GetComponent<PartsHead>();
            Head.SetOwner(this);
            Head.TransFormParts(Body.GetHeadPos().position);
            GameObject lArm = Instantiate(partsList.GetLArmObject(lArmID));
            lArm.transform.parent = Body.transform;
            LArm = lArm.GetComponent<PartsLArm>();
            LArm.SetOwner(this);
            LArm.TransFormParts(Body.GetLArmPos().position);
            GameObject weaponL = Instantiate(partsList.GetWeaponObject(weaponLID));
            weaponL.transform.parent = LArm.ArmParts().transform;
            LArmWeapon = weaponL.GetComponent<Weapon>();
            LArmWeapon.SetOwner(this);
            LArmWeapon.TransFormParts(LArm.GetGrip().position);
            GameObject rArm = Instantiate(partsList.GetRArmObject(rArmID));
            rArm.transform.parent = Body.transform;
            RArm = rArm.GetComponent<PartsRArm>();
            RArm.SetOwner(this);
            RArm.TransFormParts(Body.GetRArmPos().position);
            GameObject weaponR = Instantiate(partsList.GetWeaponObject(weaponRID));
            weaponR.transform.parent = RArm.ArmParts().transform;
            RArmWeapon = weaponR.GetComponent<Weapon>();
            RArmWeapon.SetOwner(this);
            RArmWeapon.TransFormParts(RArm.GetGrip().position);
            StartUnitAngle();//向きを戻す
            silhouetteOn = true;
        }
    }
    public void UnitCreate(int bodyID, int weaponLID)
    {
        if (!silhouetteOn)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);//パーツ生成時に向きを合わせる
            GameObject body = Instantiate(partsList.GetBodyObject(bodyID));
            body.transform.position = transform.position;
            body.transform.parent = transform;
            Body = body.GetComponent<PartsBody>();
            Body.SetOwner(this);
            GameObject weaponL = Instantiate(partsList.GetWeaponObject(weaponLID));
            weaponL.transform.parent = Body.GetBodyHand().transform;
            LArmWeapon = weaponL.GetComponent<Weapon>();
            LArmWeapon.SetOwner(this);
            LArmWeapon.TransFormParts(Body.GetBodyHand().transform.position);
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
            if (Body.unitType == UnitType.Human)
            {
                if (movePower != Leg.MovePower + Body.MovePower)
                {
                    movePower = Leg.MovePower + Body.MovePower;
                }
                if (moveSpeed != Leg.MoveSpeed)
                {
                    moveSpeed = Leg.MoveSpeed;
                }
                if (CurrentHp != Body.CurrentPartsHp + Head.CurrentPartsHp + LArm.CurrentPartsHp + RArm.CurrentPartsHp + Leg.CurrentPartsHp)
                {
                    if (CurrentHp - (Body.CurrentPartsHp + Head.CurrentPartsHp + LArm.CurrentPartsHp + RArm.CurrentPartsHp + Leg.CurrentPartsHp) < 20)
                    {
                        HitMotion();
                    }
                    else
                    {
                        DamgeMotion();
                    }
                    CurrentHp = Body.CurrentPartsHp + Head.CurrentPartsHp + LArm.CurrentPartsHp + RArm.CurrentPartsHp + Leg.CurrentPartsHp;
                }
                if (liftingForce != Body.LiftingForce + Leg.LiftingForce)
                {
                    liftingForce = Body.LiftingForce + Leg.LiftingForce;
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
            else if (true)
            {
                if (movePower != Body.MovePower)
                {
                    movePower = Body.MovePower;
                }
                if (CurrentHp != Body.CurrentPartsHp)
                {
                    CurrentHp = Body.CurrentPartsHp;
                }
                if (liftingForce != Body.LiftingForce)
                {
                    liftingForce = Body.LiftingForce;
                }
                if (DetectionRange != 90)
                {
                    DetectionRange = 90;
                }
                if (moveSpeed != 50)
                {
                    moveSpeed = 50;
                }
                if (Body.CurrentPartsHp <= 0)
                {
                    Dead();
                }
            }
        }
    }

    protected Quaternion headRotaion = Quaternion.Euler(0, 0, 0);
    protected float headRSpeed = 1.0f;
    protected Quaternion bodyRotaion = Quaternion.Euler(0, 0, 0);
    protected float bodyRSpeed = 1.0f;
    protected Quaternion lArmRotaion = Quaternion.Euler(0, 0, 0);
    protected float lArmRSpeed = 1.0f;
    protected Quaternion rArmRotaion = Quaternion.Euler(0, 0, 0);
    protected float rArmRSpeed = 1.0f;
    protected Quaternion legRotaion = Quaternion.Euler(0, 0, 0);
    protected float legRSpeed = 1.0f;
    protected Quaternion legL1Rotaion = Quaternion.Euler(0, 0, 0);
    protected float legL1RSpeed = 1.0f;
    protected Quaternion legL2Rotaion = Quaternion.Euler(0, 0, 0);
    protected float legL2RSpeed = 1.0f;
    protected Quaternion legR1Rotaion = Quaternion.Euler(0, 0, 0);
    protected float legR1RSpeed = 1.0f;
    protected Quaternion legR2Rotaion = Quaternion.Euler(0, 0, 0);
    protected float legR2RSpeed = 1.0f;
    protected Vector3 targtPos = new Vector3(0, 0, 0);
    protected float actionSpeed = 4.0f;
    protected GameObject legL1;
    protected GameObject legL2;
    protected GameObject legR1;
    protected GameObject legR2;
    protected void PartsMotion()
    {
        if (Body.unitType == UnitType.Human)
        {
            Head.transform.localRotation = Quaternion.Lerp(Head.transform.localRotation, headRotaion, headRSpeed * Time.deltaTime);
            Body.transform.localRotation = Quaternion.Lerp(Body.transform.localRotation, bodyRotaion, bodyRSpeed * Time.deltaTime);
            LArm.transform.localRotation = Quaternion.Lerp(LArm.transform.localRotation, lArmRotaion, lArmRSpeed * Time.deltaTime);
            RArm.transform.localRotation = Quaternion.Lerp(RArm.transform.localRotation, rArmRotaion, rArmRSpeed * Time.deltaTime);
            Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, legRotaion, legRSpeed * Time.deltaTime);
            Leg.transform.localPosition = Vector3.Lerp(Leg.transform.localPosition, targtPos, actionSpeed * Time.deltaTime);
            legL1.transform.localRotation = Quaternion.Lerp(legL1.transform.localRotation, legL1Rotaion, legL1RSpeed * Time.deltaTime);
            legL2.transform.localRotation = Quaternion.Lerp(legL2.transform.localRotation, legL2Rotaion, legL2RSpeed * Time.deltaTime);
            legR1.transform.localRotation = Quaternion.Lerp(legR1.transform.localRotation, legR1Rotaion, legR1RSpeed * Time.deltaTime);
            legR2.transform.localRotation = Quaternion.Lerp(legR2.transform.localRotation, legR2Rotaion, legR2RSpeed * Time.deltaTime);
        }
        else if (Body.unitType == UnitType.Helicopter)
        {
            Body.transform.localRotation = Quaternion.Lerp(Body.transform.localRotation, bodyRotaion, bodyRSpeed * Time.deltaTime);
        }

    }
    public void DamgeMotion()
    {
        if (Body.unitType == UnitType.Human)
        {
            headRotaion = Quaternion.Euler(0, 0, 0);
            headRSpeed = 2.0f;
            bodyRotaion = Quaternion.Euler(0, 0, 0);
            bodyRSpeed = 1.0f;
            lArmRotaion = Quaternion.Euler(0, 0, 0);
            lArmRSpeed = 2.0f;
            rArmRotaion = Quaternion.Euler(0, 0, 0);
            rArmRSpeed = 2.0f;
            legRotaion = Quaternion.Euler(0, 0, 0);
            legRSpeed = 2.0f;
            Body.transform.localRotation = Quaternion.Euler(-20, 0, 0);
            Leg.transform.localRotation = Quaternion.Euler(10, 0, 0);
            LArm.transform.localRotation = Quaternion.Euler(-5, 0, 10);
            RArm.transform.localRotation = Quaternion.Euler(-5, 0, -10);
            EffectManager.PlayEffect(EffectID.BreakParts, Body.transform.position);
        }
    }
    protected float moveMotionTimer = 0;
    protected float resetTimer = 0;
    protected bool moveMotionStart = false;
    protected void MoveMotion()
    {
        if (moveMood)
        {
            if (Body.unitType == UnitType.Human)
            {
                if (!moveMotionStart)
                {
                    legL1RSpeed = moveSpeed * 0.2f;
                    legL2RSpeed = moveSpeed * 0.2f;
                    legR1RSpeed = moveSpeed * 0.2f;
                    legR2RSpeed = moveSpeed * 0.2f;
                    lArmRSpeed = moveSpeed * 0.2f;
                    rArmRSpeed = moveSpeed * 0.2f;
                    bodyRSpeed = moveSpeed * 0.2f;
                    legRSpeed = moveSpeed * 0.2f;
                    headRSpeed = moveSpeed * 0.2f;
                    moveMotionStart = true;
                    moveMotionTimer = 0;
                    resetTimer = 0;
                }
                moveMotionTimer += moveSpeed * Time.deltaTime;
                if (moveMotionTimer <= 10)
                {
                    legL1Rotaion = Quaternion.Euler(40, 0, -5);
                    legL2Rotaion = Quaternion.Euler(-20, 0, 0);
                    legR1Rotaion = Quaternion.Euler(-20, 0, 5);
                    legR2Rotaion = Quaternion.Euler(-10, 0, 0);
                    lArmRotaion = Quaternion.Euler(-10, 0, 0);
                    rArmRotaion = Quaternion.Euler(10, 0, 0);
                    bodyRotaion = Quaternion.Euler(0, -30, 0);
                    legRotaion = Quaternion.Euler(-10, 20, 0);
                    headRotaion = Quaternion.Euler(0, 10, 0);
                }
                else if (moveMotionTimer <= 20)
                {
                    legL1Rotaion = Quaternion.Euler(-20, 0, -5);
                    legL2Rotaion = Quaternion.Euler(-10, 0, 0);
                    legR1Rotaion = Quaternion.Euler(40, 0, 5);
                    legR2Rotaion = Quaternion.Euler(-20, 0, 0);
                    lArmRotaion = Quaternion.Euler(10, 0, 0);
                    rArmRotaion = Quaternion.Euler(-10, 0, 0);
                    bodyRotaion = Quaternion.Euler(0, 30, 0);
                    legRotaion = Quaternion.Euler(-10, -20, 0);
                    headRotaion = Quaternion.Euler(0, -10, 0);
                }
                else
                {
                    moveMotionTimer = 0;
                }
            }
            else if (Body.unitType == UnitType.Helicopter)
            {
                if (!moveMotionStart)
                {
                    bodyRotaion = Quaternion.Euler(-20, 0, 0);
                    bodyRSpeed = 10.0f;
                    moveMotionStart = true;
                    moveMotionTimer = 0;
                }
            }
        }
        else if (moveMotionStart)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer > 0.5f)
            {
                moveMotionStart = false;
            }
            legL1Rotaion = Quaternion.Euler(0, 0, 0);
            legL2Rotaion = Quaternion.Euler(0, 0, 0);
            legR1Rotaion = Quaternion.Euler(0, 0, 0);
            legR2Rotaion = Quaternion.Euler(0, 0, 0);
            bodyRotaion = Quaternion.Euler(0, 0, 0);
            lArmRotaion = Quaternion.Euler(0, 0, 0);
            rArmRotaion = Quaternion.Euler(0, 0, 0);
            legRotaion = Quaternion.Euler(0, 0, 0);
            headRotaion = Quaternion.Euler(0, 0, 0);
        }
    }
    public void HitMotion()
    {
        bodyRSpeed = 3.0f;
        Body.transform.localRotation = Body.transform.localRotation * Quaternion.Euler(-2, 0, 0);
    }
    public void DeadMotion()
    {
        if (DestroyBody && bomCount < 6)
        {
            deadTimer += Time.deltaTime;
            if (deadTimer > 0.1f)
            {
                if (bomCount == 0 && deadTimer > 0.6f)
                {
                    EffectManager.PlayEffect(EffectID.Explosion, Body.GetBodyCentrer().position);
                    bomCount = 1;
                    deadTimer = 0;
                }
                else if (bomCount == 1 && deadTimer > 0.4f)
                {
                    EffectManager.PlayEffect(EffectID.Explosion, Body.GetHeadPos().position);
                    bomCount = 2;
                    deadTimer = 0;
                }
                else if (bomCount == 2 && deadTimer > 0.4f)
                {
                    EffectManager.PlayEffect(EffectID.Explosion, Body.GetLArmPos().position);
                    bomCount = 3;
                    deadTimer = 0;
                }
                else if (bomCount == 3 && deadTimer > 0.4f)
                {
                    legRSpeed = 2.0f;
                    EffectManager.PlayEffect(EffectID.Explosion, Body.GetRArmPos().position);
                    bomCount = 4;
                    deadTimer = 0;
                }
                else if (bomCount == 4 && deadTimer > 0.4f)
                {
                    EffectManager.PlayEffect(EffectID.Explosion, transform.position);
                    bomCount = 5;
                    deadTimer = 0;
                }
                else if (bomCount == 5 && deadTimer > 0.8f)
                {
                    EffectManager.PlayEffect(EffectID.HyperExplosion, Body.GetBodyCentrer().position);
                    gameObject.SetActive(false);
                    bomCount = 6;
                }
            }
        }
    }
    public void OnClickThis()
    {
        TargetCursor.instance.SetCursor(this);
    }
}
