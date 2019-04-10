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
bool isFirst = true;
bool moveDirChanged = false;
int moveDirection = 1;
float lastGunPower;
public float  angleChandeAccuracy;

float gunAngleSpeed = 0f;
public float gunRotTime = .5f;

public TerrainScript terrainScript;
Transform selfTransform;
Transform enemyTransform;
TankAIScript aIScript;
//public bool myTurn;
TankScript[] tanks;



////// test
//public GameObject test;
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

        Transform selfTransform = gameObject.transform;
        Transform enemyTransform = target.transform;
        side = (int)Mathf.Sign(enemyTransform.position.x - selfTransform.position.x);
        ShootAngleSearch();
    }

    IEnumerator AngleChangeCoroutine(float newAngle) {
        // TODO найти баг определения угла

        // после разворота танка перевернуть все углы на 180
        float angleSide = 90 * gunScript.forwardDirection - 90;

        // пока новый угол пушки и угол, до которого нужно довернуть, не сравняются ...
        while (Mathf.Abs(Mathf.DeltaAngle(newAngle, gunScript.transform.eulerAngles.z - angleSide)) > angleChandeAccuracy) { //TODO вывести трансформ
        // TODO добавить параметров для более натурального разворота пушки
        // ... плавно доворачиваем пушку
        float angle = Mathf.SmoothDampAngle(gunScript.transform.eulerAngles.z , newAngle - angleSide, ref gunAngleSpeed, gunRotTime);
        gunScript.transform.eulerAngles = new Vector3(0, 0, angle);

        // при пересечении пушкой 90 - разворачиваем танк
        if (Mathf.Abs(Mathf.DeltaAngle(gunScript.transform.eulerAngles.z, transform.eulerAngles.z) ) > gunScript.maxGunAngle) {
            gunScript.TurnAround();
            newAngle += 180;
            }
        
        yield return new WaitForSeconds(Time.deltaTime);
        }
    //Debug.Log("TANK: REal angle=" + gunScript.transform.eulerAngles.z);
        Fire();
    }

    void Fire() {
        //aIScript.ShootOK();
        gunScript.Fire();
    }

    float ShootDistanceSelect() {
        float enemyPosition = target.transform.position.x;
        if (lastHitPoint > enemyPosition) {
            rightAimPoint = lastHitPoint;
            leftAimPoint = enemyPosition - lastEnemyPosition + leftAimPoint;
        }
        if (lastHitPoint < enemyPosition) {
            leftAimPoint = lastHitPoint;
            rightAimPoint = enemyPosition - lastEnemyPosition + rightAimPoint;
        }
        lastEnemyPosition = enemyPosition;
        return enemyPosition;
        //return Random.Range(leftAimPoint, rightAimPoint);
    } 

    void ShootAngleSearch() {
        // высочайшая точка поверхности между танками
        Vector2 hiTerrPoint = terrainScript.HighestPointBetween(selfTransform.position, enemyTransform.position);
        // если side - вправо - 0, если влево +180
        float angleSide = -(90 * side - 90);
        float angle = selfTransform.eulerAngles.z + angleSide;
        // делаем, пока угол между направлением выстрела, и наклоном танка > 0, учитывая сторону, в которую будем стрелять
        while(Mathf.Abs(Mathf.DeltaAngle(angle, selfTransform.eulerAngles.z - angleSide + 180)) > angleSearchAccuracy) {
            float powerHi = gunScript.GunPowerToPoint(hiTerrPoint, angle, side);
            float powerTg = gunScript.GunPowerToPoint(enemyTransform.position, angle, side);
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
