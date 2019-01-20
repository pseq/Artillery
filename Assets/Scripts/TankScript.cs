using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
public float speed = 1f;
WheelJoint2D leftWheel, rightWheel;
Rigidbody2D leftWheelRigid, rightWheelRigid;
public GameObject target;
public float angleSearchStepGrad = 5f;
public GunScript gunScript;
float leftAimPoint;
float rightAimPoint;
float lastHitPoint;
float lastEnemyPosition;
float gunPower;
public float maxGunPower = 25;
bool isFirst = true;
bool moveDirChanged = false;
float cantMovinDelta  = 5f;
float changePositionDistance  = 1f;
int moveDirection = 1;
float lastGunPower;
public float minShootAngle = -360;
public float maxShootAngle = 360;
public float  angleChandeAccuracy = .1f;
public float gunRotSpeed = 30f;

////// test
public GameObject test;
public int floorAngleSign = 1;
public int side = 1;


    // Start is called before the first frame update
    void Start()
    {
        GameObject leftWheelObj = transform.GetChild(1).gameObject;
        GameObject rightWheelObj = transform.GetChild(2).gameObject;
        rightWheel = rightWheelObj.GetComponent<WheelJoint2D>();
        leftWheel = leftWheelObj.GetComponent<WheelJoint2D>();
        rightWheelRigid = rightWheelObj.GetComponent<Rigidbody2D>();
        leftWheelRigid = leftWheelObj.GetComponent<Rigidbody2D>();
// TODO сделать выбор из размеров terrain
        leftAimPoint = 0;
        //rightAimPoint = GameObject.Find("Terrain").GetComponent<Mesh>().bounds.center.x;
        rightAimPoint = 150;
        lastHitPoint = target.transform.position.x;
        lastEnemyPosition = lastHitPoint;
        //gunPower = gunScript.GunPowerToPoint(target.transform.position, 45f);
        gunPower = 10;

        //Debug.Log("TANK TANK angle " + transform.eulerAngles.z);
        //Debug.Log("TANK GUN angle " + gunScript.transform.eulerAngles.z);

    }

    public void Move(float dist) {
        // TODO формула вычисления времени из расстояния
        float time = dist/speed;
        StartCoroutine(MoveCoroutine(time));
    }

    bool ChangePosition() {
        lastGunPower = gunPower;

        float startPos = transform.position.x;
        Move(changePositionDistance * moveDirection);
        isFirst = false;
        if (Mathf.Abs(startPos - transform.position.x) > 0) return true;
        else return false;
    }
    
    public void Aim() {

        Transform selfTransform = gameObject.transform;
        Transform enemyTransform = target.transform;
        TurnaroundToEnemy(selfTransform, enemyTransform);

        side = (int)Mathf.Sign(enemyTransform.position.x - selfTransform.position.x);
        // угол между танком и противником
        float floorAngle = Mathf.Atan2(side * (enemyTransform.position.y - selfTransform.position.y),
                                       side * (enemyTransform.position.x - selfTransform.position.x)) * Mathf.Rad2Deg * side;

        float distance = Vector2.Distance(selfTransform.position, enemyTransform.position);
        float alpha = ShootAngleSearch(gameObject, distance, side) - floorAngle;
        float beta  = ShootAngleSearch(target, distance, -side) + floorAngle;
        float angle = (Mathf.Max(alpha, beta) + floorAngle) * side;

 Debug.DrawLine(selfTransform.position, Quaternion.Euler(0, 0, angle) * Vector2.right * 100 + selfTransform.position, Color.white);

        // Если угол больше 45 - добавить ещё половину
        float to90angle = Mathf.Abs(Mathf.DeltaAngle(90, angle));
        Debug.Log("TANK to90angle " + to90angle);
        if (to90angle < 45) {
        Debug.Log("TANK to90angle < 45");

            angle += to90angle/2 * side;
        }



 Debug.DrawLine(selfTransform.position, Quaternion.Euler(0, 0, angle) * Vector2.right * 100 + selfTransform.position, Color.green);
        
    Debug.Break();

        // чтобы мощность выстрела не превышала максимальную, увеличиваем угол до 45
        //while (gunScript.GunPowerToPoint(target.transform.position, angle) > maxGunPower && angle < 45f) angle += angleSearchStepGrad;
        
        StartCoroutine(AngleChangeCoroutine(angle));


    }

    void ChangePositionLogicModule(float angle) {
        // Power Calc to Enemy
        gunPower =  gunScript.GunPowerToPoint(target.transform.position, angle, side);

        //test
        AiminOK(angle);
        return;

        if (gunPower <= maxGunPower) {
            // завершаем алгоритм
            //Debug.Log("TANK gunPower <= maxGunPower AiminOK");
            AiminOK(angle);
            return;
        } 
        else if (!isFirst) {
            if (gunPower > lastGunPower) {
                if (ChangeMovinDirection()) {
                    //Debug.Log("TANK gunPower > lastGunPower AiminOK");

                    AiminOK(angle);
                    return;
                } else if (ChangePosition()) Aim(); // рекурсия            
            } else if (ChangePosition()) Aim();    
        } else if (ChangePosition()) Aim(); // рекурсия
    }

    bool ChangeMovinDirection() {
        moveDirection *= -1;
        if (!ChangePosition() && !moveDirChanged) {
            moveDirChanged = true;
            ChangeMovinDirection();
        }
        return moveDirChanged;
    }

    IEnumerator AngleChangeCoroutine(float newAngle) {
 Debug.DrawLine(transform.position, Quaternion.Euler(0, 0, newAngle) * Vector2.right * 80 + transform.position, Color.magenta);

        bool isRotated = false;
        float critAngle = transform.eulerAngles.z + gunScript.maxGunAngle;
        float delta90lastAngle = Mathf.DeltaAngle(critAngle, gunScript.transform.eulerAngles.z);
        Debug.Log("GUN critAngle " + critAngle);
        while (Mathf.Abs(Mathf.DeltaAngle(newAngle, gunScript.transform.eulerAngles.z)) > angleChandeAccuracy) { //TODO вывести трансформ
        float angle = Mathf.MoveTowardsAngle(gunScript.transform.eulerAngles.z, newAngle, gunRotSpeed * Time.deltaTime);
        gunScript.transform.eulerAngles = new Vector3(0, 0, angle);

        if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, gunScript.transform.eulerAngles.z)) > gunScript.maxGunAngle && !isRotated) {
            gunScript.TurnAround();
            isRotated = true;
            newAngle += 180;
            }
        
        yield return new WaitForSeconds(Time.deltaTime);
        }
        
        ChangePositionLogicModule(gunScript.transform.eulerAngles.z);
    }

    void AiminOK(float angle) {
        // SET GUN AIM
        isFirst = true;
        moveDirChanged = false;

        //gunScript.transform.eulerAngles = new Vector3(0f,0f,angle);

        //TODO сделать корутин вращения пушки
        //gunScript.GunAngleChange(angle);

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

    float ShootAngleSearch(GameObject self, float distance, int course) {
        LayerMask mask = LayerMask.GetMask("Terrain");
        Transform selfTransform = self.transform;
        for (float a = 90 - 180 * course; (90 - 90 * course + a * course) <= 90; a += angleSearchStepGrad * course) {
            Vector2 direction = Quaternion.Euler(0, 0, a) * Vector2.right;
            RaycastHit2D rkHit = Physics2D.Raycast(selfTransform.position, direction, distance, mask);
    Debug.DrawLine(selfTransform.position, direction * 100 + (Vector2)selfTransform.position, Color.blue);
    if(rkHit) Debug.DrawLine(selfTransform.position, rkHit.point, Color.yellow);
            // возвращаем всегда угол -90 .. 90
            //if(!rkHit) return (90 - 90 * course + a * course);
            if(!rkHit) {
             Debug.DrawLine(selfTransform.position, Quaternion.Euler(0, 0, (90 - 90 * course + a * course)) * Vector2.right * 120 + selfTransform.position, Color.red);
             return (90 - 90 * course + a * course);

            }
        }
        Debug.Log("TANK ShootAngleSearch return 0");
        return 0;
    }

    void TurnaroundToEnemy(Transform self, Transform target) {
        if (target.position.x > self.position.x && self.localScale.x < 0) gunScript.TurnAround(); 
        if (target.position.x < self.position.x && self.localScale.x > 0) gunScript.TurnAround();
    }

    IEnumerator MoveCoroutine(float time) {
        // move
        rightWheelRigid.freezeRotation = false;
        leftWheelRigid.freezeRotation = false;
        if (time > 0) leftWheel.useMotor = true;
        else rightWheel.useMotor = true;
        //wait
        //Debug.Log("TANK START MOVE " + transform.name);

        yield return new WaitForSeconds(Mathf.Abs(time));
        // stop
        //Debug.Log("TANK STOP MOVE " + transform.name);

        rightWheelRigid.freezeRotation = true;
        leftWheelRigid.freezeRotation = true;
        rightWheel.useMotor = false;
        leftWheel.useMotor = false;
    }
}
