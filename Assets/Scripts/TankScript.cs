using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction : int {
    Left = -1,
    Right = 1
}
public class TankScript : MonoBehaviour
{
public GameObject target;
public float angleSearchStepGrad = 5f;
public float angleSearchAccuracy = 1f;
public GunScript gunScript;
float leftAimPoint;
float rightAimPoint;
float lastHitPoint;
float lastEnemyPosition;
float gunPower;
public float maxGunPower = 25;
public float  angleChandeAccuracy;

float gunAngleSpeed = 0f;
int gunAngleCylesLimit = 200;
public float gunRotTime = .5f;

public TerrainScript terrainScript;
Transform selfTransform;
Transform enemyTransform;
TankAIScript aIScript;


////// test
public GameObject test1;
public GameObject test2;
public int side = 1;


    // Start is called before the first frame update
    void Start()
    {

// TODO сделать выбор из размеров terrain
        leftAimPoint = 0;
        terrainScript = GameObject.Find("Terrain").GetComponent<TerrainScript>();
        rightAimPoint = 150;
        lastHitPoint = target.transform.position.x;
        lastEnemyPosition = lastHitPoint;
        gunPower = 10;

        //Debug.Log("TANK TANK angle " + transform.eulerAngles.z);
        //Debug.Log("TANK GUN angle " + gunScript.transform.eulerAngles.z);
        selfTransform = gameObject.transform;
        enemyTransform = target.transform;
        aIScript = GetComponent<TankAIScript>();
    }

/*
    public void setTurn() {
        myTurn = true;
        tanks = FindObjectsOfType<TankScript>();
        foreach (TankScript tank in tanks)
            if (tank != this) tank.unsetTurn();
    }

    public void unsetTurn() {
        myTurn = false;
    }

    public bool getTurn() {
        return myTurn;
    }
*/

    
    public void Aim() {

        //Transform selfTransform = gameObject.transform;
        Transform enemyTransform = target.transform;
        side = (int)Mathf.Sign(enemyTransform.position.x - transform.position.x);
        ShootAngleSearch();
    }

    IEnumerator AngleChangeCoroutine(float newAngle) {
        int i = 0;
        float last = gunScript.maxGunAngle;
        // пока новый угол пушки и угол, до которого нужно довернуть, не сравняются ...
        while (Mathf.Abs(Mathf.DeltaAngle(newAngle, gunScript.transform.eulerAngles.z)) > angleChandeAccuracy && i < gunAngleCylesLimit) { //TODO вывести трансформ
        // ... плавно доворачиваем пушку
        i++;
        float angle = Mathf.SmoothDampAngle(gunScript.transform.eulerAngles.z , newAngle, ref gunAngleSpeed, gunRotTime);
        gunScript.transform.eulerAngles = new Vector3(0, 0, angle);

        // при пересечении пушкой 90 - разворачиваем танк
        if ((Mathf.Abs(Mathf.DeltaAngle(gunScript.transform.eulerAngles.z, transform.eulerAngles.z) ) > gunScript.maxGunAngle &&
            last < gunScript.maxGunAngle) ||
            (Mathf.Abs(Mathf.DeltaAngle(gunScript.transform.eulerAngles.z, transform.eulerAngles.z) ) < gunScript.maxGunAngle &&
            last > gunScript.maxGunAngle)
            ) {
            gunScript.TurnAround();
            }

        last = Mathf.Abs(Mathf.DeltaAngle(gunScript.transform.eulerAngles.z, transform.eulerAngles.z));
        
        yield return new WaitForSeconds(Time.deltaTime);
        }
        Fire();
    }

    void Fire() {
        //aIScript.ShootOK();
        gunScript.Fire();
    }

    public void SetLastHitPoint(float x) {
        lastHitPoint = x;
    }

    float ShootDistanceSelect() {

Debug.Log("lastHitPoint:" + Mathf.Round(lastHitPoint) + " enemyTransform.position.x:" + Mathf.Round(enemyTransform.position.x));


        float enemyPosition = target.transform.position.x;
        if (Mathf.Round(lastHitPoint) > Mathf.Round(enemyTransform.position.x)) {
        //if (RoundedHitBigestEnemy()) {
            Debug.Log("RoundedHitBigestEnemy");
            rightAimPoint = lastHitPoint;
            leftAimPoint = enemyTransform.position.x - lastEnemyPosition + leftAimPoint;
        }
        if (Mathf.Round(lastHitPoint) < Mathf.Round(enemyTransform.position.x)) {
        //if (!RoundedHitBigestEnemy()) {
            Debug.Log("RoundedHitSmalestEnemy");
            leftAimPoint = lastHitPoint;
            rightAimPoint = enemyTransform.position.x - lastEnemyPosition + rightAimPoint;
        }

        //if (Mathf.Approximately(lastHitPoint,enemyTransform.position.x)) Debug.Log("ON BEGIN");


        lastEnemyPosition = enemyTransform.position.x;


        test1.transform.position = new Vector2(leftAimPoint, 50);
        test2.transform.position = new Vector2(rightAimPoint, 50);
        //return enemyTransform.position;
        return Random.Range(leftAimPoint, rightAimPoint);
    }

    bool RoundedHitBigestEnemy() {
        return (Mathf.Round(lastHitPoint) > Mathf.Round(enemyTransform.position.x));
    }

    void ShootAngleSearch() {
        // выбираем точку прицеливания по алгоритму промаха
        Vector2 aimPoint = new Vector2(ShootDistanceSelect(), enemyTransform.position.y);
        // высочайшая точка поверхности между танками
        Vector2 hiTerrPoint = terrainScript.HighestPointBetween(selfTransform.position, aimPoint);
        // если side - вправо - 0, если влево +180
        float angleSide = -(90 * side - 90);
        float angle = selfTransform.eulerAngles.z + angleSide;
        // делаем, пока угол между направлением выстрела, и наклоном танка > 0, учитывая сторону, в которую будем стрелять
        while(Mathf.Abs(Mathf.DeltaAngle(angle, selfTransform.eulerAngles.z - angleSide + 180)) > angleSearchAccuracy) {
            float powerHi = gunScript.GunPowerToPoint(hiTerrPoint, angle, side);
            float powerTg = gunScript.GunPowerToPoint(aimPoint, angle, side);
    //Debug.DrawLine(selfTransform.position, Quaternion.Euler(0, 0, angle) * Vector2.right * 120 + selfTransform.position, Color.yellow);

            // сравниваем мощности, чтобы добить до самой высокой точки, и до цели с макс мощностью
            if (powerTg < maxGunPower && powerTg > powerHi) {
    //Debug.Log("TANK Angle search " + angle + " powerTG " + powerTg);
                StartCoroutine(AngleChangeCoroutine(angle));
    //Debug.DrawLine(selfTransform.position, Quaternion.Euler(0, 0, angle) * Vector2.right * 110 + selfTransform.position, Color.red);
    //Debug.Break();
                return;  
            }
        // инкремент или декремент угла выстрела
        angle += angleSearchStepGrad * side;
    //Debug.DrawLine(selfTransform.position, Quaternion.Euler(0, 0, angle) * Vector2.right * 120 + selfTransform.position, Color.yellow);

        }
        // говорим ИИ, что достать противника не получится
        aIScript.CantShoot();
    }
}
